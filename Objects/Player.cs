using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class Player
{
    public CoreGame GameParent;
    public Texture2D Texture;
    public Vector2 Position;
    public Player(CoreGame parent)
    {
        GameParent = parent;
        Position = new(800,450);
        LoadContent();
    }

    public void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("guy");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), Vector2.One, SpriteEffects.None, 0f);
    }
}