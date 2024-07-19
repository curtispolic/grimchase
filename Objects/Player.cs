using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace grimchase.Objects;

public class Player : Drawable
{
    public Vector2 Target;
    public List<Vector2> PathListToTarget;
    public int MaxHP, CurrentHP, MaxMP, CurrentMP, AttackSpeed, Damage;
    public double SwingTimer;
    public Player(CoreGame parent, Vector2 screenCenter): base(parent, new(64,0), screenCenter)
    {
        Target = Position;
        PathListToTarget = new();
        Collision = true;

        AttackSpeed = 250; // This is milliseconds per attack
        SwingTimer = 0;
        Damage = 10; 

        MaxHP = 100;
        MaxMP = 100;
        CurrentHP = 100;
        CurrentMP = 100;

        LoadContent();
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("guy");
        base.LoadContent();
    }

    public void Update(GameTime gameTime, List<Drawable> collidableList)
    {
        SwingTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

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

        // step is direction towards target
        Vector2 step = Target - Position;
        // get the unit circle version
        step /= step.Length();
        // halve vertical speed because isometric
        step.Y /= 2;
        // adjust for framerate
        step *= (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        // divide so not too fast
        step /= 5;

        foreach (Drawable collidable in collidableList)
        {
            if (collidable.CheckCollision(Position + step) && collidable != this)
            {
                return;
            }
        }

        Position += step;

        UpdateCollisionMasks();
    }

    public bool Attack(Enemy enemy)
    {
        // Only allow attack if swing has reset
        if (SwingTimer <= 0)
        {
            SwingTimer = AttackSpeed;
            enemy.TakeDamage(Damage);
            return true;
        }

        return false;
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        // Intentional override to discard playerPos to call all Drawables together
        spriteBatch.Draw(Texture, ScreenCenter, null, Color.White, 0f, Offset, Vector2.One, SpriteEffects.None, 0f);
    }
}