using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace grimchase.Objects.Items;

public class Item
{
    public CoreGame GameParent;
    public Texture2D Texture;
    public int Width, Height;
    public Item(CoreGame parent)
    {
        GameParent = parent;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(Texture, position, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
    }
}