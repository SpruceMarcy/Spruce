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
    class Weapon
    {
        string textureKey;
        public Weapon(string textureKey)
        {
            this.textureKey = textureKey;
        }
        public void Update(MouseState mouseState, Coord movement)
        {
            
        }
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict,Coord parentPos,float angle)
        {
            Texture2D sprite = textureDict[textureKey];
            spriteBatch.Draw(sprite,new Rectangle(parentPos.ToPoint().X,parentPos.ToPoint().Y,sprite.Width,sprite.Height), null, Color.White, angle, new Vector2(sprite.Width / 2f, sprite.Height), SpriteEffects.None,0);
        }
    }
}
