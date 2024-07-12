using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class Drawable
{
    public CoreGame GameParent;
    public Texture2D Texture;
    public Vector2 Position, Offset, ScreenCenter;
    public bool Collision;
    public Drawable(CoreGame parent, Vector2 pos, Vector2 screenCenter)
    {
        GameParent = parent;
        Position = pos;
        ScreenCenter = screenCenter;
    }

    public static int Comparison(Drawable a, Drawable b)
    {
        if (a.Position.Y < b.Position.Y) return -1;
        else if (a.Position.Y > b.Position.Y) return 1;
        else return 0;
    }

    public virtual void LoadContent()
    {
        Offset = new(Texture.Width / 2, Texture.Height - 16);
    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Offset + playerPos - ScreenCenter, Vector2.One, SpriteEffects.None, 0f);
    }
}