using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System.Text.RegularExpressions;

namespace SpruceGame
{
    static class GlobalMethods
    {
        public static Color[] GetRectangleDataFromTemplate(Texture2D Template, Rectangle rectangle)
        {
            Color[] TextureData = new Color[rectangle.Width * rectangle.Height]; //MB: An array to hold the color values of the button texture.
            Color[] TemplateData1D = new Color[Template.Width * Template.Height];//MB: An array to hold the color values of the template texture.
            Template.GetData(TemplateData1D);//MB: Puts the template data in an array
            Color[,] TemplateData2D = new Color[Template.Width, Template.Height];//MB: A 2D array to make accessing the color values easier.
            {//MB: This indented section just formats TemplateData1D into TemplateData2D
                int X = 0;
                int Y = 0;
                int TemplateWidth = Template.Width;
                foreach (Color color in TemplateData1D)
                {
                    TemplateData2D[X, Y] = color;
                    X++;
                    if (X >= TemplateWidth)
                    {
                        X = 0;
                        Y++;
                    }
                }
            }
            int BorderWidth = (Template.Width - 1) / 2;
            int BorderHeight = (Template.Height - 1) / 2;
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
                        if (TemplateX > Right - 1)
                        {
                            TemplateX = x - Right + BorderWidth + 1;
                        }
                        else
                        {
                            TemplateX = BorderWidth;
                        }
                    }
                    TemplateY = y;
                    if (TemplateY > BorderHeight)
                    {
                        if (TemplateY > Bottom - 1)
                        {
                            TemplateY = y - Bottom + BorderHeight + 1;
                        }
                        else
                        {
                            TemplateY = BorderHeight;
                        }
                    }
                    TextureData[y * rectangle.Width + x] = TemplateData2D[TemplateX, TemplateY];
                }
            }
            return TextureData;
        }
        public static byte HexCharToByte(char HexChar)
        {
            switch (HexChar)
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
    }
}
