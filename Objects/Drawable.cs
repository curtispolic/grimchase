using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace grimchase.Objects;

public class Drawable
{
    public CoreGame GameParent;
    public Texture2D Texture;
    public Vector2 Position, Offset, ScreenCenter;
    public List<Rectangle> CollisionMasks;
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
        CollisionMasks = new()
        {
            new((int)Position.X - 28, (int)Position.Y - 3, 56, 6),
            new((int)Position.X - 22, (int)Position.Y - 6, 44, 12),
            new((int)Position.X - 16, (int)Position.Y - 9, 32, 18),
            new((int)Position.X - 10, (int)Position.Y - 12, 20, 24),
            new((int)Position.X - 4, (int)Position.Y - 15, 8, 30)
        };
    }

    public bool CheckCollision(Vector2 pos)
    {
        foreach (Rectangle rect in CollisionMasks)
        {
            if (rect.Contains(pos)) return true;
        }

        return false;
    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Offset + playerPos - ScreenCenter, Vector2.One, SpriteEffects.None, 0f);
    }
}