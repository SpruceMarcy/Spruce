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
        private Texture2D buttonTexture;//MB: Holds the texture for the sprite without text
        private Texture2D hoverTexture;//MB: Texture for when the mouse is over the button
        private Texture2D pressedTexture;//MB: Texture for when the button is being clicked
        private Texture2D disabledTexture;//MB: Texture for when the button is unclickable
        private Texture2D selectedTexture;//MB: Texture for when the button is activated
        readonly Rectangle rectangle;//MB: This is the size and position of the button.
                                     //MB: If you need to change it; just declare a new button.
        public bool enabled = true;//MB: If false, the button will be greyed out and not function.
        public bool selected = false;//MB: For use with external logic to make radio buttons and checkboxes
        public string text;//MB: The message on the button. Can be changed.
        SpriteFont buttonFont;//MB: The font used to display the text on the button.
                              /// <summary>
                              /// Instanciates a button, creating the appropriate textures for it.
                              /// </summary>
                              /// <param name="rectangle">The size and shape of the button.</param>
                              /// <param name="text">The message on the button.</param>
                              /// <param name="graphicsDevice"></param>
                              /// <param name="textureDict"></param>
                              /// <param name="buttonFont">The font of the message on the button.</param>
        public Button(Rectangle rectangle, string text, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict, SpriteFont buttonFont)
        {
            this.rectangle = rectangle;
            this.text = text;
            this.buttonFont = buttonFont;
            buttonTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);//MB: Creates a blank texture for the button
            hoverTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
            pressedTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
            disabledTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
            selectedTexture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
            buttonTexture.SetData(GetRectangleDataFromTemplate(textureDict["ButtonUnpressed"],rectangle));//MB: Sets the actual texture to the locally generated texture
            hoverTexture.SetData(GetRectangleDataFromTemplate(textureDict["ButtonHover"], rectangle));
            pressedTexture.SetData(GetRectangleDataFromTemplate(textureDict["ButtonPressed"], rectangle));
            disabledTexture.SetData(GetRectangleDataFromTemplate(textureDict["ButtonDisabled"], rectangle));
            selectedTexture.SetData(GetRectangleDataFromTemplate(textureDict["ButtonSelected"], rectangle));
        }
        /// <summary>
        /// Draws the button texture and text to a given SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw the button to.</param>
        public void Draw(SpriteBatch spriteBatch, MouseState mouseState)
        {
            if (enabled)
            {
                if (rectangle.Contains(mouseState.Position))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        spriteBatch.Draw(pressedTexture, rectangle.Location.ToVector2());
                    }
                    else
                    {
                        spriteBatch.Draw(hoverTexture, rectangle.Location.ToVector2());
                    }

                }
                else
                {
                    if (selected)
                    {
                        spriteBatch.Draw(selectedTexture, rectangle.Location.ToVector2());//MB: Draws the background
                    }
                    else
                    {
                        spriteBatch.Draw(buttonTexture, rectangle.Location.ToVector2());//MB: Draws the background
                    }
                }
            }
            else
            {
                spriteBatch.Draw(disabledTexture, rectangle.Location.ToVector2());
            }
            float width = buttonFont.MeasureString(text).X;//MB: Gets the width of the text
            float height = buttonFont.MeasureString(text).Y;//MB: Gets the height of the text
            spriteBatch.DrawString(buttonFont, text, new Vector2(rectangle.X + (rectangle.Width - width) / 2, rectangle.Y + (rectangle.Height - height) / 2), Color.Black);//MB: Draws the text in the middle of the button
        }
        /// <summary>
        /// Returns whether or not the button has been activated when given the location of a click.
        /// </summary>
        /// <param name="mousePos">The position of a mouse click.</param>
        /// <returns>Boolean</returns>
        public bool ClickCheck(Point mousePos) => enabled && rectangle.Contains(mousePos);
    }
}
#pragma warning restore CS0618