using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    public class SaveGame
    {
        // - - - - Variables Global to this Save
        readonly string SaveName;
        byte[] Seed;
        // - - - - - - - - - - - - - - - - - - -

        public SaveGame(byte[] Seed) //sub new
        {
            this.Seed = Seed;
        }

        public void Update() //backend
        {

        }

        public void Draw(SpriteBatch spriteBatch) //frontend
        {

        }
    }
}
#pragma warning restore CS0618