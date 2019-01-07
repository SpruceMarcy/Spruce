using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;//MB: Imports dictionaries
using System;//MB: Allows use of [Serializable]
using static SpruceGame.GlobalMethods;
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    [Serializable]//MB: This allows an instance of this class to be written to file
    public class Container
    {
        string textureKey;//MB: The string to access the container texture from a texture dictionary
        float angle;//MB: The angle of the container
        float radius;//MB: how far the mouse can be from the position of the container when clicking on it
        Coord position;
        List<Item> contents;//MB: The collection of items inside this container
        [NonSerialized]//MB: Textures can't be serialized
        private Texture2D menu;//MB: The background texture of the container interface
        string menuTemplateKey;//MB: The string for the texture dictionary, to access the template for generation of the menu graphic.
        private List<UIButton> contentList;//MB: Not used, may be unnecessary
        private bool isMenuVisible=false;//MB: By default, the menu doesn't show
        /// <summary>
        /// Initialisation of a container
        /// </summary>
        /// <param name="textureKey">The key that will be used to get the container texture</param>
        /// <param name="menuTemplateKey">The key that will be used to get the template for menu generation</param>
        /// <param name="position">The position of the container in the room</param>
        /// <param name="contents">The collection of items inside this container</param>
        /// <param name="radius">The size of the circle for accessing the container</param>
        /// <param name="angle">Optional. The direction of the container</param>
        public Container(string textureKey, string menuTemplateKey, Coord position, List<Item> contents, float radius, float angle = 0)
        {
            this.textureKey = textureKey;
            this.position = position;
            this.contents = contents;
            this.radius = radius;
            this.angle = angle;
            this.menuTemplateKey = menuTemplateKey;
        }
        public void Draw(SpriteBatch spriteBatch, Coord roomOffset, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict)
        {
            Coord DrawPosition = roomOffset + position;//MB: The draw postition is the place on screen where it will be drawn
            spriteBatch.Draw(textureDict[textureKey],new Rectangle( DrawPosition.ToPoint(),textureDict[textureKey].Bounds.Size), null,Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);//MB: Draws the container to screen
            if (isMenuVisible)
            {//MB: Draws the menu to the screen. INCOMPLETE
                menu = new Texture2D(graphicsDevice, 100, 100);
                menu.SetData<Color>(GetRectangleDataFromTemplate(textureDict[menuTemplateKey], new Rectangle(0, 0, 100, 100)));
                spriteBatch.Draw(menu, DrawPosition.ToVector2());
            }
        }
        public void Update(MouseState mouseState, Coord roomOffset)
        {
            if (isMenuVisible)
            {
                //MB: If the menu is visible, hide it if the mouse leaves the menu or the container radius
                isMenuVisible = Vector2.Distance(mouseState.Position.ToVector2(), (roomOffset + position).ToVector2()) < radius || menu.Bounds.Contains(mouseState.Position.ToVector2() - roomOffset.ToVector2() - position.ToVector2());
            }
            else
            {
                //MB: If the menu is hidden, show it if the mouse is clicked within the container radius
                isMenuVisible = Vector2.Distance(mouseState.Position.ToVector2(), (roomOffset + position).ToVector2()) < radius && mouseState.LeftButton == ButtonState.Pressed;
            }
        }
    }
    [Serializable]//MB: This allows an instance of this class to be written to file
    public class Item
    {
        string displayName;//MB: The name of the item as it appears ingame
        public Item(string displayName) => this.displayName = displayName;
    }
}
#pragma warning restore CS0618