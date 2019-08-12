using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpruceGame
{
    [Serializable]
    public class Projectile
    {
        string textureKey;
        public float angle;
        public float speed;
        public Coord position;
        public Coord movement;
        [NonSerialized]
        Rectangle hitBox;
        public Projectile(string textureKey, float angle,float speed,Coord position, Coord parentMovement,Coord movement)
        {
            this.textureKey = textureKey;
            this.angle = angle;
            this.speed = speed;
            this.position = position;
            this.movement = (movement*speed)+(parentMovement*4);
        }
        public void Draw(SpriteBatch spriteBatch, Coord roomOffset, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict)
        {
            Coord DrawPosition = roomOffset + position;//MB: The draw postition is the place on screen where it will be drawn
            spriteBatch.Draw(textureDict[textureKey], new Rectangle(DrawPosition.ToPoint(), textureDict[textureKey].Bounds.Size), null, Color.White, angle, new Vector2(textureDict[textureKey].Width/2f, textureDict[textureKey].Height/2f), SpriteEffects.None, 0);//MB: Draws the projectile to screen
        }
        public void Update(MouseState mouseState)
        {
            position += movement;
        }
    }
}
