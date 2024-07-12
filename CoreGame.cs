using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using grimchase.Objects;

namespace grimchase;

public class CoreGame : Game
{
    public GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;
    public Tile[,] TileList;
    public Player GamePlayer;

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

        TileList = new Tile[10,10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Vector2 tilepos = new(400 + (i+j)*32, 400 + (j-i)*16);
                TileList[i,j] = new(this, tilepos);
            }
        }

        GamePlayer = new(this);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the screen and begin rendering
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        foreach (Tile tile in TileList)
        {
            tile.Draw(_spriteBatch);
        }

        GamePlayer.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
