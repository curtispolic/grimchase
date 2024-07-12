using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using grimchase.Objects;

namespace grimchase;

public class CoreGame : Game
{
    public GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;
    public Tile[,] TileList;
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

        int MAP_SIZE = 15;

        TileList = new Tile[MAP_SIZE,MAP_SIZE];

        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                Vector2 tilepos = new((i+j)*32, (j-i)*16);
                TileList[i,j] = new(this, tilepos);
            }
        }

        GamePlayer = new(this, screenCenter);

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
                GamePlayer.Target = new(mouseState.X, mouseState.Y);
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

        foreach (Tile tile in TileList)
        {
            tile.Draw(_spriteBatch, GamePlayer.Position);
        }

        GamePlayer.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
