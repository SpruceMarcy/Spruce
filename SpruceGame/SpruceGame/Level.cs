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
    public class Level
    {
        // - - - - Variables Global to this Level
        readonly string LevelName;//MB: Unused
        public Room[,] rooms;//MB: The collection of rooms in this level
        public Door[] doors;
        int width;//MB: The width of the level in rooms
        int height;//MB: The height of the level in rooms

        // - - - - - - - - - - - - - - - - - - -
        public Level(int width, int height, Dictionary<string, Texture2D> TextureDict,byte[] seed,int RoomCount)//MB: On instanciation
        {
            this.width = width;
            this.height = height;
            rooms = GenerateLabyrinth(RoomCount,new Vector2(5,5),new Vector2(0,0),seed);//MB: Make a level with placeholder values
            doors = new Door[] { };
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (rooms[x,y] != null)
                    {
                        if ((rooms[x, y].DoorProfile & 0b1) == 0b1)
                        {
                            Array.Resize(ref doors, doors.Length + 1);
                            doors[doors.Length - 1] = new Door("Door", false, new Coord((float)(x + 0.5) * 512, y * 512), new Coord[] { new Coord(x, y), new Coord(x, y - 1) });
                            //doors[doors.Length - 1].IsVisible = true;
                        }
                        if ((rooms[x, y].DoorProfile & 0b1000) == 0b1000)
                        {
                            Array.Resize(ref doors, doors.Length + 1);
                            doors[doors.Length - 1] = new Door("Door", true, new Coord((x+1) * 512, (float)(y + 0.5) * 512), new Coord[] { new Coord(x, y), new Coord(x+1,y) });
                            //doors[doors.Length - 1].IsVisible = true;
                        }
                    }
                }
            }
        }
        public void Update(MouseState mouseState, Coord Position)//MB: Runs through each room and updates them
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (rooms[x, y] != null)//MB: Prevents null reference exceptions
                    {
                        rooms[x, y].Update(mouseState,Position + new Coord(x * 512, y * 512));
                    }
                }
            }
            foreach (Door door in doors)
            {
                door.Update(new Coord(960,540) - Position);
                if (door.Gap==1)
                {
                    foreach (Coord conRoom in door.ConnectingRooms)
                    {
                        rooms[(int)conRoom.X, (int)conRoom.Y].Discover();
                        foreach (Door tempdoor in doors)
                        {
                            if (Array.IndexOf<Coord>(tempdoor.ConnectingRooms,conRoom)>-1)
                            {
                                tempdoor.IsVisible = true;
                            }
                        }
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, Coord Position, GraphicsDevice graphicsDevice, Dictionary<string,Texture2D> TextureDict)//MB: Draws the level to the screen
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0;y < height; y++)
                {
                    if (rooms[x,y] != null)//MB: Prevents null reference exceptions
                    {
                        rooms[x,y].Draw(spriteBatch, Position+new Coord(x*32*16,y * 32 * 16),graphicsDevice,TextureDict);
                    }
                }
            }
            foreach (Door door in doors)
            {
                door.Draw(spriteBatch,Position,TextureDict);
            }
        }
        /// <summary>
        /// Generates a labyrinth of rooms using a partial Prim's algorithm on a randomised graph.
        /// While a path is guaranteed to  each room, loops are random and more likely at higher room counts.
        /// Entering a MaxRoomCount higher than the total amount of rooms possible will result in a completely open labyrinth (no solid walls)
        /// </summary>
        /// <param name="MaxRoomCount">The total amount of generated rooms in the labyrinth</param>
        /// <param name="Dimensions">The size of the labyrinth in rooms</param>
        /// <param name="StartRoom">Where the labyrinth will be generated from. Ideal starting room due to guarantee of existance</param>
        /// <param name="seed">Seed for randomness. Will determine layout and contents of rooms</param>
        /// <returns>2D array of rooms</returns>
        private Room[,] GenerateLabyrinth(int MaxRoomCount, Vector2 Dimensions, Vector2 StartRoom,byte[] seed)
        {
            // HERE BE DRAGONS //
            int RoomCount=1;
            Node[,] WeightMatrix = new Node[(int)Dimensions.X, (int)Dimensions.Y];
            for (int x = 0; x < Dimensions.X; x++)
            {
                for (int y = 0; y < Dimensions.Y; y++)
                {
                    WeightMatrix[x, y] = new Node {Visited = false};
                    WeightMatrix[x, y].North = y < Dimensions.Y - 1 ? (uint)new Random(x + y).Next(1, 101) : uint.MaxValue;
                    WeightMatrix[x, y].East = x < Dimensions.X - 1 ? (uint)new Random(x + y+ seed[0]<<16+seed[1]<<8+seed[2]).Next(1, 101) : uint.MaxValue;
                }
            }
            List<Edge> Edges = new List<Edge>();
            if (StartRoom.X > 0)
            {
                Edges.Add(new Edge(StartRoom, StartRoom + new Vector2(-1, 0)));
            }
            if (StartRoom.Y > 0)
            {
                Edges.Add(new Edge(StartRoom, StartRoom + new Vector2(0, -1)));
            }
            if (StartRoom.X < Dimensions.X)
            {
                Edges.Add(new Edge(StartRoom, StartRoom + new Vector2(1, 0)));
            }
            if (StartRoom.Y< Dimensions.Y)
            {
                Edges.Add(new Edge(StartRoom, StartRoom + new Vector2(0,1)));
            }
            WeightMatrix[(int)StartRoom.X, (int)StartRoom.Y].Visited = true;
            while (RoomCount<MaxRoomCount && Edges.Count>0)
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
                foreach (Vector2 node in new Vector2[] {ShortestEdge.One,ShortestEdge.Two})
                {
                    if (!WeightMatrix[(int)node.X, (int)node.Y].Visited)
                    {
                        if (node.X > 0 && !WeightMatrix[(int)node.X-1, (int)node.Y].Visited)
                        {
                            Edges.Add(new Edge(node, node + new Vector2(-1, 0)));
                        }
                        if (node.Y > 0 && !WeightMatrix[(int)node.X, (int)node.Y-1].Visited)
                        {
                            Edges.Add(new Edge(node, node + new Vector2(0, -1)));
                        }
                        if (node.X < Dimensions.X-1 && !WeightMatrix[(int)node.X+1, (int)node.Y].Visited)
                        {
                            Edges.Add(new Edge(node, node + new Vector2(1, 0)));
                        }
                        if (node.Y < Dimensions.Y-1 && !WeightMatrix[(int)node.X, (int)node.Y+1].Visited)
                        {
                            Edges.Add(new Edge(node, node + new Vector2(0, 1)));
                        }
                        WeightMatrix[(int)node.X, (int)node.Y].Visited = true;
                        RoomCount++;
                    }
                }
            }
            Room[,] rooms = new Room[(int)Dimensions.X, (int)Dimensions.Y];
            for (int x = 0; x < Dimensions.X; x++)
            {
                for (int y = 0; y < Dimensions.Y; y++)
                {
                    if (WeightMatrix[x,y].Visited)
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
                        if (x < Dimensions.X - 1 && WeightMatrix.GetEdgeValue(new Edge(new Vector2(x, y), new Vector2(x + 1, y))) == 0)
                        {
                            DoorProfile |= 0b1000;
                        }
                        if (y < Dimensions.Y - 1 && WeightMatrix.GetEdgeValue(new Edge(new Vector2(x, y), new Vector2(x , y+ 1))) == 0)
                        {
                            DoorProfile |= 0b100;
                        }
                        rooms[x, y] = new Room(16, 16,DoorProfile,new Vector2(x,y)==StartRoom);
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
    public Vector2 One;
    public Vector2 Two;
    public bool isVertical;
    public Edge(Vector2 NodeOne,Vector2 NodeTwo)
    {
        One = NodeOne;
        Two = NodeTwo;
        isVertical = One.X == Two.X;
        if ((NodeOne-NodeTwo).Length()!=1)
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
    public uint North;
    public uint East;
    public bool Visited;
}
/// <summary>
/// For use in Prim's algorithm
/// </summary>
public static class Extensions
{
    public static uint GetEdgeValue(this Node[,] nodes, Edge edge)//MB: the "this" means that this function is called on a 2d array of nodes
    {
        Node NodeOne = nodes[(int)edge.One.X, (int)edge.One.Y];
        Node NodeTwo = nodes[(int)edge.Two.X, (int)edge.Two.Y];
        return edge.isVertical
            ? edge.One.Y>edge.Two.Y ? NodeTwo.North : NodeOne.North
            : edge.One.X > edge.Two.X ? NodeTwo.East : NodeOne.East;
    }
    public static void SetEdgeValue(this Node[,] nodes, Edge edge, uint Value)
    {
        if (edge.isVertical)
        {
            if (edge.One.Y > edge.Two.Y)
            {
                nodes[(int)edge.Two.X, (int)edge.Two.Y].North=Value;
            }
            else
            {
                nodes[(int)edge.One.X, (int)edge.One.Y].North = Value;
            }
        }
        else
        {
            if (edge.One.X > edge.Two.X)
            {
                nodes[(int)edge.Two.X, (int)edge.Two.Y].East = Value;
            }
            else
            {
                nodes[(int)edge.One.X, (int)edge.One.Y].East = Value;
            }
        }
    }
}


#pragma warning restore CS0618