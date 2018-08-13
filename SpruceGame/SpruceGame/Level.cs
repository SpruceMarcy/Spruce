using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    public class Level
    {
        // - - - - Variables Global to this Level
        readonly string LevelName;
        public Room[,] rooms;
        int width;
        int height;

        // - - - - - - - - - - - - - - - - - - -
        public Level(int width, int height, Dictionary<string, Texture2D> TextureDict)
        {
            this.width = width;
            this.height = height;
            rooms = new Room[width, height];
            rooms[0, 0] = new Room(16, 16, TextureDict);
        }
        public void draw(SpriteBatch spriteBatch, Vector2 Position)
        {
            foreach (Room room in rooms)
            {
                if (room!=null)
                {
                    room.draw(spriteBatch, Position);
                }
            }
        }
    }
}
#pragma warning restore CS0618