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
    class Player
    {
        public string textureKey; //MB: A key for the texture dictionary to retrieve the player texture
        public Coord pos; //MB: The position of the player in the level. May want to move this to Level
        float angle;
        public Player()
        {

        }
        public void Update(KeyboardState keyboardState, MouseState mouseState)
        {
            angle = (float)Math.Atan2(mouseState.Position.Y - 540, mouseState.Position.X - 960) + MathHelper.PiOver2;
        }
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict)
        {
            //MB: This is a bodge-together approach to drawing the player at an angle since the center of rotation is in the top left of a texture by default
            Texture2D PlayerTexture = textureDict[textureKey];
            Point SpritePosition = new Point(960, 540);
            spriteBatch.Draw(PlayerTexture, new Rectangle(SpritePosition, new Point(PlayerTexture.Width, PlayerTexture.Height)), null, Color.Black, angle, new Vector2(PlayerTexture.Width / 2f, PlayerTexture.Height / 2f), SpriteEffects.None, 0);
        }
    }
}
