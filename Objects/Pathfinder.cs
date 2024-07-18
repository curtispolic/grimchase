using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class Pathfinder
{
    public List<Vector2> Pathfind(int[,] tileArray, Vector2 position, Vector2 target)
    {
        (int X, int Y) originIndex = new((int)(position.X / 64 - position.Y / 32), (int)(position.X / 64 + position.Y / 32));
        (int X, int Y) goalIndex = new((int)(target.X / 64 - target.Y / 32), (int)(target.X / 64 + target.Y / 32));

        List<Vector2> output = new();

        // Scuffed A* below
        // openSet is nodes to be checked
        List<(int, int)> openSet = new()
        {
            originIndex
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
        gScore[originIndex.X,originIndex.Y] = 0;

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
        fScore[originIndex.X,originIndex.Y] = PathfindH(goalIndex, originIndex);

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
                output = PathfindConstruct(cameFrom, current, originIndex);
                output.Add(target);
                return output;
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

        output.Add(position);

        return output;
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
}