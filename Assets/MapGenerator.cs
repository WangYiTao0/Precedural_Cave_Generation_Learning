using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int width = 60;
    [SerializeField] int height = 80;
    [Range(1,100)]
    [SerializeField] int randomFillPercent = 45;
    [SerializeField] string seed;
    [SerializeField] bool useRandomSeed;
    [SerializeField] int smoothmap = 5;
    int[,] map; //1 wall 0 empty

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for(int i = 0; i< smoothmap; i++)
        {
            SmoothMap();
        }
    }

    void RandomFillMap()
    {
        if(useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        //System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        UnityEngine.Random.InitState(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    //map[x, y] = pseudoRandom.Next(0, 100) < randomFillPercent ? 1 : 0;
                    map[x, y] = UnityEngine.Random.Range(0, 100) < randomFillPercent ? 1 : 0;

                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                
                if(neighbourWallTiles > 4)// if has over 4 wall
                {
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles < 4)  // if has over 4 empty
                {
                    map[x, y] = 0;
                }
            
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

         //loop 3x3
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)   
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)   //  check edge
                {
                    if (neighbourX != gridX || neighbourY != gridY)  // check
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    private void OnDrawGizmos()
    {
        if(map!=null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2f + x + 0.5f, 0, -height / 2f + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
