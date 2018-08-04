using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
#pragma warning disable CS0618//MB: This disables the depreciated method warning

/// <summary>
/// MB: This class is to encapsulate all features of a clickable button
/// </summary>
public class Button
{
    private Texture2D ButtonTexture;//MB: Holds the texture for the sprite without text
    private Texture2D HoverTexture;
    private Texture2D PressedTexture;
    private Texture2D DisabledTexture;
    private Texture2D SelectedTexture;
    readonly Rectangle rectangle;//MB: This is the size and position of the button.
                                 //MB: If you need to change it; just declare a new button.
    public bool Enabled = true;//MB: If false, the button will be greyed out and not function.
    public bool Selected = false;//MB: For use externally as radio buttons and checkboxes
    public string Text;//MB: The message on the button. Can be changed.
    SpriteFont ButtonFont;//MB: The font used to display the text on the button.
                          /// <summary>
                          /// Instanciates a button, creating the appropriate textures for it.
                          /// </summary>
                          /// <param name="rectangle">The size and shape of the button.</param>
                          /// <param name="Text">The message on the button.</param>
                          /// <param name="graphicsDevice"></param>
                          /// <param name="TextureDict"></param>
                          /// <param name="ButtonFont">The font of the message on the button.</param>
    public Button(Rectangle rectangle, string Text, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> TextureDict, SpriteFont ButtonFont)
    {
        this.rectangle = rectangle;
        this.Text = Text;
        this.ButtonFont = ButtonFont;
        ButtonTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);//MB: Creates a blank texture for the button
        HoverTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
        PressedTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
        DisabledTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
        SelectedTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
        ButtonTexture.SetData(GetTextureData(TextureDict["ButtonUnpressed"]));//MB: Sets the actual texture to the locally generated texture
        HoverTexture.SetData(GetTextureData(TextureDict["ButtonHover"]));
        PressedTexture.SetData(GetTextureData(TextureDict["ButtonPressed"]));
        DisabledTexture.SetData(GetTextureData(TextureDict["ButtonDisabled"]));
        SelectedTexture.SetData(GetTextureData(TextureDict["ButtonSelected"]));
    }
    /// <summary>
    /// Returns the colour array of the full button texture when provided a template
    /// </summary>
    /// <returns>Array of Color</returns>
    private Color[] GetTextureData(Texture2D Template)
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
    /// <summary>
    /// Draws the button texture and text to a given SpriteBatch
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch to draw the button to.</param>
    public void Draw(SpriteBatch spriteBatch, MouseState mouseState)
    {
        if (Enabled)
        { 
            if (rectangle.Contains(mouseState.Position))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    spriteBatch.Draw(PressedTexture, rectangle.Location.ToVector2());
                }
                else
                {
                    spriteBatch.Draw(HoverTexture, rectangle.Location.ToVector2());
                }

            }
            else
            {
                if (Selected)
                {
                    spriteBatch.Draw(SelectedTexture, rectangle.Location.ToVector2());//MB: Draws the background
                }
                else
                {
                    spriteBatch.Draw(ButtonTexture, rectangle.Location.ToVector2());//MB: Draws the background
                }
            }
        }
        else
        {
            spriteBatch.Draw(DisabledTexture, rectangle.Location.ToVector2());
        }
        float width = ButtonFont.MeasureString(Text).X;//MB: Gets the width of the text
        float height = ButtonFont.MeasureString(Text).Y;//MB: Gets the height of the text
        spriteBatch.DrawString(ButtonFont, Text, new Vector2(rectangle.X + (rectangle.Width - width) / 2, rectangle.Y + (rectangle.Height - height) / 2), Color.Black);//MB: Draws the text in the middle of the button
    }
    /// <summary>
    /// Returns whether or not the button has been activated when given the location of a click.
    /// </summary>
    /// <param name="MousePos">The position of a mouse click.</param>
    /// <returns>Boolean</returns>
    public bool ClickCheck(Point MousePos)
    {
        return Enabled && rectangle.Contains(MousePos);
    }
}
#pragma warning restore CS0618