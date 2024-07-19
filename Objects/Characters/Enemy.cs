using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace grimchase.Objects.Characters;

public class Enemy : Character
{
    public string Behaviour;
    public Enemy(CoreGame parent, Vector2 screenCenter): base(parent, new(256,0), screenCenter)
    {
        Behaviour = "idle";
        LoadContent();
    }

    public override void Update(GameTime gameTime, List<Drawable> collidableList)
    {
        switch (Behaviour)
        {
            case "aggro":
                PathListToTarget = GameParent.pathfinder.Pathfind(GameParent.TileArray, Position, GameParent.GamePlayer.Position);
                Target = PathListToTarget[0];
                break;
            case "wander":
                // TODO
                break;
            default:
                // For "idle" or incorrect, do nothing
                break;
        }

        base.Update(gameTime, collidableList);
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("goblin");
        base.LoadContent();
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        // Backing for health bar
        spriteBatch.Draw(HealthBarBackTexture, Position - new Vector2(0,32), null, Color.White, 0f, Offset + playerPos - ScreenCenter, Vector2.One, SpriteEffects.None, 0f);
        
        // Health bar
        Rectangle healthPortion = new(new(0,0), new(64 * CurrentHP / MaxHP, 16));
        spriteBatch.Draw(HealthBarTexture, Position - new Vector2(0,32), healthPortion, Color.White, 0f, Offset + playerPos - ScreenCenter, Vector2.One, SpriteEffects.None, 0f);
        
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Offset + playerPos - ScreenCenter, Vector2.One, SpriteEffects.None, 0f);
    }
}