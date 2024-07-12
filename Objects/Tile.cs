using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class Tile
{
    public CoreGame GameParent;
    public Texture2D Texture;
    public Vector2 Position;
    public Tile(CoreGame parent, Vector2 pos)
    {
        GameParent = parent;
        Position = pos;
        LoadContent();
    }

    public void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("tile");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), Vector2.One, SpriteEffects.None, 0f);
    }
}