using Microsoft.Xna.Framework.Graphics;

namespace grimchase.Objects.Items;

public class Ring : Item
{
    public Ring(CoreGame parent): base(parent)
    {
        Width = 1;
        Height = 1;
        LoadContent();
    }

    public void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("ring");
    }
}