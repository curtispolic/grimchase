using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace grimchase.Objects;

public class BottomUI : Drawable
{
    public Texture2D HealthOrbTexture, ManaOrbTexture;
    public BottomUI(CoreGame parent, Vector2 screenCenter): base(parent, new Vector2(400, 772), screenCenter)
    {
        Collision = false;
        LoadContent();
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("bottomUI");
        HealthOrbTexture = GameParent.Content.Load<Texture2D>("health_orb");
        ManaOrbTexture = GameParent.Content.Load<Texture2D>("mana_orb");
        base.LoadContent();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Scale down for now
        Vector2 scale = new((float)0.5, (float)0.5);

        // Draw a health circle, cropped based on HP amount
        float hpPercent = (float)GameParent.GamePlayer.CurrentHP / GameParent.GamePlayer.MaxHP;
        Rectangle orbPortion = new(0, (int)(HealthOrbTexture.Height - HealthOrbTexture.Height * hpPercent), HealthOrbTexture.Width, (int)(HealthOrbTexture.Height * hpPercent));
        Vector2 offset = new(50, (int)(HealthOrbTexture.Height * (1 - hpPercent)));
        offset *= scale;
        spriteBatch.Draw(HealthOrbTexture, Position + offset, orbPortion, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        // Same for mana
        float mpPercent = (float)GameParent.GamePlayer.CurrentMP / GameParent.GamePlayer.MaxMP;
        orbPortion = new(new Point(0, (int)(ManaOrbTexture.Height - ManaOrbTexture.Height * mpPercent)), new Point(ManaOrbTexture.Width, (int)(ManaOrbTexture.Height * mpPercent)));
        offset = new(1295, (int)(ManaOrbTexture.Height * (1 - mpPercent)));
        offset *= scale;
        spriteBatch.Draw(ManaOrbTexture, Position + offset, orbPortion, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        // Draw the whole UI bar
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }
}