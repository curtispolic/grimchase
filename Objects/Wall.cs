using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class Wall
{
    public CoreGame GameParent;
    public Texture2D Texture;
    public Vector2 Position, Offset, ScreenCenter;
    public Wall(CoreGame parent, Vector2 pos, Vector2 screenCenter)
    {
        GameParent = parent;
        Position = pos;
        ScreenCenter = screenCenter;
        LoadContent();
    }

    public void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("wall");
        Offset = new(Texture.Width / 2, Texture.Height - 16);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Offset + playerPos - ScreenCenter, Vector2.One, SpriteEffects.None, 0f);
    }
}