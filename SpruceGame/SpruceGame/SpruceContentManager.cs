using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpruceGame
{
    static class SpruceContentManager
    {
        private static List<Texture2D> texture2Ds = new List<Texture2D>();
        public static int newTexture2D(Texture2D texture2D)
        {
            texture2Ds.Add(texture2D);
            return texture2Ds.Count()-1;
        }
        public static Texture2D get(int key)
        {
            return texture2Ds[key];
        } 
        public static void Dispose()
        {
            foreach (Texture2D texture2D in texture2Ds)
            {
                texture2D.Dispose();
            }
            texture2Ds.Clear();
        }
    }
}
