using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System;
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
        public Level(int width, int height, Dictionary<string, Texture2D> TextureDict,byte[] seed,int RoomCount)
        {
            this.width = width;
            this.height = height;
            rooms = GenerateLabyrinth(RoomCount,new Vector2(5,5),new Vector2(0,0),TextureDict,seed);
        }
        public void Update(MouseState mouseState, Vector2 Position,GraphicsDevice graphicsDevice)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (rooms[x, y] != null)
                    {
                        rooms[x, y].Update(mouseState,Position + new Vector2(x * 32 * 16, y * 32 * 16),graphicsDevice);
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 Position)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0;y < height; y++)
                {
                    if (rooms[x,y] != null)
                    {
                        rooms[x,y].Draw(spriteBatch, Position+new Vector2(x*32*16,y * 32 * 16));
                    }
                }
            }
        }
        private Room[,] GenerateLabyrinth(int MaxRoomCount, Vector2 Dimensions, Vector2 StartRoom,Dictionary<string,Texture2D> TextureDict,byte[] seed)
        {
            int RoomCount=1;
            Node[,] WeightMatrix = new Node[(int)Dimensions.X, (int)Dimensions.Y];
            for (int x = 0; x < Dimensions.X; x++)
            {
                for (int y = 0; y < Dimensions.Y; y++)
                {
                    WeightMatrix[x, y] = new Node();
                    WeightMatrix[x, y].Visited = false;
                    if (y < Dimensions.Y - 1)
                    {
                        WeightMatrix[x, y].North = (uint)new Random(x + y).Next(1, 101);
                    }
                    else
                    {
                        WeightMatrix[x, y].North = uint.MaxValue;
                    }
                    if (x < Dimensions.X - 1)
                    {
                        WeightMatrix[x, y].East = (uint)new Random(x + y+ seed[0]<<16+seed[1]<<8+seed[2]).Next(1, 101);
                    }
                    else
                    {
                        WeightMatrix[x, y].East = uint.MaxValue;
                    }
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
                        rooms[x, y] = new Room(16, 16, TextureDict,DoorProfile);
                    }
                }
            }
            return rooms;
        }
    }
}
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
public class Node
{
    public uint North;
    public uint East;
    public bool Visited;
}
public static class Extensions
{
    public static uint GetEdgeValue(this Node[,] nodes, Edge edge)
    {
        Node NodeOne = nodes[(int)edge.One.X, (int)edge.One.Y];
        Node NodeTwo = nodes[(int)edge.Two.X, (int)edge.Two.Y];
        if (edge.isVertical)
        {
            if (edge.One.Y>edge.Two.Y)
            {
                return NodeTwo.North;
            }
            else
            {
                return NodeOne.North;
            }
        }
        else
        {
            if (edge.One.X > edge.Two.X)
            {
                return NodeTwo.East;
            }
            else
            {
                return NodeOne.East;
            }
        }
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