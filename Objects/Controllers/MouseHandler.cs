using grimchase.Objects.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace grimchase.Objects.Controllers;

public class MouseHandler
{
    public CoreGame GameParent;
    public bool LeftMouseDown, RightMouseDown, MiddleMouseDown, MouseFourDown, MouseFiveDown;
    public MouseHandler(CoreGame parent)
    {
        GameParent = parent;
        LeftMouseDown = false;
        RightMouseDown = false;
        MiddleMouseDown = false;
        MouseFourDown = false;
        MouseFiveDown = false;
    }

    public void Update(MouseState mouseState, GraphicsDeviceManager graphics)
    {
        // Only handle clicks inside an active game window
        if (0 <= mouseState.X && mouseState.X <= graphics.PreferredBackBufferWidth && 0 <= mouseState.Y && mouseState.Y <= graphics.PreferredBackBufferHeight && GameParent.IsActive)
        {

            // This just lives here for the moment
            Enemy FirstEnemy = GameParent.FirstEnemy;
            Player GamePlayer = GameParent.GamePlayer;
            Pathfinder pathfinder = GameParent.pathfinder;
            int[,] TileArray = GameParent.TileArray;


            // Left mouse down handling
            if (mouseState.LeftButton == ButtonState.Pressed && !LeftMouseDown)
            {
                LeftMouseDown = true;

                // TODO: Refactor
                bool enemyClick = false;
                foreach (Rectangle mask in FirstEnemy.CollisionMasks)
                {
                    if (mask.Contains(new Vector2(mouseState.X, mouseState.Y) + GamePlayer.Position - GamePlayer.ScreenCenter))
                    {
                        enemyClick = true;
                        break;
                    }
                }

                if (enemyClick)
                {
                    Vector2 distance = FirstEnemy.Position - GamePlayer.Position;
                    if (Math.Abs(distance.X) < 64 && Math.Abs(distance.Y) < 32)
                    {
                        GamePlayer.Attack(FirstEnemy);
                    }
                    else
                    {
                        GamePlayer.PathListToTarget = pathfinder.Pathfind(TileArray, GamePlayer.Position, new Vector2(mouseState.X, mouseState.Y) + GamePlayer.Position - GamePlayer.ScreenCenter);
                        GamePlayer.Target = GamePlayer.PathListToTarget[0];
                    }
                }
                else
                {
                    GamePlayer.PathListToTarget = pathfinder.Pathfind(TileArray, GamePlayer.Position, new Vector2(mouseState.X, mouseState.Y) + GamePlayer.Position - GamePlayer.ScreenCenter);
                    GamePlayer.Target = GamePlayer.PathListToTarget[0];
                }
                // END TODO lol

            }
            else if (mouseState.LeftButton == ButtonState.Released &&  LeftMouseDown)
            {
                LeftMouseDown = false;
            }

            // Right mouse handling
            if (mouseState.RightButton == ButtonState.Pressed && !RightMouseDown)
            {
                RightMouseDown = true;

                FirstEnemy.Behaviour = "aggro";
                GamePlayer.CurrentHP -= 10;

            }
            else if (mouseState.RightButton == ButtonState.Released &&  RightMouseDown)
            {
                RightMouseDown = false;
            }

            // Middle mouse handling
            if (mouseState.MiddleButton == ButtonState.Pressed && !MiddleMouseDown)
            {
                MiddleMouseDown = true;

            }
            else if (mouseState.MiddleButton == ButtonState.Released &&  MiddleMouseDown)
            {
                MiddleMouseDown = false;
            }
            
        }
    }
}
