using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System;
using static SpruceGame.GlobalMethods;
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    [Serializable]
    public class Container
    {
        Texture2D texture;
        float angle;
        float Radius;
        Vector2 Position;
        List<Item> Contents;
        private Texture2D Menu;
        private Texture2D MenuTemplate;
        private List<Button> ContentList;
        private bool IsMenuVisible=false;
        public Container(Texture2D texture,Texture2D MenuTemplate, Vector2 Position, List<Item> Contents, float Radius, float angle = 0)
        {
            this.texture = texture;
            this.Position = Position;
            this.Contents = Contents;
            this.Radius = Radius;
            this.angle = angle;
            this.MenuTemplate = MenuTemplate;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 RoomOffset)
        {
            Vector2 DrawPosition = RoomOffset + Position;
            spriteBatch.Draw(texture,new Rectangle( DrawPosition.ToPoint(),texture.Bounds.Size), null,Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
            if (IsMenuVisible)
            {
                spriteBatch.Draw(Menu, DrawPosition);
            }
        }
        public void Update(MouseState mouseState, Vector2 RoomOffset,GraphicsDevice graphicsDevice)
        {
            if (IsMenuVisible)
            {
                IsMenuVisible = Vector2.Distance(mouseState.Position.ToVector2(), RoomOffset + Position) < Radius || Menu.Bounds.Contains(mouseState.Position.ToVector2()-RoomOffset-Position);
            }
            else
            {
                IsMenuVisible = Vector2.Distance(mouseState.Position.ToVector2(), RoomOffset + Position) < Radius && mouseState.LeftButton==ButtonState.Pressed;
                if (IsMenuVisible)
                {
                    Menu = new Texture2D(graphicsDevice,100,100);
                    Menu.SetData<Color>(GetRectangleDataFromTemplate(MenuTemplate, new Rectangle(0, 0, 100, 100)));
                }
            }
        }
    }
    [Serializable]
    public class Item
    {
        string DisplayName;
        public Item(string DisplayName)
        {
            this.DisplayName = DisplayName;
        }
    }
}
#pragma warning restore CS0618