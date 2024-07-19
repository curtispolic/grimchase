using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using grimchase.Objects.Items;

namespace grimchase.Objects;

public class Inventory : Drawable
{
    public bool Visible;
    public bool[,] TileOccupied;
    public Item[,] Items;
    public Inventory(CoreGame parent, Vector2 screenCenter): base(parent, new Vector2(850, 50), screenCenter)
    {
        Collision = false;
        Visible = false;
        TileOccupied = new bool[10,4];
        Items = new Item[10,4];
        for (int i = 0; i < Items.GetLength(0); i++)
        {
            for (int j = 0; j < Items.GetLength(1); j++)
            {
                TileOccupied[i,j] = false;
            }
        }

        Items[0,0] = new Ring(GameParent);
        TileOccupied[0,0] = true;

        Items[1,1] = new Helmet(GameParent);
        TileOccupied[1,1] = true;
        TileOccupied[2,1] = true;
        TileOccupied[1,2] = true;
        TileOccupied[2,2] = true;

        Items[0,3] = new Ring(GameParent);
        TileOccupied[0,3] = true;

        Items[9,3] = new Ring(GameParent);
        TileOccupied[9,3] = true;

        LoadContent();
    }

    public override void LoadContent()
    {
        Texture = GameParent.Content.Load<Texture2D>("inventory");
        base.LoadContent();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Overriden to draw without relation to player
        // Draw the inventory panel
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

        // Draw the items
        bool[,] hasDrawn = new bool[10,4];
        for (int i = 0; i < Items.GetLength(0); i++)
        {
            for (int j = 0; j < Items.GetLength(1); j++)
            {
                hasDrawn[i,j] = false;
            }
        }

        for (int i = 0; i < Items.GetLength(0); i++)
        {
            for (int j = 0; j < Items.GetLength(1); j++)
            {
                // If this tile hasnt been drawn already and contains something
                if (!hasDrawn[i,j] && TileOccupied[i,j])
                {
                    Items[i,j].Draw(spriteBatch, Position + new Vector2(27 + i*64, 481 + j*64));
                    // Mark the tiles of that item as drawn
                    for (int a = 0; a < Items[i,j].Width; a++)
                    {
                        for (int b = 0; b < Items[i,j].Height; b++)
                        {
                            hasDrawn[i+a,j+b] = true;
                        }
                    }
                }
            }
        }
    }
}