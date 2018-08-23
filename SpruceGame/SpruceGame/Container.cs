using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using static SpruceGame.GlobalMethods;
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    class Container
    {
        Texture2D texture;
        float angle;
        float Radius;
        Vector2 Position;
        List<Item> Contents;
        public Container(Texture2D texture, Vector2 Position, List<Item> Contents, float Radius, float angle = 0)
        {
            this.texture = texture;
            this.Position = Position;
            this.Contents = Contents;
            this.Radius = Radius;
            this.angle = angle;
        }
        void Draw(SpriteBatch spriteBatch, Vector2 RoomOffset, Point mousePos)
        {
            Vector2 DrawPosition = RoomOffset + Position;
            spriteBatch.Draw(texture,new Rectangle( DrawPosition.ToPoint(),texture.Bounds.Size), null, Color.Black, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
    class Item
    {

    }
}
#pragma warning restore CS0618