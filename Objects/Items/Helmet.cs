using Microsoft.Xna.Framework.Graphics;

namespace grimchase.Objects.Items;

public class Helmet : Item
{
    public Helmet(CoreGame parent): base(parent)
    {
        Width = 2;
        Height = 2;
        LoadContent();
    }

    public void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("helmet");
    }
}