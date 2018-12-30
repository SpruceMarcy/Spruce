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
        float Radius;//MB: how far the mouse can be from the position of the container when clicking on it
        Coord Position;
        List<Item> Contents;//MB: The collection of items inside this container
        [NonSerialized]//MB: Textures can't be serialized
        private Texture2D Menu;//MB: The background texture of the container interface
        string MenuTemplateKey;//MB: The string for the texture dictionary, to access the template for generation of the menu graphic.
        private List<Button> ContentList;//MB: Not used, may be unnecessary
        private bool IsMenuVisible=false;//MB: By default, the menu doesn't show
        /// <summary>
        /// Initialisation of a container
        /// </summary>
        /// <param name="textureKey">The key that will be used to get the container texture</param>
        /// <param name="MenuTemplateKey">The key that will be used to get the template for menu generation</param>
        /// <param name="Position">The position of the container in the room</param>
        /// <param name="Contents">The collection of items inside this container</param>
        /// <param name="Radius">The size of the circle for accessing the container</param>
        /// <param name="angle">Optional. The direction of the container</param>
        public Container(string textureKey,String MenuTemplateKey, Coord Position, List<Item> Contents, float Radius, float angle = 0)
        {
            this.textureKey = textureKey;
            this.Position = Position;
            this.Contents = Contents;
            this.Radius = Radius;
            this.angle = angle;
            this.MenuTemplateKey = MenuTemplateKey;
        }
        public void Draw(SpriteBatch spriteBatch, Coord RoomOffset, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> TextureDict)
        {
            Coord DrawPosition = RoomOffset + Position;//MB: The draw postition is the place on screen where it will be drawn
            spriteBatch.Draw(TextureDict[textureKey],new Rectangle( DrawPosition.ToPoint(),TextureDict[textureKey].Bounds.Size), null,Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);//MB: Draws the container to screen
            if (IsMenuVisible)
            {//MB: Draws the menu to the screen. INCOMPLETE
                Menu = new Texture2D(graphicsDevice, 100, 100);
                Menu.SetData<Color>(GetRectangleDataFromTemplate(TextureDict[MenuTemplateKey], new Rectangle(0, 0, 100, 100)));
                spriteBatch.Draw(Menu, DrawPosition.ToVector2());
            }
        }
        public void Update(MouseState mouseState, Coord RoomOffset)
        {
            if (IsMenuVisible)
            {
                //MB: If the menu is visible, hide it if the mouse leaves the menu or the container radius
                IsMenuVisible = Vector2.Distance(mouseState.Position.ToVector2(), (RoomOffset + Position).ToVector2()) < Radius || Menu.Bounds.Contains(mouseState.Position.ToVector2() - RoomOffset.ToVector2() - Position.ToVector2());
            }
            else
            {
                //MB: If the menu is hidden, show it if the mouse is clicked within the container radius
                IsMenuVisible = Vector2.Distance(mouseState.Position.ToVector2(), (RoomOffset + Position).ToVector2()) < Radius && mouseState.LeftButton == ButtonState.Pressed;
            }
        }
    }
    [Serializable]//MB: This allows an instance of this class to be written to file
    public class Item
    {
        string DisplayName;//MB: The name of the item as it appears ingame
        public Item(string DisplayName) => this.DisplayName = DisplayName;
    }
}
#pragma warning restore CS0618