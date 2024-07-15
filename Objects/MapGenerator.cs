using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;

namespace grimchase.Objects;

public class MapGenerator
{
    public CoreGame Parent;
    public Vector2 ScreenCenter;
    public MapGenerator(CoreGame parent, Vector2 screenCenter)
    {
        Parent = parent;
        ScreenCenter = screenCenter;
    }

    public List<Drawable> CreateRoomsLevel(int levelSize)
    {
        // Create a level that splits down the rooms
        int[,] tileArray = new int[levelSize, levelSize];

        // Make the level one big room
        for (int i = 0; i < levelSize; i ++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                if (i == 0 || j == 0 || i == levelSize - 1 || j == levelSize - 1)
                {
                    tileArray[i,j] = 1;
                }
                else
                {
                    tileArray[i,j] = 2;
                }
            }
        }

        List<Tuple<int, int, int, int>> roomList = RecurseRooms(tileArray, 0, 0, levelSize - 1, levelSize - 1);
        PrintLevel(tileArray, levelSize);

        return CreateDrawables(tileArray, levelSize);
    }

    public List<Tuple<int,int,int,int>> RecurseRooms(int[,] tileArray, int startX, int startY, int endX, int endY)
    {
        Random rand = new();
        List<Tuple<int, int, int, int>> output = new();

        // 15% chance to not split a room
        if (rand.Next(20) < 3)
        {
            output.Add(new Tuple<int, int, int, int>(startX, startY, endX, endY));
            return output;
        }

        // 50/50 for split direction
        if (rand.Next(2) == 0)
        {
            // Split on x
            // Don't split rooms down too small
            if (endX - startX < 13)
            {
                output.Add(new Tuple<int, int, int, int>(startX, startY, endX, endY));
                return output;
            }
            else
            {
                // Put a wall at a random spot along X
                int midX = rand.Next(startX + 5, endX - 5);
                for (int i = startY; i <= endY; i++)
                {
                    tileArray[midX,i] = 1;
                }
                // Recurse into the two new rooms
                output.AddRange(RecurseRooms(tileArray, startX, startY, midX, endY));
                output.AddRange(RecurseRooms(tileArray, midX, startY, endX, endY));
            }
        }
        else
        {
            // Split on y
            // Don't split rooms down too small
            if (endY - startY < 11)
            {
                output.Add(new Tuple<int, int, int, int>(startX, startY, endX, endY));
                return output;
            }
            else
            {
                // Put a wall at a random spot along X
                int midY = rand.Next(startY + 4, endY - 4);
                for (int i = startX; i <= endX; i++)
                {
                    tileArray[i,midY] = 1;
                }
                // Recurse into the two new rooms
                output.AddRange(RecurseRooms(tileArray, startX, startY, endX, midY));
                output.AddRange(RecurseRooms(tileArray, startX, midY, endX, endY));
            }
        }

        return output;
    }

    public List<Drawable> CreateHallwayLevel(int levelSize)
    {
        // Creates a level with rooms and hallways
        // TODO: Hallways
        int[,] tileArray = new int[levelSize, levelSize];
        Random rand = new();
        int fails = 0;

        while (fails < 100)
        {
            bool fail = false;
            int x = rand.Next(levelSize); int y = rand.Next(levelSize);
            int width = 0; int height = 0;
            // Fail if can't make a big enough room
            if (levelSize - x > 5 && levelSize - y > 5)
            {
                width = rand.Next(5, levelSize - x); height = rand.Next(5, levelSize - y);
                // Fail if overlaps an existing room
                for (int i = x; i < x + width; i++)
                {
                    for (int j = y; j < y + height; j++)
                    {
                        if (tileArray[i,j] != 0)
                        {
                            fail = true;
                            break;
                        }
                    }
                    if (fail)
                    {
                        break;
                    }
                }
            }
            else 
            {
                fail = true;
            }

            // Create room if not a fail, else increment
            if (fail)
            {
                fails++;
            }
            else
            {
                // Success; mark and reset
                fails = 0;
                
                for (int i = x; i < x + width; i ++)
                {
                    for (int j = y; j < y + height; j++)
                    {
                        if (i == x || j == y || i == x + width - 1 || j == y + height - 1)
                        {
                            tileArray[i,j] = 1;
                        }
                        else
                        {
                            tileArray[i,j] = 2;
                        }
                    }
                }
            }
        }

        PrintLevel(tileArray, levelSize);
        return CreateDrawables(tileArray, levelSize);
    }

    public List<Drawable> CreateDrawables(int[,] tilesArray, int levelSize)
    {
        List<Drawable> output = new();

        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                Vector2 objpos = new((i+j)*32, (j-i)*16);
                if (tilesArray[i,j] == 1)
                {
                    output.Add(new Wall(Parent, objpos, ScreenCenter));
                }
                else if (tilesArray[i,j] == 2)
                {
                    output.Add(new Tile(Parent, objpos, ScreenCenter));
                }
            }
        }

        return output;
    }

    public void PrintLevel(int[,] level, int levelSize)
    {
        string print = "";
        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                if (level[j,i] == 1)
                {
                    print += "#";
                }
                else if (level[j,i] == 2)
                {
                    print += ".";
                }
                else
                {
                    print += " ";
                }
            }
            print += "\n";
        }

        Console.WriteLine(print);
    }
}