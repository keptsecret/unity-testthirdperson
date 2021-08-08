using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGen_CellularAutomata : IMapGenerator
{
    public int Width;
    public int Height;

    public string Seed;
    public bool UseRandomSeed;
    [Min(0)] public int InitialIterations;

    [Range(0, 100)] public int RandomFillPercent;

    private int[,] _map;

    public MapGen_CellularAutomata(int width, int height, int initialIterations, int randomFillPercent)
    {
        Width = width;
        Height = height;
        UseRandomSeed = true;
        InitialIterations = initialIterations;
        RandomFillPercent = randomFillPercent;
    }

    /*
    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SmoothMap();
        }
    }
    */

    public void GenerateMap()
    {
        _map = new int[Width, Height];
        RandomFillMap();

        for (int i = 0; i < InitialIterations; i++)
        {
            SmoothMap();
        }
    }

    public int[,] GetMap()
    {
        return _map;
    }

    private void RandomFillMap()
    {
        if (UseRandomSeed)
        {
            Seed = Time.time.ToString();
        }

        System.Random random = new System.Random(Seed.GetHashCode());

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                {
                    _map[x, y] = 1;
                }
                else
                {
                    _map[x, y] = (random.Next(0, 100) < RandomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    private void SmoothMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int neighborCount = GetNeighborWallCount(x, y);

                if (neighborCount > 4)
                {
                    _map[x, y] = 1;
                }
                else if (neighborCount < 4)
                {
                    _map[x, y] = 0;
                }
            }
        }
    }

    private int GetNeighborWallCount(int x, int y)
    {
        int count = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < Width && j >= 0 && j < Height)
                {
                    if (i != x || j != y)
                    {
                        count += _map[i, j];
                    }
                }
                else
                {
                    count++;
                }
            }
        }

        return count;
    }

    /*
    void OnDrawGizmos()
    {
        if (_map != null)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Gizmos.color = (_map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-Width / 2 + x + 0.5f, 0, -Height / 2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
    */
}
