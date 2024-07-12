using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class Tile
{
    public CoreGame GameParent;
    public Texture2D Texture;
    public Vector2 Position, Offset;
    public Tile(CoreGame parent, Vector2 pos)
    {
        GameParent = parent;
        Position = pos;
        LoadContent();
    }

    public void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("tile");
        Offset = new(Texture.Width / 2, Texture.Height / 2);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Offset - playerPos, Vector2.One, SpriteEffects.None, 0f);
    }
}