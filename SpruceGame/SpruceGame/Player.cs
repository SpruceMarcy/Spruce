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
    class Player:Actor
    {

        public Player()
        {
        }
        public void Update(KeyboardState keyboardState, MouseState mouseState, Coord movement)
        {
            base.Update(mouseState.Position.toCoord(),movement);
        }
        public new void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict)
        {
            base.Draw(spriteBatch, graphicsDevice, textureDict);
        }
    }
}
