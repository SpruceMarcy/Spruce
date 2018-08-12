using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System;
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    public class SaveGame
    {
        // - - - - Variables Global to this Save
        readonly string SaveName;
        byte[] Seed;
        Level LoadedLevel;
        Texture2D PlayerTexture;
        Vector2 PlayerPos;
        // - - - - - - - - - - - - - - - - - - -

        public SaveGame(byte[] Seed,Dictionary<string,Texture2D> TextureDict) //sub new
        {
            this.Seed = Seed;
            LoadedLevel = new Level(5,5,TextureDict);
            PlayerPos = new Vector2(300, 300);
            PlayerTexture = TextureDict["Player"];
        }

        public void Update(KeyboardState keyboardState) //backend
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                PlayerPos +=new Vector2(0,-1);
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                PlayerPos += new Vector2(0, 1);
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                PlayerPos += new Vector2(-1, 0);
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                PlayerPos += new Vector2(1,0);
            }
        }

        public void Draw(SpriteBatch spriteBatch) //frontend
        {
            LoadedLevel.draw(spriteBatch,new Vector2(960,540)-PlayerPos);
            {
                float angle = (float)Math.Atan2(Mouse.GetState().Position.Y - 540, Mouse.GetState().Position.X - 960) + MathHelper.PiOver2;
                Point SpritePosition = new Point((int)(960-(new Vector2(PlayerTexture.Width, PlayerTexture.Height).Length()/2 * Math.Cos(angle))), (int)(540 - (new Vector2(PlayerTexture.Width, PlayerTexture.Height).Length() / 2 * Math.Sin(angle))));
                spriteBatch.Draw(PlayerTexture, new Rectangle(SpritePosition, new Point(PlayerTexture.Width, PlayerTexture.Height)), null, Color.Black, angle, Vector2.Zero, SpriteEffects.None, 0);
            }
            
        }
    }
}
#pragma warning restore CS0618