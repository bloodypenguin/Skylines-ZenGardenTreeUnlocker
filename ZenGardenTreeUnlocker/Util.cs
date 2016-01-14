using System;
using System.IO;
using ColossalFramework.UI;
using UnityEngine;

namespace ZenGardenTreeUnlocker
{
    public static class Util
    {
        public static Texture2D LoadTextureFromAssembly(string path, bool readOnly = true)
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                using (var textureStream = assembly.GetManifestResourceStream(path))
                {
                    return LoadTextureFromStream(readOnly, textureStream);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
                return null;
            }
        }

        public static UITextureAtlas CreateAtlas(Texture2D[] sprites)
        {
            UITextureAtlas atlas = new UITextureAtlas();
            atlas.material = new Material(GetUIAtlasShader());

            Texture2D texture = new Texture2D(0, 0);
            Rect[] rects = texture.PackTextures(sprites, 0);

            for (int i = 0; i < rects.Length; ++i)
            {
                Texture2D sprite = sprites[i];
                Rect rect = rects[i];

                UITextureAtlas.SpriteInfo spriteInfo = new UITextureAtlas.SpriteInfo();
                spriteInfo.name = sprite.name;
                spriteInfo.texture = sprite;
                spriteInfo.region = rect;
                spriteInfo.border = new RectOffset();

                atlas.AddSprite(spriteInfo);
            }
            atlas.material.mainTexture = texture;
            return atlas;
        }

        private static Shader GetUIAtlasShader()
        {
            return UIView.GetAView().defaultAtlas.material.shader;
        }

        private static Texture2D LoadTextureFromStream(bool readOnly, Stream textureStream)
        {
            var buf = new byte[textureStream.Length]; //declare arraysize
            textureStream.Read(buf, 0, buf.Length); // read from stream to byte array
            textureStream.Close();
            var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(buf);
            tex.Apply(false, readOnly);
            tex.name = Guid.NewGuid().ToString();
            return tex;
        }
    }
}