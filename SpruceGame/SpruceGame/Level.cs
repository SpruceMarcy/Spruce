﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System;//MB: Allows use of [Serializable]
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    [Serializable]//MB: This allows an instance of this class to be written to file
    public class Level
    {
        // - - - - Variables Global to this Level
        readonly string levelName;//MB: Unused
        private Room[,] rooms;//MB: The collection of rooms in this level
        private string dataPackKey;
        public Room getRoom(int x, int y)
        {
            if (0 <= x && x < rooms.GetLength(0) && 0 <= y && y < rooms.GetLength(1))
            {
                return rooms[x, y];
            }
            return Room.NONE;
        }
        public Door[] doors;
        public List<Projectile> projectiles;
        public int width;//MB: The width of the level in rooms
        public int height;//MB: The height of the level in rooms

        // - - - - - - - - - - - - - - - - - - -
        public Level(int width, int height, string mapDataPackKey, byte[] seed, int roomCount, Texture2D roomData)//MB: On instanciation
        {
            this.width = width;
            this.height = height;
            this.dataPackKey = mapDataPackKey;
            rooms = GenerateLabyrinth(roomCount, new Vector2(5, 5), new Vector2(0, 0), seed, roomData);//MB: Make a level with placeholder values
            doors = new Door[] { };
            projectiles = new List<Projectile>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (rooms[x, y] != null)
                    {
                        if ((rooms[x, y].doorProfile & 0b1) == 0b1)
                        {
                            Array.Resize(ref doors, doors.Length + 1);
                            doors[doors.Length - 1] = new Door("Door", false, new Coord((float)(x + 0.5) * 512, y * 512), new Coord[] { new Coord(x, y), new Coord(x, y - 1) });
                            doors[doors.Length - 1].isVisible = Array.IndexOf(doors[doors.Length - 1].connectingRooms, new Coord(0, 0)) >= 0;
                        }
                        if ((rooms[x, y].doorProfile & 0b1000) == 0b1000)
                        {
                            Array.Resize(ref doors, doors.Length + 1);
                            doors[doors.Length - 1] = new Door("Door", true, new Coord((x + 1) * 512, (float)(y + 0.5) * 512), new Coord[] { new Coord(x, y), new Coord(x + 1, y) });
                            doors[doors.Length - 1].isVisible = Array.IndexOf(doors[doors.Length - 1].connectingRooms, new Coord(0, 0)) >= 0;
                        }
                    }
                }
            }
        }
        public void Update(MouseState mouseState, Coord position)//MB: Runs through each room and updates them
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (rooms[x, y] != null)//MB: Prevents null reference exceptions
                    {
                        rooms[x, y].Update(mouseState, position + new Coord(x * 512, y * 512));
                    }
                }
            }
            foreach (Door door in doors)
            {
                door.Update(new Coord(960, 540) - position);
                if (door.gap == 1)
                {
                    foreach (Coord conRoom in door.connectingRooms)
                    {
                        rooms[(int)conRoom.x, (int)conRoom.y].Discover();
                        foreach (Door tempdoor in doors)
                        {
                            if (Array.IndexOf<Coord>(tempdoor.connectingRooms, conRoom) > -1)
                            {
                                tempdoor.isVisible = true;
                            }
                        }
                    }
                }
            }
            List<Projectile> projectileDisposeList = new List<Projectile>();
            foreach (Projectile projectile in projectiles)
            {
                for (int i = 1; i <= projectile.speed; i++)
                {
                    if (IsSolid(projectile.position + (projectile.movement * i / projectile.speed)))
                    {
                        projectileDisposeList.Add(projectile);
                        break;
                    }
                }
                projectile.Update(mouseState);
            }
            foreach (Projectile projectile in projectileDisposeList)
            {
                projectiles.Remove(projectile);
            }
            projectileDisposeList.Clear();
        }
        public void Draw(SpriteBatch spriteBatch, Coord position, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict, Dictionary<string, MapDataPack> dataPacks)//MB: Draws the level to the screen
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (rooms[x, y] != null)//MB: Prevents null reference exceptions
                    {
                        rooms[x, y].Draw(spriteBatch, position + new Coord(x * 32 * 16, y * 32 * 16), graphicsDevice, textureDict, dataPacks[dataPackKey]);
                    }
                }
            }
            foreach (Door door in doors)
            {
                door.Draw(spriteBatch, position, textureDict, dataPacks[dataPackKey]);
            }
            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch, position, graphicsDevice, textureDict);
            }
            
        }
        /// <summary>
        /// Returns whether or not the specified position is occupied, preventing movement.
        /// This could also potentially be moved to Level
        /// </summary>
        /// <param name="position">Coordinates in the level to test</param>
        /// <returns></returns>
        public bool IsSolid(Coord position)
        {
            Room room;
            room = getRoom((int)Math.Floor(position.x / (16 * 32)), (int)Math.Floor(position.y / (16 * 32)));//MB: get the room at those coordinates
            Tile tile;
            tile = room.tiles[(int)Math.Floor((position.x % (16 * 32)) / 32), (int)Math.Floor((position.y % (16 * 32)) / 32)];//MB: get the tile at those coordinates
            if( Hitbox.IsIn(tile.hitbox,new Coord(position.x % 32, position.y % 32)))
            {
                return true;
            }
            foreach(Door door in doors)
            {
                if (Hitbox.IsIn(door.hitbox, position))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsSolid(Hitbox hitbox,Coord Origin)
        {
            Hitbox offsetHitbox = hitbox.Adjust(Origin);
            for(int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (Hitbox.Collides(offsetHitbox,new Hitbox(new Rectangle(x*512,y*512,512,512))))
                    {
                        Room thisRoom = getRoom(x, y);
                        for (int tiley = 0; tiley < thisRoom.tiles.GetLength(1); tiley++)
                        {
                            for (int tilex = 0; tilex < thisRoom.tiles.GetLength(0); tilex++)
                            {
                                Hitbox relativeHitbox = thisRoom.tiles[tilex,tiley].hitbox==null?null: thisRoom.tiles[tilex, tiley].hitbox.Adjust(new Coord(x*512 + (tilex * 32), y*512 + (tiley * 32)));
                                if (Hitbox.Collides(relativeHitbox, offsetHitbox))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            foreach (Door door in doors)
            {
                if (Hitbox.Collides(door.hitbox, offsetHitbox))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Generates a labyrinth of rooms using a partial Prim's algorithm on a randomised graph.
        /// While a path is guaranteed to  each room, loops are random and more likely at higher room counts.
        /// Entering a MaxRoomCount higher than the total amount of rooms possible will result in a completely open labyrinth (no solid walls)
        /// </summary>
        /// <param name="maxRoomCount">The total amount of generated rooms in the labyrinth</param>
        /// <param name="dimensions">The size of the labyrinth in rooms</param>
        /// <param name="startRoom">Where the labyrinth will be generated from. Ideal starting room due to guarantee of existance</param>
        /// <param name="seed">Seed for randomness. Will determine layout and contents of rooms</param>
        /// <returns>2D array of rooms</returns>
        private Room[,] GenerateLabyrinth(int maxRoomCount, Vector2 dimensions, Vector2 startRoom,byte[] seed,Texture2D roomData)
        {
            // HERE BE DRAGONS //
            int RoomCount=1;
            Node[,] WeightMatrix = new Node[(int)dimensions.X, (int)dimensions.Y];
            for (int x = 0; x < dimensions.X; x++)
            {
                for (int y = 0; y < dimensions.Y; y++)
                {
                    WeightMatrix[x, y] = new Node {visited = false};
                    WeightMatrix[x, y].north = y < dimensions.Y - 1 ? (uint)new Random(x + y).Next(1, 101) : uint.MaxValue;
                    WeightMatrix[x, y].east = x < dimensions.X - 1 ? (uint)new Random(x + y+ seed[0]<<16+seed[1]<<8+seed[2]).Next(1, 101) : uint.MaxValue;
                }
            }
            List<Edge> Edges = new List<Edge>();
            if (startRoom.X > 0)
            {
                Edges.Add(new Edge(startRoom, startRoom + new Vector2(-1, 0)));
            }
            if (startRoom.Y > 0)
            {
                Edges.Add(new Edge(startRoom, startRoom + new Vector2(0, -1)));
            }
            if (startRoom.X < dimensions.X)
            {
                Edges.Add(new Edge(startRoom, startRoom + new Vector2(1, 0)));
            }
            if (startRoom.Y< dimensions.Y)
            {
                Edges.Add(new Edge(startRoom, startRoom + new Vector2(0,1)));
            }
            WeightMatrix[(int)startRoom.X, (int)startRoom.Y].visited = true;
            while (RoomCount<maxRoomCount && Edges.Count>0)
            {
                Edge ShortestEdge = null;
                foreach (Edge edge in Edges)
                {
                    if (ShortestEdge==null || WeightMatrix.GetEdgeValue(ShortestEdge)> WeightMatrix.GetEdgeValue(edge))
                    {
                        ShortestEdge=edge;
                    }
                }
                Edges.Remove(ShortestEdge);
                WeightMatrix.SetEdgeValue(ShortestEdge, 0);
                foreach (Vector2 node in new Vector2[] {ShortestEdge.one,ShortestEdge.two})
                {
                    if (!WeightMatrix[(int)node.X, (int)node.Y].visited)
                    {
                        if (node.X > 0 && !WeightMatrix[(int)node.X-1, (int)node.Y].visited)
                        {
                            Edges.Add(new Edge(node, node + new Vector2(-1, 0)));
                        }
                        if (node.Y > 0 && !WeightMatrix[(int)node.X, (int)node.Y-1].visited)
                        {
                            Edges.Add(new Edge(node, node + new Vector2(0, -1)));
                        }
                        if (node.X < dimensions.X-1 && !WeightMatrix[(int)node.X+1, (int)node.Y].visited)
                        {
                            Edges.Add(new Edge(node, node + new Vector2(1, 0)));
                        }
                        if (node.Y < dimensions.Y-1 && !WeightMatrix[(int)node.X, (int)node.Y+1].visited)
                        {
                            Edges.Add(new Edge(node, node + new Vector2(0, 1)));
                        }
                        WeightMatrix[(int)node.X, (int)node.Y].visited = true;
                        RoomCount++;
                    }
                }
            }
            Room[,] rooms = new Room[(int)dimensions.X, (int)dimensions.Y];
            for (int x = 0; x < dimensions.X; x++)
            {
                for (int y = 0; y < dimensions.Y; y++)
                {
                    if (WeightMatrix[x,y].visited)
                    {
                        //MB: ...Sorry.
                        //MB: Door profiles are a way of combining boolean values. Think of each bit in the byte as its own boolean variable
                        //MB: The LSB determines if there is a door at the top of a room
                        //MB: The 4 most significant bits are unused
                        //MB: Therefore the format of a door profile is the following:
                        //MB: 0:0:0:0:RightDoor:BottomDoor:LeftDoor:TopDoor
                        //MB: For example, 00001010 represents a room with doors on the left and right.
                        byte DoorProfile = 0;
                        if (x > 0 && WeightMatrix.GetEdgeValue(new Edge(new Vector2(x, y), new Vector2(x-1, y))) == 0)
                        {
                            DoorProfile |= 0b10;
                        }
                        if (y > 0 && WeightMatrix.GetEdgeValue(new Edge(new Vector2(x, y), new Vector2(x , y- 1))) == 0)
                        {
                            DoorProfile |= 0b1;
                        }
                        if (x < dimensions.X - 1 && WeightMatrix.GetEdgeValue(new Edge(new Vector2(x, y), new Vector2(x + 1, y))) == 0)
                        {
                            DoorProfile |= 0b1000;
                        }
                        if (y < dimensions.Y - 1 && WeightMatrix.GetEdgeValue(new Edge(new Vector2(x, y), new Vector2(x , y+ 1))) == 0)
                        {
                            DoorProfile |= 0b100;
                        }
                        rooms[x, y] = new Room(16, 16,DoorProfile,new Vector2(x,y)==startRoom,roomData);
                    }
                }
            }
            return rooms;
        }
    }
}
/// <summary>
/// For use in Prim's algorithm
/// </summary>
public class Edge
{
    public Vector2 one;
    public Vector2 two;
    public bool isVertical;
    public Edge(Vector2 nodeOne,Vector2 nodeTwo)
    {
        one = nodeOne;
        two = nodeTwo;
        isVertical = one.X == two.X;
        if ((nodeOne-nodeTwo).Length()!=1)
        {
            throw new System.Exception("Nodes Aren't Adjacent");
        }
    }
}
/// <summary>
/// For use in Prim's algorithm
/// </summary>
public class Node
{
    public uint north;
    public uint east;
    public bool visited;
}
/// <summary>
/// For use in Prim's algorithm
/// </summary>
public static class Extensions
{
    public static uint GetEdgeValue(this Node[,] nodes, Edge edge)//MB: the "this" means that this function is called on a 2d array of nodes
    {
        Node NodeOne = nodes[(int)edge.one.X, (int)edge.one.Y];
        Node NodeTwo = nodes[(int)edge.two.X, (int)edge.two.Y];
        return edge.isVertical
            ? edge.one.Y>edge.two.Y ? NodeTwo.north : NodeOne.north
            : edge.one.X > edge.two.X ? NodeTwo.east : NodeOne.east;
    }
    public static void SetEdgeValue(this Node[,] nodes, Edge edge, uint value)
    {
        if (edge.isVertical)
        {
            if (edge.one.Y > edge.two.Y)
            {
                nodes[(int)edge.two.X, (int)edge.two.Y].north = value;
            }
            else
            {
                nodes[(int)edge.one.X, (int)edge.one.Y].north = value;
            }
        }
        else
        {
            if (edge.one.X > edge.two.X)
            {
                nodes[(int)edge.two.X, (int)edge.two.Y].east = value;
            }
            else
            {
                nodes[(int)edge.one.X, (int)edge.one.Y].east = value;
            }
        }
    }
}


#pragma warning restore CS0618