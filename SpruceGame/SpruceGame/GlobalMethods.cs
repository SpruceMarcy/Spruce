using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System;

namespace SpruceGame
{
    //MB: These functions can be called without a class instance as long as the file contains "using static SpruceGame.GlobalMethods;"
    static class GlobalMethods
    {
        public static Color[] GetRectangleDataFromTemplate(Texture2D template, Rectangle rectangle)
        {
            Color[] TextureData = new Color[rectangle.Width * rectangle.Height]; //MB: An array to hold the color values of the button texture.
            Color[] TemplateData1D = new Color[template.Width * template.Height];//MB: An array to hold the color values of the template texture.
            template.GetData(TemplateData1D);//MB: Puts the template data in an array
            Color[,] TemplateData2D = new Color[template.Width, template.Height];//MB: A 2D array to make accessing the color values easier.
            TemplateData2D = DeserializeArray<Color>(TemplateData1D,template.Width);
            int BorderWidth = (template.Width - 1) / 2;
            int BorderHeight = (template.Height - 1) / 2;
            int Bottom = rectangle.Height - BorderHeight;
            int Right = rectangle.Width - BorderWidth;
            int TemplateX = 0;
            int TemplateY = 0;
            //MB: The following code assigns each pixel of the button the data from the corresponding template pixel.
            //MB: This section is very susceptible to off-by-one errors. Please don't touch it unless necessary. 
            for (int y = 0; y < rectangle.Height; y++)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    TemplateX = x;
                    if (TemplateX > BorderWidth)
                    {
                        TemplateX = TemplateX > Right - 1 ? x - Right + BorderWidth + 1 : BorderWidth;
                    }
                    TemplateY = y;
                    if (TemplateY > BorderHeight)
                    {
                        TemplateY = TemplateY > Bottom - 1 ? y - Bottom + BorderHeight + 1 : BorderHeight;
                    }
                    TextureData[y * rectangle.Width + x] = TemplateData2D[TemplateX, TemplateY];
                }
            }
            return TextureData;
        }
        /// <summary>
        /// returns the corresponding value of a hexadecimal character
        /// </summary>
        public static byte HexCharToByte(char hexChar)
        {
            switch (hexChar)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'A':
                    return 10;
                case 'B':
                    return 11;
                case 'C':
                    return 12;
                case 'D':
                    return 13;
                case 'E':
                    return 14;
                case 'F':
                    return 15;
                default:
                    break;
            }
            return 0;
        }
        public static Texture2D Crop(this Texture2D image, Rectangle source)
        {
            var graphics = image.GraphicsDevice;
            var ret = new RenderTarget2D(graphics, source.Width, source.Height);
            var sb = new SpriteBatch(graphics);

            graphics.SetRenderTarget(ret); // draw to image
            graphics.Clear(new Color(0, 0, 0, 0));

            sb.Begin();
            sb.Draw(image, Vector2.Zero, source, Color.White);
            sb.End();
            sb.Dispose();
            graphics.SetRenderTarget(null); // set back to main window
            return (Texture2D)ret;
        }
        public static T[,] DeserializeArray<T>(T[] array,int width)
        {
            T[,] returnArray = new T[width,array.Length/width];
            int X = 0;
            int Y = 0;
            foreach (T item in array)
            {
                returnArray[X, Y] = item;
                X++;
                if (X >= width)
                {
                    X = 0;
                    Y++;
                }
            }
            return returnArray;
        }
    }
    [Serializable]
    public class Coord //MB: Custom XY grouping kinda like Vector2 or Point but can be written to file
    {
        public float x;
        public float y;
        public Coord(float x,float y)
        {
            this.x = x;
            this.y = y;
        }
        public static Coord operator +(Coord coord1, Coord coord2) => new Coord(coord1.x + coord2.x, coord1.y + coord2.y);
        public static Coord operator -(Coord coord1, Coord coord2) => new Coord(coord1.x - coord2.x, coord1.y - coord2.y);
        public static Coord operator /(Coord coord1, float denominator) => new Coord(coord1.x/denominator, coord1.y/denominator);
        public static Coord operator *(Coord coord1, float coefficient) => new Coord(coord1.x * coefficient, coord1.y *coefficient);
        public Vector2 ToVector2() => new Vector2((int)x, (int)y);
        public Point ToPoint() => new Point((int)x, (int)y);

        public override bool Equals(object obj)//MB: auto-generated
        {
            return obj is Coord coord &&
                   x == coord.x &&
                   y == coord.y;
        }
        public override int GetHashCode() => base.GetHashCode();//MB: auto-generated
    }
}
