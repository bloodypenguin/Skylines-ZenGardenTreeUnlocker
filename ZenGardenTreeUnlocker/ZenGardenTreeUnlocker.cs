using System.Reflection;
using ColossalFramework;
using ColossalFramework.Globalization;
using ICities;
using UnityEngine;

namespace ZenGardenTreeUnlocker
{
    public class ZenGardenTreeUnlocker : LoadingExtensionBase, IUserMod
    {
        public string Name => "Zen Garden Cherry Blossom Unlocker";

        public string Description => "Unlocks for placing the Cherry Blossom tree that is bundled with Zen Garden";

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            var prefab = PrefabCollection<TreeInfo>.FindLoaded("CherryTree01");
            if (prefab == null)
            {
                UnityEngine.Debug.LogWarning("CherryTree01 wasn't found");
                return;
            }
            prefab.m_availableIn = ItemClass.Availability.All;
            var field = typeof (PrefabInfo).GetField("m_UICategory", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(prefab, "BeautificationProps");
            var thumb = Util.LoadTextureFromAssembly($"{typeof (ZenGardenTreeUnlocker).Name}.thumb.png", false);
            var tooltip = Util.LoadTextureFromAssembly($"{typeof(ZenGardenTreeUnlocker).Name}.tooltip.png", false);
            var atlas = Util.CreateAtlas(new[] {thumb, tooltip});
            prefab.m_Atlas = atlas;
            prefab.m_Thumbnail = thumb.name;
            prefab.m_InfoTooltipAtlas = atlas;
            prefab.m_InfoTooltipThumbnail = tooltip.name;


            var localeField = typeof(LocaleManager).GetField("m_Locale", BindingFlags.NonPublic | BindingFlags.Instance);
            var locale = (Locale)localeField.GetValue(SingletonLite<LocaleManager>.instance);
            var key = new Locale.Key() { m_Identifier = "TREE_TITLE", m_Key = prefab.name };
            if (!locale.Exists(key))
            {
                locale.AddLocalizedString(key, "Zen Garden Cherry Blossom");
            }
            key = new Locale.Key() { m_Identifier = "TREE_DESC", m_Key = prefab.name };
            if (!locale.Exists(key))
            {
                locale.AddLocalizedString(key, "A Cherry Blossom tree that is bundled with Zen Garden");
            }

            switch (mode)
            {
                case LoadMode.NewGame:
                case LoadMode.LoadGame:
                    GameObject.Find("BeautificationPropsPanel").GetComponent<BeautificationPanel>().RefreshPanel();
                    break;
                case LoadMode.NewMap:
                case LoadMode.LoadMap:
                    GameObject.Find("ForestPanel").GetComponent<ForestGroupPanel>().RefreshPanel();
                    break;
            }
        }
    }
}
