using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using grimchase.Objects;

namespace grimchase;

public class CoreGame : Game
{
    public GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;
    public List<Drawable> DrawableList;
    public Player GamePlayer;
    public Vector2 mouseTarget;
    public bool leftMouseDown;

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
        mouseTarget = screenCenter;

        int MAP_SIZE = 50;

        DrawableList = new();

        MapGenerator mapGenerator = new(this, screenCenter);
        DrawableList = mapGenerator.CreateRoomsLevel(MAP_SIZE);

        GamePlayer = new(this, screenCenter);
        
        DrawableList.Add(GamePlayer);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        // Handle instances where the mouse is inside the game window
        if (0 <= mouseState.X && mouseState.X <= graphics.PreferredBackBufferWidth && 0 <= mouseState.Y && mouseState.Y <= graphics.PreferredBackBufferHeight)
        {
            // Left mouse down handling
            if (mouseState.LeftButton == ButtonState.Pressed && !leftMouseDown)
            {
                // To ensure only one click goes through
                leftMouseDown = true;
                GamePlayer.Target = new Vector2(mouseState.X, mouseState.Y) + GamePlayer.Position - GamePlayer.ScreenCenter;
            }
            else if (mouseState.LeftButton == ButtonState.Released &&  leftMouseDown)
            {
                leftMouseDown = false;
            }
        }

        GamePlayer.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the screen and begin rendering
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

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

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
