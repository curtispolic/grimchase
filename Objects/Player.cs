using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace grimchase.Objects;

public class Player : Drawable
{
    public Vector2 Target;
    public Player(CoreGame parent, Vector2 screenCenter): base(parent, new(64,0), screenCenter)
    {
        Target = Position;
        LoadContent();
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("guy");
        Offset = new(Texture.Width / 2, Texture.Height / 2 + 16);
    }

    public void Update(GameTime gameTime, List<Drawable> collidableList)
    {
        // Prevent divide by zero
        if ((float) gameTime.ElapsedGameTime.TotalMilliseconds == 0 || Target == Position)
        {
            return;
        }

        // Return if at (or very close to) target
        if (Math.Abs(Target.X - Position.X) < 2 && Math.Abs(Target.Y - Position.Y) < 2)
        {
            return;
        }

        Vector2 step = Target - Position;
        step /= Math.Abs(step.X) + Math.Abs(step.Y);
        step *= (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        step /= 5;

        foreach (Drawable collidable in collidableList)
        {
            if (collidable.CheckCollision(Position + step))
            {
                return;
            }
        }

        Position += step;
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        // Intentional override to discard playerPos to call all Drawables together
        spriteBatch.Draw(Texture, ScreenCenter, null, Color.White, 0f, Offset, Vector2.One, SpriteEffects.None, 0f);
    }
}