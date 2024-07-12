using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace grimchase.Objects;

public class Player
{
    public CoreGame GameParent;
    public Texture2D Texture;
    public Vector2 Position, Offset, ScreenCenter, Target;
    public Player(CoreGame parent, Vector2 screenCenter)
    {
        GameParent = parent;
        ScreenCenter = screenCenter;
        Position = new(64,0);
        Target = Position;
        Console.WriteLine($"{ScreenCenter}     {Position}     {Target}");
        LoadContent();
    }

    public void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("guy");
        Offset = new(Texture.Width / 2, Texture.Height / 2 + 16);
    }

    public void Update(GameTime gameTime)
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
        Position += step; 
        Console.WriteLine($"{ScreenCenter}     {Position}     {Target}");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, ScreenCenter, null, Color.White, 0f, Offset, Vector2.One, SpriteEffects.None, 0f);
    }
}