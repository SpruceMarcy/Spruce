using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System;//MB: Allows use of [Serializable]
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    [Serializable]//MB: This allows an instance of this class to be written to file
    public class Tile
    {
        // - - - - Variables Global to this tile
        public bool isSolid;//MB: Determines whether the player can walk on his tile
        string textureKey;//MB: Used with a texture dictionary to get the texture of this tile
        // - - - - - - - - - - - - - - - - - - -
        public Tile(string textureKey, bool isSolid)
        {
            this.textureKey = textureKey;
            this.isSolid = isSolid;
        }
        public void Draw(SpriteBatch spritebatch, Coord Position, Dictionary<string, Texture2D> TextureDict)
        {
            spritebatch.Draw(TextureDict[textureKey], Position.ToVector2());
        }
    }
    [Serializable]//MB: This allows an instance of this class to be written to file
    public class Room
    {
        // - - - - Variables Global to this Room
        public Tile[,] tiles;//MB: The collection of tiles that make up the room
        public List<Container> Containers;//MB: The collection of containers confined to this room
        int width;//MB: The width of the room in tiles
        int height;//MB: The height of the room in tiles

        // - - - - - - - - - - - - - - - - - - -
        public Room(int width, int height, byte DoorProfile)
        {
            this.width = width;
            this.height = height;
            Containers = new List<Container> { new Container("Container","MenuTemplate",new Coord(90,32*11),new List<Item> { },48)};//MB: Test container
            tiles = new Tile[width, height];
            //MB: The following is a painstaking method of assigning tiles based on location
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
            //MB: ...Sorry.
            //MB: Door profiles are a way of combining boolean values. Think of each bit in the byte as its own boolean variable
            //MB: The LSB determines if there is a door at the top of a room
            //MB: The 4 most significant bits are unused
            //MB: Therefore the format of a door profile is the following:
            //MB: 0:0:0:0:RightDoor:BottomDoor:LeftDoor:TopDoor
            //MB: For example, 00001010 represents a room with doors on the left and right.
            if ((DoorProfile & 0b1) == 0b1)
            {
                int midwidth = width / 2;
                tiles[midwidth + 1, 0] = new Tile("WallBottomLeftInv", true);
                tiles[midwidth, 0] = new Tile("WallMiddle", false);
                tiles[midwidth-1, 0] = new Tile("WallMiddle", false);
                tiles[midwidth-2, 0] = new Tile("WallBottomRightInv", true);
            }
            if ((DoorProfile & 0b10) == 0b10)
            {
                int midheight = height / 2;
                tiles[0, midheight+1] = new Tile("WallTopRightInv", true);
                tiles[0, midheight] = new Tile("WallMiddle", false);
                tiles[0, midheight-1] = new Tile("WallMiddle", false);
                tiles[0, midheight-2] = new Tile("WallBottomRightInv", true);
            }
            if ((DoorProfile & 0b100) == 0b100)
            {
                int midwidth = width / 2;
                int bottom = height - 1;
                tiles[midwidth + 1, bottom] = new Tile("WallTopLeftInv", true);
                tiles[midwidth, bottom] = new Tile("WallMiddle", false);
                tiles[midwidth - 1, bottom] = new Tile("WallMiddle", false);
                tiles[midwidth - 2, bottom] = new Tile("WallTopRightInv", true);
            }
            if ((DoorProfile & 0b1000) == 0b1000)
            {
                int midheight = height / 2;
                int right = width - 1;
                tiles[right, midheight + 1] = new Tile("WallTopLeftInv", true);
                tiles[right, midheight] = new Tile("WallMiddle", false);
                tiles[right, midheight - 1] = new Tile("WallMiddle", false);
                tiles[right, midheight - 2] = new Tile("WallBottomLeftInv", true);
            }
        }
        public void Draw(SpriteBatch spriteBatch, Coord position, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> TextureDict)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y].Draw(spriteBatch,position+new Coord(x*32,y*32),TextureDict);//MB: Draws each tile, one by one
                }
            }
            foreach (Container container in Containers)
            {
                container.Draw(spriteBatch, position,graphicsDevice,TextureDict);//MB: Draws each container in this room
            }
        }
        public void Update(MouseState mouseState, Coord position)
        {
            foreach (Container container in Containers)
            {
                container.Update(mouseState, position);//MB: Runs the game logic for each container
            }
        }
    }
}
#pragma warning restore CS0618