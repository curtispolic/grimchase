using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace grimchase.Objects.Characters;

public class Player : Character
{
    public int MaxMP, CurrentMP;
    public Inventory Invent;
    public Player(CoreGame parent, Vector2 screenCenter): base(parent, new(64,0), screenCenter)
    {
        MaxMP = 100;
        CurrentMP = 100;

        Invent = new(parent, screenCenter);

        LoadContent();
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("guy");
        base.LoadContent();
    }

    public override void Update(GameTime gameTime, List<Drawable> collidableList)
    {
        base.Update(gameTime, collidableList);
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        // Intentional override to discard playerPos to call all Drawables together
        spriteBatch.Draw(Texture, ScreenCenter, null, Color.White, 0f, Offset, Vector2.One, SpriteEffects.None, 0f);
    }
}