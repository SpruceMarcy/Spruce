using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System;//MB: Allows use of [Serializable]
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    [Serializable]//MB: This means that an instance of this class can be written to file
    public class SaveGame
    {
        // - - - - Variables Global to this Save
        readonly string SaveName;//MB: No use yet
        byte[] Seed; //MB: The seed for all randomly determined elements
        Level LoadedLevel; //MB: The labyrinth that is being played
        String PlayerTextureKey; //MB: A key for the texture dictionary to retrieve the player texture
        Coord PlayerPos; //MB: The position of the player in the level. May want to move this to Level
        // - - - - - - - - - - - - - - - - - - -

        public SaveGame(byte[] Seed, Dictionary<string, Texture2D> TextureDict) //MB: on instanciation
        {
            this.Seed = Seed;
            LoadedLevel = new Level(5,5,TextureDict,Seed,15); //MB: Create a new placeholder level
            PlayerPos = new Coord(300, 300); //MB: Sets the player to (300,300) just as a placeholder
            PlayerTextureKey = "Player";
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState) //MB: Game Logic (single frame)
        {
            Coord movementVector = new Coord(0,0);//MB: This variable records where the player is moving next
            if (keyboardState.IsKeyDown(Keys.W))
            {
                movementVector +=new Coord(0,-1); //MB: If "W" is pressed, move up (y values go up as you go down the screen)
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                movementVector += new Coord(0, 1); //MB: If "S" is pressed, move down
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                movementVector += new Coord(-1, 0); //MB: If "A" is pressed, move left
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                movementVector += new Coord(1,0); //MB: If "D" is pressed, move right
            }
            if (!movementVector.Equals(new Coord(0, 0)))//MB: This avoids div by 0 errors
            {
                movementVector /= movementVector.ToVector2().Length();//MB: makes it a unit vector so that diagonal movement is not faster
                movementVector = movementVector + movementVector;//MB: doubles the speed
            }
            if (!IsSolid(PlayerPos+movementVector))
            {
                PlayerPos += movementVector;//MB: Moves to the next location if the new location is not in a wall or something
            }
            LoadedLevel.Update(mouseState, new Coord(960, 540) - PlayerPos);//MB: run the level logic
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> TextureDict) //MB: frontend stuff
        {
            LoadedLevel.Draw(spriteBatch,new Coord(960,540)-PlayerPos,graphicsDevice,TextureDict); //MB: Draw the level
            {
                //MB: This is a bodge-together approach to drawing the player at an angle since the center of rotation is in the top left of a texture by default
                Texture2D PlayerTexture = TextureDict[PlayerTextureKey];
                float angle = (float)Math.Atan2(Mouse.GetState().Position.Y - 540, Mouse.GetState().Position.X - 960) + MathHelper.PiOver2;
                Point SpritePosition = new Point((int)(960-(new Vector2(PlayerTexture.Width, PlayerTexture.Height).Length()/2 * Math.Cos(angle+Math.Atan2(PlayerTexture.Height/2, PlayerTexture.Width / 2)))), (int)(540 - (new Vector2(PlayerTexture.Width, PlayerTexture.Height).Length()/2 * Math.Sin(angle + Math.Atan2( PlayerTexture.Height / 2, PlayerTexture.Width / 2)))));
                spriteBatch.Draw(PlayerTexture, new Rectangle(SpritePosition, new Point(PlayerTexture.Width, PlayerTexture.Height)), null, Color.Black, angle, Vector2.Zero, SpriteEffects.None, 0);
            }
        }
        /// <summary>
        /// Returns whether or not the specified position is occupied, preventing movement.
        /// This could also potentially be moved to Level
        /// </summary>
        /// <param name="Position">Coordinates in the level to test</param>
        /// <returns></returns>
        public bool IsSolid(Coord Position)
        {
            Room room;
            room = LoadedLevel.rooms[(int)Math.Floor(Position.X/(16*32)),(int)Math.Floor(Position.Y/ (16 * 32))];//MB: get the room at those coordinates
            Tile tile;
            tile = room.tiles[(int)Math.Floor((Position.X % (16 * 32)) / 32), (int)Math.Floor((Position.Y % (16 * 32)) / 32)];//MB: get the tile at those coordinates
            return tile.isSolid;
        }
    }
}
#pragma warning restore CS0618