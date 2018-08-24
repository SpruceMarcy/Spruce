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
        Coord PlayerPos;
        // - - - - - - - - - - - - - - - - - - -

        public SaveGame(byte[] Seed,Dictionary<string,Texture2D> TextureDict) //sub new
        {
            this.Seed = Seed;
            LoadedLevel = new Level(5,5,TextureDict,Seed,15);
            PlayerPos = new Coord(300, 300);
            PlayerTexture = TextureDict["Player"];
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState,GraphicsDevice graphicsDevice) //backend
        {
            Coord movementVector = new Coord(0,0);
            if (keyboardState.IsKeyDown(Keys.W))
            {
                movementVector +=new Coord(0,-1);
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                movementVector += new Coord(0, 1);
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                movementVector += new Coord(-1, 0);
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                movementVector += new Coord(1,0);
            }
            if (!movementVector.Equals(new Coord(0, 0)))
            {
                movementVector /= movementVector.ToVector2().Length();//makes it a unit vector
                movementVector = movementVector + movementVector;///2;
            }
            if (!isSolid(PlayerPos+movementVector))
            {
                PlayerPos += movementVector;
            }
            LoadedLevel.Update(mouseState, (new Coord(960, 540) - PlayerPos).ToVector2(),graphicsDevice);
        }

        public void Draw(SpriteBatch spriteBatch) //frontend
        {
            LoadedLevel.Draw(spriteBatch,(new Coord(960,540)-PlayerPos).ToVector2());
            {
                float angle = (float)Math.Atan2(Mouse.GetState().Position.Y - 540, Mouse.GetState().Position.X - 960) + MathHelper.PiOver2;
                Point SpritePosition = new Point((int)(960-(new Vector2(PlayerTexture.Width, PlayerTexture.Height).Length()/2 * Math.Cos(angle+Math.Atan2(PlayerTexture.Height/2, PlayerTexture.Width / 2)))), (int)(540 - (new Vector2(PlayerTexture.Width, PlayerTexture.Height).Length()/2 * Math.Sin(angle + Math.Atan2( PlayerTexture.Height / 2, PlayerTexture.Width / 2)))));
                spriteBatch.Draw(PlayerTexture, new Rectangle(SpritePosition, new Point(PlayerTexture.Width, PlayerTexture.Height)), null, Color.Black, angle, Vector2.Zero, SpriteEffects.None, 0);
            }
            
        }
        public bool isSolid(Coord Position)
        {
            Room room;
            room = LoadedLevel.rooms[(int)Math.Floor(Position.X/(16*32)),(int)Math.Floor(Position.Y/ (16 * 32))];
            Tile tile;
            tile = room.tiles[(int)Math.Floor((Position.X % (16 * 32)) / 32), (int)Math.Floor((Position.Y % (16 * 32)) / 32)];
            return tile.isSolid;
        }
    }
}
#pragma warning restore CS0618