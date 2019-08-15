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
        public byte[] seed; //MB: The seed for all randomly determined elements
        public Level loadedLevel; //MB: The labyrinth that is being played
        Player player;
        // - - - - - - - - - - - - - - - - - - -

        public SaveGame(byte[] seed,Texture2D roomData) //MB: on instanciation
        {
            this.seed = seed;
            player = new Player();
            player.pos = new Coord(300, 300); //MB: Sets the player to (300,300) just as a placeholder
            player.textureKey = "Player";
            player.legsKey = "PlayerLegs";
            player.primaryWeapon = new Weapon("CandyGun","CandyBullet");
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, Dictionary<string, Texture2D> textureDict) //MB: Game Logic (single frame)
        {
            Coord movementVector = new Coord(0,0);//MB: This variable records where the player is moving next
            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (!loadedLevel.IsSolid(player.hitbox,player.pos + new Coord(0,-1)))
                {
                    movementVector +=new Coord(0,-1); //MB: If "W" is pressed, move up (y values go up as you go down the screen)
                }
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                if (!loadedLevel.IsSolid(player.hitbox, player.pos + new Coord(0, 1)))
                {
                    movementVector += new Coord(0, 1);  //MB: If "S" is pressed, move down
                }
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (!loadedLevel.IsSolid(player.hitbox, player.pos + new Coord(-1, 0))) //MB: If "A" is pressed, move left
                {
                    movementVector += new Coord(-1, 0); //MB: If "A" is pressed, move left
                }
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (!loadedLevel.IsSolid(player.hitbox, player.pos + new Coord(1,0))) //MB: If "D" is pressed, move right
                {
                    movementVector += new Coord(1, 0); //MB: If "D" is pressed, move right
                }
            }
            if (!movementVector.Equals(new Coord(0, 0)))//MB: This avoids div by 0 errors
            {
                movementVector /= movementVector.ToVector2().Length();//MB: makes it a unit vector so that diagonal movement is not faster
            }
            for (int i = 1; i <= 4; i++)
            {
                if (!loadedLevel.IsSolid(player.hitbox, player.pos+movementVector*i))
                {
                    player.pos += movementVector;//MB: Moves to the next location if the new location is not in a wall or something
                }
                else
                {
                    break;
                }
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 muzzle = new Vector2(0, textureDict[player.primaryWeapon.textureKey].Bounds.Height);
                loadedLevel.projectiles.Add(player.primaryWeapon.Fire(player.pos+ Vector2.Transform(new Vector2(8, 0), Matrix.CreateRotationZ(player.angle)).toCoord()-Vector2.Transform(muzzle,Matrix.CreateRotationZ(player.primaryWeapon.angle)).toCoord(),movementVector));
            }
            player.Update(keyboardState,mouseState,movementVector);
            loadedLevel.Update(mouseState, new Coord(960, 540) - player.pos);//MB: run the level logic
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict, Dictionary<string, MapDataPack> dataPacks) //MB: frontend stuff
        {
            loadedLevel.Draw(spriteBatch,new Coord(960,540)-player.pos,graphicsDevice,textureDict,dataPacks); //MB: Draw the level
            player.Draw(spriteBatch,graphicsDevice,textureDict);
            DrawHitboxes(spriteBatch, graphicsDevice);
        }
        public void DrawHitboxes(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            List<Hitbox> hitboxes = new List<Hitbox>();
            hitboxes.Add(player.hitbox.Adjust(new Coord(960, 540)));
            for (int y = 0; y < loadedLevel.height; y++)
            {
                for (int x = 0; x < loadedLevel.width; x++)
                {
                    Room thisRoom = loadedLevel.getRoom(x, y);
                    if (thisRoom != null)
                    {
                        for (int tiley = 0; tiley < thisRoom.tiles.GetLength(1); tiley++)
                        {
                            for (int tilex = 0; tilex < thisRoom.tiles.GetLength(0); tilex++)
                            {
                                Hitbox tileHitbox=thisRoom.tiles[tilex, tiley].hitbox == null ? null : thisRoom.tiles[tilex, tiley].hitbox.Adjust(new Coord(x * 512 + (tilex * 32), y * 512 + (tiley * 32)));
                                if (tileHitbox != null && (tileHitbox.rectangles[0].rectangle.Location.ToVector2() - player.pos.ToVector2()).Length()<400)
                                {
                                    hitboxes.Add(tileHitbox.Adjust(player.pos*-1+ new Coord(960, 540)));
                                }
                            }
                        }
                    }

                }
            }
            foreach(Door door in loadedLevel.doors)
            {
                hitboxes.Add(door.hitbox.Adjust(player.pos*-1 + new Coord(960, 540)));
            }
            foreach (Hitbox hitbox in hitboxes)
            {
                foreach (SerializableRectangle serializableRectangle in hitbox.rectangles)
                {
                    Rectangle rectangleToDraw = serializableRectangle.rectangle;
                    int thicknessOfBorder = 2;
                    Color borderColor = Color.White;
                    int pixel = SpruceContentManager.newTexture2D(new Texture2D(graphicsDevice,1,1));
                    SpruceContentManager.get(pixel).SetData(new Color[] { Color.White });
                    spriteBatch.Draw(SpruceContentManager.get(pixel), new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

                    // Draw left line
                    spriteBatch.Draw(SpruceContentManager.get(pixel), new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

                    // Draw right line
                    spriteBatch.Draw(SpruceContentManager.get(pixel), new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                                    rectangleToDraw.Y,
                                                    thicknessOfBorder,
                                                    rectangleToDraw.Height), borderColor);
                    // Draw bottom line
                    spriteBatch.Draw(SpruceContentManager.get(pixel), new Rectangle(rectangleToDraw.X,
                                                    rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                                    rectangleToDraw.Width,
                                                    thicknessOfBorder), borderColor);
                }

            }
        }
    }
}
#pragma warning restore CS0618