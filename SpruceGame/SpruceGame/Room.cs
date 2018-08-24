using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System;
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    [Serializable]
    public class Tile
    {
        // - - - - Variables Global to this tile
        public bool isSolid;
        string textureKey;
        // - - - - - - - - - - - - - - - - - - -
        public Tile(string textureKey, bool isSolid)
        {
            this.textureKey = textureKey;
            this.isSolid = isSolid;
        }
        public void draw(SpriteBatch spritebatch, Coord Position, Dictionary<string, Texture2D> TextureDict)
        {
            spritebatch.Draw(TextureDict[textureKey], Position.ToVector2());
        }
    }
    [Serializable]
    public class Room
    {
        // - - - - Variables Global to this Room
        public Tile[,] tiles;
        public List<Container> Containers;
        int width;
        int height;

        // - - - - - - - - - - - - - - - - - - -
        public Room(int width, int height,Dictionary<string,Texture2D> TextureDict,byte DoorProfile)
        {
            this.width = width;
            this.height = height;
            Containers = new List<Container> { new Container("Container","MenuTemplate",new Coord(90,32*11),new List<Item> { },48)};
            tiles = new Tile[width, height];
            tiles[0, 0] = new Tile("WallTopLeft", true);
            for (int x = 1; x < width-1; x++)
            {
                tiles[x, 0] = new Tile("WallTop", true);
            }
            tiles[width-1, 0] = new Tile("WallTopRight", true);
            for (int y = 1; y < height - 1; y++)
            {
                tiles[width-1, y] = new Tile("WallRight", true);
            }
            tiles[width-1, height-1] = new Tile("WallBottomRight", true);
            for (int x = 1; x < width - 1; x++)
            {
                tiles[x, height-1] = new Tile("WallBottom", true);
            }
            tiles[0, height-1] = new Tile("WallBottomLeft", true);
            for (int y = 1; y < height - 1; y++)
            {
                tiles[0, y] = new Tile("WallLeft", true);
            }
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    tiles[x, y] = new Tile("WallMiddle", false);
                }
            }
            if ((DoorProfile & 0b1) == 0b1)
            {
                tiles[width / 2, 0] = new Tile("WallMiddle", false);
            }
            if ((DoorProfile & 0b10) == 0b10)
            {
                tiles[0, height/2] = new Tile("WallMiddle", false);
            }
            if ((DoorProfile & 0b100) == 0b100)
            {
                tiles[width / 2, height-1] = new Tile("WallMiddle", false);
            }
            if ((DoorProfile & 0b1000) == 0b1000)
            {
                tiles[width-1, height / 2] = new Tile("WallMiddle", false);
            }
        }
        public void Draw(SpriteBatch spriteBatch, Coord position, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> TextureDict)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y].draw(spriteBatch,position+new Coord(x*32,y*32),TextureDict);
                }
            }
            foreach (Container container in Containers)
            {
                container.Draw(spriteBatch, position,graphicsDevice,TextureDict);
            }
        }
        public void Update(MouseState mouseState, Coord position)
        {
            foreach (Container container in Containers)
            {
                container.Update(mouseState, position);
            }
        }
    }
}
#pragma warning restore CS0618