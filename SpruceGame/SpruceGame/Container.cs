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
        string textureKey;
        float angle;
        float Radius;
        Coord Position;
        List<Item> Contents;
        [NonSerialized]
        private Texture2D Menu;
        string MenuTemplateKey;
        private List<Button> ContentList;
        private bool IsMenuVisible=false;
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
            Coord DrawPosition = RoomOffset + Position;
            spriteBatch.Draw(TextureDict[textureKey],new Rectangle( DrawPosition.ToPoint(),TextureDict[textureKey].Bounds.Size), null,Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
            if (IsMenuVisible)
            {
                Menu = new Texture2D(graphicsDevice, 100, 100);
                Menu.SetData<Color>(GetRectangleDataFromTemplate(TextureDict[MenuTemplateKey], new Rectangle(0, 0, 100, 100)));
                spriteBatch.Draw(Menu, DrawPosition.ToVector2());
            }
        }
        public void Update(MouseState mouseState, Coord RoomOffset)
        {
            if (IsMenuVisible)
            {
                IsMenuVisible = Vector2.Distance(mouseState.Position.ToVector2(), (RoomOffset + Position).ToVector2()) < Radius || Menu.Bounds.Contains(mouseState.Position.ToVector2()-RoomOffset.ToVector2()-Position.ToVector2());
            }
            else
            {
                IsMenuVisible = Vector2.Distance(mouseState.Position.ToVector2(), (RoomOffset + Position).ToVector2()) < Radius && mouseState.LeftButton==ButtonState.Pressed;

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