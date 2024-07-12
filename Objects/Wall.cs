using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class Wall : Drawable
{
    public Texture2D TransparentTexture;
    public Wall(CoreGame parent, Vector2 pos, Vector2 screenCenter): base(parent, pos, screenCenter)
    {
        Collision = true;
        LoadContent();
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("wall");
        TransparentTexture = GameParent.Content.Load<Texture2D>("wall_clear");
        base.LoadContent();
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        Vector2 test = Position - playerPos;
        if (test.X > -48 && test.X < 32 && test.Y < 80 && test.Y > -4)
        {
            spriteBatch.Draw(TransparentTexture, Position, null, Color.White, 0f, Offset + playerPos - ScreenCenter, Vector2.One, SpriteEffects.None, 0f);
        }
        else
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Offset + playerPos - ScreenCenter, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}