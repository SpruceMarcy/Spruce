using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpruceGame
{
    [Serializable]//MB: This means that an instance of this class can be written to file
    public class MapDataPack
    {
        public Dictionary<string, Texture2D> textureDict;
        public MapDataPack(Texture2D wallPack)
        {
            this.textureDict = new Dictionary<string, Texture2D>();
            string[] keys = new string[]{"WallTopLeft",
                "WallTopRight",
                "WallTopLeftInv",
                "WallTopRightInv",
                "WallBottomLeft",
                "WallBottomRight",
                "WallBottomLeftInv",
                "WallBottomRightInv",
                "WallTop",
                "WallRight",
                "WallMiddle",
                "WallSolid",
                "WallLeft",
                "WallBottom",
                "WallBlank",
                "WallClear"};
            for (int y = 0; y < 128; y+=32)
            {
                for (int x = 0; x < 128; x+=32)
                {
                    this.textureDict.Add(keys[textureDict.Count],wallPack.Crop(new Rectangle(x,y,32,32)));
                }
            }
        }
    }
}
