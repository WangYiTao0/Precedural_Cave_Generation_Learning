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
    int[,] map;

    private void Start()
    {
        GenerateMap();
        RandomFillMap();
    }

    private void GenerateMap()
    {
        map = new int[width, height];
    }

    void RandomFillMap()
    {
        if(useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random psuedoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = psuedoRandom.Next(0, 100) < randomFillPercent ? 1 : 0;
            }
        }
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
