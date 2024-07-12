using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class Tile : Drawable
{
    public Tile(CoreGame parent, Vector2 pos, Vector2 screenCenter): base(parent, pos, screenCenter)
    {
        Collision = false;
        LoadContent();
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("tile");
        base.LoadContent();
    }
}