using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    public class Tile
    {
        // - - - - Variables Global to this tile
        public bool isSolid;
        Texture2D texture;
        // - - - - - - - - - - - - - - - - - - -
        public Tile(Texture2D texture, bool isSolid)
        {
            this.texture = texture;
            this.isSolid = isSolid;
        }
        public void draw(SpriteBatch spritebatch, Vector2 Position)
        {
            spritebatch.Draw(texture, Position);
        }
    }
    public class Room
    {
        // - - - - Variables Global to this Room
        public Tile[,] tiles;
        int width;
        int height;

        // - - - - - - - - - - - - - - - - - - -
        public Room(int width, int height,Dictionary<string,Texture2D> TextureDict,byte DoorProfile)
        {
            this.width = width;
            this.height = height;
            tiles = new Tile[width, height];
            tiles[0, 0] = new Tile(TextureDict["WallTopLeft"], true);
            for (int x = 1; x < width-1; x++)
            {
                tiles[x, 0] = new Tile(TextureDict["WallTop"], true);
            }
            tiles[width-1, 0] = new Tile(TextureDict["WallTopRight"], true);
            for (int y = 1; y < height - 1; y++)
            {
                tiles[width-1, y] = new Tile(TextureDict["WallRight"], true);
            }
            tiles[width-1, height-1] = new Tile(TextureDict["WallBottomRight"], true);
            for (int x = 1; x < width - 1; x++)
            {
                tiles[x, height-1] = new Tile(TextureDict["WallBottom"], true);
            }
            tiles[0, height-1] = new Tile(TextureDict["WallBottomLeft"], true);
            for (int y = 1; y < height - 1; y++)
            {
                tiles[0, y] = new Tile(TextureDict["WallLeft"], true);
            }
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    tiles[x, y] = new Tile(TextureDict["WallMiddle"], false);
                }
            }
            if ((DoorProfile & 0b1) == 0b1)
            {
                tiles[width / 2, 0] = new Tile(TextureDict["WallMiddle"], false);
            }
            if ((DoorProfile & 0b10) == 0b10)
            {
                tiles[0, height/2] = new Tile(TextureDict["WallMiddle"], false);
            }
            if ((DoorProfile & 0b100) == 0b100)
            {
                tiles[width / 2, height-1] = new Tile(TextureDict["WallMiddle"], false);
            }
            if ((DoorProfile & 0b1000) == 0b1000)
            {
                tiles[width-1, height / 2] = new Tile(TextureDict["WallMiddle"], false);
            }
        }
        public void draw(SpriteBatch spriteBatch, Vector2 position)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y].draw(spriteBatch,position+new Vector2(x*32,y*32));
                }
            }
        }
    }
}
#pragma warning restore CS0618