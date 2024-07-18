using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace grimchase.Objects;

public class Enemy : Drawable
{
    public Vector2 Target;
    public List<Vector2> PathListToTarget;
    public Texture2D HealthBarTexture, HealthBarBackTexture;
    public string Behaviour;
    public int MaxHP, CurrentHP, MaxMP, CurrentMP;
    public Enemy(CoreGame parent, Vector2 screenCenter): base(parent, new(256,0), screenCenter)
    {
        Target = Position;
        PathListToTarget = new();
        Behaviour = "idle";
        Collision = true;
        CurrentHP = 70;
        MaxHP = 100;
        LoadContent();
    }

    public void Update(GameTime gameTime, List<Drawable> collidableList)
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

        // Return if at (or very close to) target (unless there's pathfinding left)
        if (Math.Abs(Target.X - Position.X) < 2 && Math.Abs(Target.Y - Position.Y) < 2)
        {
            if (PathListToTarget.Count > 1)
            {
                PathListToTarget.RemoveAt(0);
                Target = PathListToTarget[0];
            }
            else
            {
                return;
            }
        }

        Vector2 step = Target - Position;
        step /= step.Length();
        step *= (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        step /= 5;

        foreach (Drawable collidable in collidableList)
        {
            if (collidable.CheckCollision(Position + step) && collidable != this)
            {
                Behaviour = "idle";
                return;
            }
        }

        Position += step;

        UpdateCollisionMasks();
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("goblin");
        HealthBarTexture = GameParent.Content.Load<Texture2D>("health_bar");
        HealthBarBackTexture = GameParent.Content.Load<Texture2D>("health_bar_back");
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