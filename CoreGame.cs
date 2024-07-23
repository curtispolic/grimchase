using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

using grimchase.Objects;
using grimchase.Objects.Characters;
using grimchase.Objects.Controllers;

namespace grimchase;

public class CoreGame : Game
{
    public GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;
    public List<Drawable> DrawableList, CollidableList;
    public MouseHandler mouseHandler;
    public Pathfinder pathfinder;
    public int[,] TileArray;
    public Player GamePlayer;
    public Enemy FirstEnemy;
    public BottomUI bottomUI;
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

        mouseHandler = new(this);

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

        // Use this to handle all the mouse events
        mouseHandler.Update(mouseState, graphics);

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
