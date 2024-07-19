using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class Inventory : Drawable
{
    public bool Visible;
    public Inventory(CoreGame parent, Vector2 screenCenter): base(parent, new Vector2(850, 50), screenCenter)
    {
        Collision = false;
        Visible = false;
        LoadContent();
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("inventory");
        base.LoadContent();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Overriden to draw without relation to player
        // Draw the inventory panel
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
    }
}