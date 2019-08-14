using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static SpruceGame.GlobalMethods;
using System.Collections.Generic;///MB: Imports dictionaries
using System;//MB: Allows use of [Serializable]
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    [Serializable]//MB: This allows an instance of this class to be written to file
    public class Tile
    {
        // - - - - Variables Global to this tile
        public Hitbox hitbox;//MB: Determines whether the player can walk on his tile
        string textureKey;//MB: Used with a texture dictionary to get the texture of this tile
        // - - - - - - - - - - - - - - - - - - -
        public Tile(string textureKey, Hitbox hitbox)
        {
            this.textureKey = textureKey;
            this.hitbox = hitbox;
        }
        public void Draw(SpriteBatch spritebatch, Coord position, Dictionary<string, Texture2D> textureDict) => spritebatch.Draw(textureDict[textureKey], position.ToVector2());
    }
    [Serializable]//MB: This allows an instance of this class to be written to file
    public class Room
    {
        // - - - - Variables Global to this Room
        public Tile[,] tiles;//MB: The collection of tiles that make up the room
        public List<Container> containers;//MB: The collection of containers confined to this room
        public byte doorProfile;
        private bool isVisible;
        int width;//MB: The width of the room in tiles
        int height;//MB: The height of the room in tiles

        // - - - - - - - - - - - - - - - - - - -
        public Room(int width, int height, byte doorProfile,bool isVisible,Texture2D roomData=null)
        {
            this.width = width;
            this.height = height;
            this.doorProfile = doorProfile;
            this.isVisible = isVisible;
            containers = new List<Container>(); //{ new Container("Container","MenuTemplate",new Coord(90,352),new List<Item> { },48)};//MB: Test container
            tiles = new Tile[width, height];

            if (roomData == null)
            {
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++)
                    {
                        tiles[x, y] = new Tile("WallMiddle", null);
                    }
                }
            }
            else
            {
                Color[] data=new Color[324];
                roomData.Crop(new Rectangle(doorProfile*18,0,18,18)).GetData(data);
                Boolean[] parsedData = new Boolean[324];
                for (int i = 0; i < data.Length; i++)
                {
                    parsedData[i] = data[i].B<128;
                }
                Boolean[,] dD = DeserializeArray<Boolean>(parsedData, 18);
                for (int x = 1; x < 17; x++)
                {
                    for (int y = 1; y < 17; y++)
                    {
                        if (dD[x, y])
                        {
                            if (dD[x - 1, y] && dD[x, y - 1] && dD[x, y + 1] && !dD[x + 1, y])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallLeft", new Hitbox(new Rectangle(0, 0, 16, 32)));
                            }
                            if (!dD[x - 1, y] && dD[x, y - 1] && dD[x, y + 1] && dD[x + 1, y])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallRight", new Hitbox(new Rectangle(16, 0, 16, 32)));
                            }
                            if (dD[x - 1, y] && !dD[x, y - 1] && dD[x, y + 1] && dD[x + 1, y])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallBottom", new Hitbox(new Rectangle(0, 16, 32, 16)));
                            }
                            if (dD[x - 1, y] && dD[x, y - 1] && !dD[x, y + 1] && dD[x + 1, y])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallTop", new Hitbox(new Rectangle(0, 0, 32, 16)));
                            }
                            if (dD[x - 1, y] && dD[x, y - 1] && dD[x, y + 1] && dD[x + 1, y] && !dD[x + 1,y + 1])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallTopLeft", new Hitbox(new Rectangle(0, 0, 32, 16)));
                                tiles[x - 1, y - 1].hitbox.Add(new Rectangle(0, 0, 16, 32));
                            }
                            if (dD[x - 1, y] && dD[x, y - 1] && dD[x, y + 1] && dD[x + 1, y] && !dD[x - 1, y + 1])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallTopRight", new Hitbox(new Rectangle(0, 0, 32, 16)));
                                tiles[x - 1, y - 1].hitbox.Add(new Rectangle(16, 0, 16, 32));
                            }
                            if (dD[x - 1, y] && dD[x, y - 1] && dD[x, y + 1] && dD[x + 1, y] && !dD[x + 1, y - 1])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallBottomLeft", new Hitbox(new Rectangle(0, 16, 32, 16)));
                                tiles[x - 1, y - 1].hitbox.Add(new Rectangle(0, 0, 16, 32));
                            }
                            if (dD[x - 1, y] && dD[x, y - 1] && dD[x, y + 1] && dD[x + 1, y] && !dD[x - 1, y - 1])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallBottomRight", new Hitbox(new Rectangle(0, 16, 32, 16)));
                                tiles[x - 1, y - 1].hitbox.Add(new Rectangle(16, 0, 16, 32));
                            }
                            if (!dD[x - 1, y] && !dD[x, y - 1] && dD[x, y + 1] && dD[x + 1, y])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallTopLeftInv", new Hitbox(new Rectangle(16, 16, 16, 16)));
                            }
                            if (dD[x - 1, y] && !dD[x, y - 1] && dD[x, y + 1] && !dD[x + 1, y])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallTopRightInv", new Hitbox(new Rectangle(0, 16, 16, 16)));
                            }
                            if (!dD[x - 1, y] && dD[x, y - 1] && !dD[x, y + 1] && dD[x + 1, y])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallBottomLeftInv", new Hitbox(new Rectangle(16, 0, 16, 16)));
                            }
                            if (dD[x - 1, y] && dD[x, y - 1] && !dD[x, y + 1] && !dD[x + 1, y])
                            {
                                tiles[x - 1, y - 1] = new Tile("WallBottomRightInv", new Hitbox(new Rectangle(0, 0, 16, 16)));
                            }
                        }
                        else
                        {
                            tiles[x-1, y-1] = new Tile("WallMiddle", null);
                        }
                        
                    }
                }
            }




            //MB: The following is a painstaking method of assigning tiles based on location
            //tiles[0, 0] = new Tile("WallTopLeft", true);
            //for (int x = 1; x < width-1; x++)
            //{
            //    tiles[x, 0] = new Tile("WallTop", true);
            //}
            //tiles[width-1, 0] = new Tile("WallTopRight", true);
            //for (int y = 1; y < height - 1; y++)
            //{
            //    tiles[width-1, y] = new Tile("WallRight", true);
            //}
            //tiles[width-1, height-1] = new Tile("WallBottomRight", true);
            //for (int x = 1; x < width - 1; x++)
            //{
            //    tiles[x, height-1] = new Tile("WallBottom", true);
            //}
            //tiles[0, height-1] = new Tile("WallBottomLeft", true);
            //for (int y = 1; y < height - 1; y++)
            //{
            //    tiles[0, y] = new Tile("WallLeft", true);
            //}
            //for (int y = 1; y < height - 1; y++)
            //{
            //    for (int x = 1; x < width - 1; x++)
            //    {
            //        tiles[x, y] = new Tile("WallMiddle", false);
            //    }
            //}
            ////MB: ...Sorry.
            ////MB: Door profiles are a way of combining boolean values. Think of each bit in the byte as its own boolean variable
            ////MB: The LSB determines if there is a door at the top of a room
            ////MB: The 4 most significant bits are unused
            ////MB: Therefore the format of a door profile is the following:
            ////MB: 0:0:0:0:RightDoor:BottomDoor:LeftDoor:TopDoor
            ////MB: For example, 00001010 represents a room with doors on the left and right.
            //if ((doorProfile & 0b1) == 0b1)
            //{
            //    int midwidth = width / 2;
            //    tiles[midwidth + 1, 0] = new Tile("WallBottomLeftInv", true);
            //    tiles[midwidth, 0] = new Tile("WallMiddle", false);
            //    tiles[midwidth-1, 0] = new Tile("WallMiddle", false);
            //    tiles[midwidth-2, 0] = new Tile("WallBottomRightInv", true);
            //}
            //if ((doorProfile & 0b10) == 0b10)
            //{
            //    int midheight = height / 2;
            //    tiles[0, midheight+1] = new Tile("WallTopRightInv", true);
            //    tiles[0, midheight] = new Tile("WallMiddle", false);
            //    tiles[0, midheight-1] = new Tile("WallMiddle", false);
            //    tiles[0, midheight-2] = new Tile("WallBottomRightInv", true);
            //}
            //if ((doorProfile & 0b100) == 0b100)
            //{
            //    int midwidth = width / 2;
            //    int bottom = height - 1;
            //    tiles[midwidth + 1, bottom] = new Tile("WallTopLeftInv", true);
            //    tiles[midwidth, bottom] = new Tile("WallMiddle", false);
            //    tiles[midwidth - 1, bottom] = new Tile("WallMiddle", false);
            //    tiles[midwidth - 2, bottom] = new Tile("WallTopRightInv", true);
            //}
            //if ((doorProfile & 0b1000) == 0b1000)
            //{
            //    int midheight = height / 2;
            //    int right = width - 1;
            //    tiles[right, midheight + 1] = new Tile("WallTopLeftInv", true);
            //    tiles[right, midheight] = new Tile("WallMiddle", false);
            //    tiles[right, midheight - 1] = new Tile("WallMiddle", false);
            //    tiles[right, midheight - 2] = new Tile("WallBottomLeftInv", true);
            //}
        }
        public void Draw(SpriteBatch spriteBatch, Coord position, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict, MapDataPack dataPack)
        {
            if (isVisible)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        tiles[x, y].Draw(spriteBatch, position + new Coord(x * 32, y * 32), dataPack.textureDict);//MB: Draws each tile, one by one
                    }
                }
                foreach (Container container in containers)
                {
                    container.Draw(spriteBatch, position, graphicsDevice, dataPack.textureDict);//MB: Draws each container in this room
                }
            }
        }
        public void Update(MouseState mouseState, Coord position)
        {
            foreach (Container container in containers)
            {
                container.Update(mouseState, position);//MB: Runs the game logic for each container
            }
        }
        public void Discover() => isVisible = true;
        public static Room NONE = new Room(16,16,0,false);
    }
    [Serializable]
    public class Door//MB: This allows an instance of this class to be written to file
    {
        public string textureKey;
        private bool isVertical;
        public bool isVisible;
        public byte gap;
        public Coord[] connectingRooms;
        Coord position;
        public Hitbox hitbox;

        public Door(string textureKey, bool isVertical,Coord position,Coord[] linkedRooms)
        {
            this.textureKey = textureKey;
            this.isVertical = isVertical;
            gap = 0;
            this.position = position;
            this.connectingRooms = linkedRooms;
            if (isVertical)
            {
                this.hitbox = new Hitbox(new Rectangle((int)position.x-16, (int)position.y-64,32,128));
            }
            else
            {
                this.hitbox = new Hitbox(new Rectangle((int)position.x - 64, (int)position.y - 16, 128, 32));
            }
        }
        public void Update(Coord playerPos)
        {
            if (gap>0 && gap<48)
            {
                gap++;
                if (isVertical)
                {
                    this.hitbox = new Hitbox(new Rectangle((int)position.x - 16, (int)position.y - 64, 32, 64-gap));
                    hitbox.Add(new Rectangle((int)position.x - 16, (int)position.y+gap, 32, 64 - gap));
                }
                else
                {
                    this.hitbox = new Hitbox(new Rectangle((int)position.x - 64, (int)position.y - 16, 64-gap, 32));
                    hitbox.Add(new Rectangle((int)position.x + gap, (int)position.y-16,  64 - gap,32));
                }
            }
            if ((playerPos-position).ToVector2().Length()<64)
            {
                if (gap==0)
                {
                    gap = 1;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, Coord offset, Dictionary<string,Texture2D> textureDict, MapDataPack dataPack)
        {
            if (isVisible)
            {
                if (isVertical)
                {
                    Coord ScreenPos = offset + position;
                    Rectangle rectangleA = new Rectangle((ScreenPos - new Coord(8, gap-8)).ToPoint(), new Point(16, 64-gap));
                    Rectangle rectangle1 = new Rectangle(0,0, 32, 64-gap);
                    int doorText1 = SpruceContentManager.newTexture2D(textureDict[textureKey].Crop(rectangle1));
                    Rectangle rectangleB = new Rectangle((ScreenPos - new Coord(8, 8-gap)).ToPoint(), new Point(16, 64-gap));
                    Rectangle rectangle2 = new Rectangle(0, 0, 32, 64 - gap);
                    int doorText2 = SpruceContentManager.newTexture2D(textureDict[textureKey].Crop(rectangle2));
                    spriteBatch.Draw(SpruceContentManager.get(doorText1), rectangleA, null, Color.White,MathHelper.Pi, new Coord(32, 0).ToVector2(),SpriteEffects.None,0);
                    spriteBatch.Draw(SpruceContentManager.get(doorText2), rectangleB, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                else
                {
                    Coord ScreenPos = offset + position;
                    Rectangle rectangleA = new Rectangle((ScreenPos - new Coord(8 - gap,8)).ToPoint(), new Point(16, 64-gap));
                    Rectangle rectangle1 = new Rectangle(0, 0, 32, 64 - gap);
                    int doorText1 = SpruceContentManager.newTexture2D(textureDict[textureKey].Crop(rectangle1));
                    Rectangle rectangleB = new Rectangle((ScreenPos - new Coord(gap - 8, 8)).ToPoint(), new Point(16, 64-gap));
                    Rectangle rectangle2 = new Rectangle(0, 0, 32 ,64 - gap);
                    int doorText2 = SpruceContentManager.newTexture2D(textureDict[textureKey].Crop(rectangle2)); ;
                    spriteBatch.Draw(SpruceContentManager.get(doorText1), rectangleA, null, Color.White, MathHelper.Pi+MathHelper.PiOver2, new Coord(32, 0).ToVector2(), SpriteEffects.None, 0);
                    spriteBatch.Draw(SpruceContentManager.get(doorText2), rectangleB, null, Color.White, MathHelper.PiOver2, Vector2.Zero, SpriteEffects.None, 0);
                }
            }
        }
    }
}
#pragma warning restore CS0618