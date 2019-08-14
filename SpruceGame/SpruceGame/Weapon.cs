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
    public class Weapon
    {
        public string textureKey;
        string projectileKey;
        public float angle;
        public Weapon(string textureKey, string projectileKey)
        {
            this.textureKey = textureKey;
            this.projectileKey = projectileKey;
        }
        public void Update(float angle)
        {
            this.angle = angle;
        }
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict,Coord parentPos)
        {
            Texture2D sprite = textureDict[textureKey];
            spriteBatch.Draw(sprite,new Rectangle(parentPos.ToPoint().X,parentPos.ToPoint().Y,sprite.Width,sprite.Height), null, Color.White, angle, new Vector2(sprite.Width / 2f, sprite.Height), SpriteEffects.None,0);
        }
        public Projectile Fire(Coord pos,Coord parentVelocity)
        {   
            return new Projectile(projectileKey, angle,10, pos,parentVelocity,Vector2.Transform(Vector2.UnitY, Matrix.CreateRotationZ(MathHelper.Pi + angle)).toCoord());
        }
    }
}
