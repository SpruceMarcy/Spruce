using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
#pragma warning disable CS0618//MB: This disables the depreciated method warning

/// <summary>
/// MB: This class is to encapsulate all features of a clickable button
/// </summary>
public class Textbox
{
    private Texture2D Texture;//MB: Holds the texture for the sprite without text
    readonly Rectangle rectangle;
    public bool Enabled = true;
    public string Text;
    SpriteFont TextFont;

    public Textbox(string Text, Rectangle rectangle, GraphicsDevice graphicsDevice, Color BorderColour, SpriteFont TextFont)
    {
        this.rectangle = rectangle;
        this.Text = Text;
        this.TextFont = TextFont;
        Texture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
        {
            Color[] TempData = new Color[rectangle.Width * rectangle.Height];
            for (int y = 0; y < rectangle.Height; y++)
            {
                TempData[y * rectangle.Width] = BorderColour;
                TempData[(y+1) * rectangle.Width-1] = BorderColour;
                for (int x = 1; x < rectangle.Width - 1; x++)
                {
                    if(y==0 || y == rectangle.Height - 1)
                    {
                        TempData[y * rectangle.Width + x] = BorderColour;
                    }
                    TempData[y * rectangle.Width + x] = Color.White;
                }
            }
            Texture.SetData(TempData);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch to draw the button to.</param>
    public void Draw(SpriteBatch spriteBatch, MouseState mouseState)
    {
        spriteBatch.Draw(Texture, rectangle.Location.ToVector2());
    }
}
#pragma warning restore CS0618