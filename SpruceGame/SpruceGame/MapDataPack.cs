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
        public MapDataPack(Dictionary<string, Texture2D> textureDict)
        {
            this.textureDict = textureDict;
        }
    }
}
