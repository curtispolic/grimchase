using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grimchase.Objects;

public class Player : Drawable
{
    public Vector2 Target;
    public List<Vector2> PathListToTarget;
    public Player(CoreGame parent, Vector2 screenCenter): base(parent, new(64,0), screenCenter)
    {
        Target = Position;
        PathListToTarget = new();
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

    public void Pathfind(int[,] tileArray)
    {
        (int X, int Y) playerIndex = new((int)(Position.X / 64 - Position.Y / 32), (int)(Position.X / 64 + Position.Y / 32));
        (int X, int Y) goalIndex = new((int)(Target.X / 64 - Target.Y / 32), (int)(Target.X / 64 + Target.Y / 32));

        // Scuffed A* below
        // openSet is nodes to be checked
        List<(int, int)> openSet = new()
        {
            playerIndex
        };

        // cameFrom is the node preceding said node on the cheapest path
        (int X, int Y)[,] cameFrom = new (int, int)[tileArray.GetLength(0), tileArray.GetLength(1)];

        // gScore is the cost of the cheapest path to that node
        int[,] gScore = new int[tileArray.GetLength(0), tileArray.GetLength(1)];
        for (int i = 0; i < gScore.GetLength(0); i++)
        {
            for (int j = 0; j < gScore.GetLength(1); j++)
            {
                gScore[i,j] = 999999999;
            }
        }
        gScore[playerIndex.X,playerIndex.Y] = 0;

        // fScore is gScore plus hueristic, representing best guess as cost of a path from start to finish
        // that goes through that node
        int[,] fScore = new int[tileArray.GetLength(0), tileArray.GetLength(1)];
        for (int i = 0; i < fScore.GetLength(0); i++)
        {
            for (int j = 0; j < fScore.GetLength(1); j++)
            {
                fScore[i,j] = 999999999;
            }
        }
        fScore[playerIndex.X,playerIndex.Y] = PathfindH(goalIndex, playerIndex);

        while (openSet.Count > 0)
        {
            // Check the lowest fScore node and return if it's the goal
            (int X, int Y) current = openSet[0];
            foreach ((int X, int Y) node in openSet)
            {
                if (fScore[node.X,node.Y] < fScore[current.X,current.Y]) current = node;
            }
            if (current == goalIndex)
            {
                PathListToTarget = PathfindConstruct(cameFrom, current, playerIndex);
                PathListToTarget.Add(Target);
                Target = PathListToTarget[0];
                return;
            }

            openSet.Remove(current);

            // Neighbour checking
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    // continue if both zero because that is current node
                    if (i == 0 && j == 0) continue;
                    (int X, int Y) neighbourIndex = (current.X + i, current.Y + j);
                    // conitnue if outside array bounds
                    if (neighbourIndex.X < 0 || neighbourIndex.Y < 0 || neighbourIndex.X >= tileArray.GetLength(0) || neighbourIndex.Y >= tileArray.GetLength(1)) continue;
                    // continue if wall
                    if (tileArray[neighbourIndex.X, neighbourIndex.Y] == 1) continue;

                    int tentativeG = gScore[current.X, current.Y] + 1;
                    if (tentativeG < gScore[neighbourIndex.X, neighbourIndex.Y])
                    {
                        // Better path to neighbour
                        cameFrom[neighbourIndex.X, neighbourIndex.Y] = current;
                        gScore[neighbourIndex.X, neighbourIndex.Y] = tentativeG;
                        fScore[neighbourIndex.X, neighbourIndex.Y] = tentativeG + PathfindH(goalIndex, neighbourIndex);
                        if (!openSet.Contains(neighbourIndex))
                        {
                            openSet.Add(neighbourIndex);
                        }
                    }
                }
            }
        }

        return;
    }

    public List<Vector2> PathfindConstruct((int X, int Y)[,] cameFrom, (int X, int Y) current, (int X, int Y) start)
    {
        List<(int X, int Y)> nodes = new()
        {
            current
        };
        (int X, int Y) workingNode = current;
        while (true)
        {
            if (workingNode == start) break;
            workingNode = cameFrom[workingNode.X, workingNode.Y];
            nodes.Insert(0,workingNode);
        }

        nodes.RemoveAt(0);

        List<Vector2> output = new();
        foreach ((int X, int Y) node in nodes)
        {
            output.Add(new((node.X+node.Y)*32, (node.Y-node.X)*16));
        }
        
        return output;
    }

    public int PathfindH((int X, int Y) goalIndex, (int X, int Y)hIndex)
    {
        return Math.Abs((int)goalIndex.X - hIndex.X) + (int)Math.Abs(goalIndex.Y - hIndex.Y);
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 playerPos)
    {
        // Intentional override to discard playerPos to call all Drawables together
        spriteBatch.Draw(Texture, ScreenCenter, null, Color.White, 0f, Offset, Vector2.One, SpriteEffects.None, 0f);
    }
}