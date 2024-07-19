using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

using grimchase.Objects;
using grimchase.Objects.Characters;

namespace grimchase;

public class CoreGame : Game
{
    public GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;
    public List<Drawable> DrawableList, CollidableList;
    public Pathfinder pathfinder;
    public int[,] TileArray;
    public Player GamePlayer;
    public Enemy FirstEnemy;
    public BottomUI bottomUI;
    public Vector2 mouseTarget;
    public bool leftMouseDown, rightMouseDown, iKeyDown;

    public CoreGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Setup the base graphics settings
        graphics.IsFullScreen = false;
        graphics.PreferredBackBufferWidth = 1600;
        graphics.PreferredBackBufferHeight = 900;
        graphics.ApplyChanges();
        Vector2 screenCenter = new(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

        leftMouseDown = false;
        rightMouseDown = false;
        iKeyDown = false;
        mouseTarget = screenCenter;

        int MAP_SIZE = 50;

        DrawableList = new();

        MapGenerator mapGenerator = new(this, screenCenter);
        (DrawableList, TileArray) = mapGenerator.CreateRoomsLevel(MAP_SIZE);
        CollidableList = new();

        pathfinder = new();

        GamePlayer = new(this, screenCenter);
        
        DrawableList.Add(GamePlayer);

        FirstEnemy = new(this, screenCenter);

        DrawableList.Add(FirstEnemy);

        bottomUI = new(this, screenCenter);

        foreach (Drawable drawable in DrawableList)
        {
            if (drawable.Collision)
            {
                CollidableList.Add(drawable);
            }
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        var keyboardState = Keyboard.GetState();

        // Handle instances where the mouse is inside the game window
        if (0 <= mouseState.X && mouseState.X <= graphics.PreferredBackBufferWidth && 0 <= mouseState.Y && mouseState.Y <= graphics.PreferredBackBufferHeight)
        {
            // Left mouse down handling
            if (mouseState.LeftButton == ButtonState.Pressed && !leftMouseDown)
            {
                // To ensure only one click goes through
                leftMouseDown = true;

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
            }
            else if (mouseState.LeftButton == ButtonState.Released &&  leftMouseDown)
            {
                leftMouseDown = false;
            }

            // Right mouse down handling
            if (mouseState.RightButton == ButtonState.Pressed && !rightMouseDown)
            {
                rightMouseDown = true;
                FirstEnemy.Behaviour = "aggro";
                GamePlayer.CurrentHP -= 10;
            }
            else if (mouseState.RightButton == ButtonState.Released &&  rightMouseDown)
            {
                rightMouseDown = false;
            }
        }


        if (keyboardState.IsKeyDown(Keys.I) && !iKeyDown)
        {
            iKeyDown = true;
            GamePlayer.Invent.Visible = !GamePlayer.Invent.Visible;
        }
        else if (!keyboardState.IsKeyDown(Keys.I) && iKeyDown)
        {
            iKeyDown = false;
        }

        GamePlayer.Update(gameTime, CollidableList);

        FirstEnemy.Update(gameTime, CollidableList);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the screen and begin rendering
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        // Sort drawables top to bottom to draw in order
        DrawableList.Sort(Drawable.Comparison);

        // Draw all of the floor first to prevent any shenanigans
        foreach (Drawable thing in DrawableList)
        {
            if (thing is Tile)
            {
                thing.Draw(_spriteBatch, GamePlayer.Position);
            }
        }

        // Draw all the non tiles afterwards
        foreach (Drawable thing in DrawableList)
        {
            if (thing is not Tile)
            {
                thing.Draw(_spriteBatch, GamePlayer.Position);
            }
        }

        bottomUI.Draw(_spriteBatch);

        if (GamePlayer.Invent.Visible) GamePlayer.Invent.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
