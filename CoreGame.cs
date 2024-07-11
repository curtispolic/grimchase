using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace grimchase;

public class CoreGame : Game
{
    public GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;

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

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
