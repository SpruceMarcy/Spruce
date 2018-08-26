using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using static SpruceGame.GlobalMethods;
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    /// <summary>
    /// MB: This class is to encapsulate all features of a clickable button
    /// </summary>
    public class Button
    {
        private Texture2D ButtonTexture;//MB: Holds the texture for the sprite without text
        private Texture2D HoverTexture;//MB: Texture for when the mouse is over the button
        private Texture2D PressedTexture;//MB: Texture for when the button is being clicked
        private Texture2D DisabledTexture;//MB: Texture for when the button is unclickable
        private Texture2D SelectedTexture;//MB: Texture for when the button is activated
        readonly Rectangle rectangle;//MB: This is the size and position of the button.
                                     //MB: If you need to change it; just declare a new button.
        public bool Enabled = true;//MB: If false, the button will be greyed out and not function.
        public bool Selected = false;//MB: For use with external logic to make radio buttons and checkboxes
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
            ButtonTexture.SetData(GetRectangleDataFromTemplate(TextureDict["ButtonUnpressed"],rectangle));//MB: Sets the actual texture to the locally generated texture
            HoverTexture.SetData(GetRectangleDataFromTemplate(TextureDict["ButtonHover"], rectangle));
            PressedTexture.SetData(GetRectangleDataFromTemplate(TextureDict["ButtonPressed"], rectangle));
            DisabledTexture.SetData(GetRectangleDataFromTemplate(TextureDict["ButtonDisabled"], rectangle));
            SelectedTexture.SetData(GetRectangleDataFromTemplate(TextureDict["ButtonSelected"], rectangle));
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
}
#pragma warning restore CS0618