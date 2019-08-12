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
    class Actor
    {
        public string textureKey; //MB: A key for the texture dictionary to retrieve the player texture
        public string legsKey;
        public Coord pos; //MB: The position of the player in the level. May want to move this to Level
        private Coord target=new Coord(0,0);
        private Coord SpritePosition = new Coord(960, 540);
        private Coord weaponPos=new Coord(0,0);
        public Weapon primaryWeapon;
        public float angle;
        float legsAngle;
        protected double anim = 0;
        protected bool foot = true;
        protected void Update( Coord target, Coord movement)
        {
            this.target = target;
            angle = (float)Math.Atan2(target.y - 540, target.x - 960) + MathHelper.PiOver2;
            if (movement.y == 0 && movement.x == 0)
            {
                legsAngle = angle;
                anim = 0;
            }
            else
            {
                if (foot)
                {
                    anim += 0.1;
                }
                else
                {
                    anim -= 0.1;
                }
                if (anim >= 1)
                {
                    foot = false;
                }
                else if (anim <= -1)
                {
                    foot = true;
                }
                legsAngle = (float)Math.Atan2(movement.y, movement.x) + MathHelper.PiOver2;
            }
            weaponPos = (SpritePosition.ToVector2() + Vector2.Transform(new Vector2(8, 0), Matrix.CreateRotationZ(angle))).toCoord();
            primaryWeapon.Update((float)Math.Atan2(target.x - weaponPos.x, weaponPos.y - target.y));
        }
        protected void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict)
        {
            //MB: This is a bodge-together approach to drawing the player at an angle since the center of rotation is in the top left of a texture by default
            Texture2D PlayerTexture = textureDict[textureKey];
            Texture2D LegsTexture = textureDict[legsKey];
            spriteBatch.Draw(LegsTexture, new Rectangle(SpritePosition.ToPoint(), new Point(LegsTexture.Width, (int)(LegsTexture.Height * Math.Abs(anim)))), null, Color.White, Math.Abs(MathHelper.WrapAngle(angle - legsAngle)) < MathHelper.PiOver2 ? legsAngle : legsAngle + MathHelper.Pi, new Vector2(LegsTexture.Width / 2f, LegsTexture.Height / 2f), anim < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
           
            primaryWeapon.Draw(spriteBatch,graphicsDevice,textureDict, weaponPos);
            spriteBatch.Draw(PlayerTexture, new Rectangle(SpritePosition.ToPoint(), new Point(PlayerTexture.Width, PlayerTexture.Height)), null, Color.White, angle, new Vector2(PlayerTexture.Width / 2f, PlayerTexture.Height / 2f), SpriteEffects.None, 0);
        }
    }
}
