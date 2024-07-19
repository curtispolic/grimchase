using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace grimchase.Objects;

public class Character : Drawable
{
    public Vector2 Target;
    public List<Vector2> PathListToTarget;
    public Texture2D HealthBarTexture, HealthBarBackTexture;
    public int MaxHP, CurrentHP, AttackSpeed, Damage;
    public double SwingTimer;
    public Character(CoreGame parent, Vector2 pos, Vector2 screenCenter): base(parent, pos, screenCenter)
    {
        Target = Position;
        PathListToTarget = new();
        Collision = true;

        AttackSpeed = 250; // This is milliseconds per attack
        SwingTimer = 0;
        Damage = 10; 

        MaxHP = 100;
        CurrentHP = 100;
    }

    public virtual void Update(GameTime gameTime, List<Drawable> collidableList)
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

    public override void LoadContent()
    {
        HealthBarTexture = GameParent.Content.Load<Texture2D>("health_bar");
        HealthBarBackTexture = GameParent.Content.Load<Texture2D>("health_bar_back");
        base.LoadContent();
    }

    public virtual bool Attack(Character enemy)
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

    public virtual void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        GameParent.DrawableList.Remove(this);
        GameParent.CollidableList.Remove(this);
    }
}