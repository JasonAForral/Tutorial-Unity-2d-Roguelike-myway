using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    [Serializable]
    public struct Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;

    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    public GameObject exit;

    public GameObject[]  floorTiles;
    public GameObject[]  wallTiles;
    public GameObject[]  foodTiles;
    public GameObject[]  enemyTiles;
    public GameObject[]  outerWallTiles;

    private Transform boardHolder;

    private List <Vector3> gridPositions = new List<Vector3>();

    private Quaternion flatRotation = Quaternion.Euler(90f, 0f, 0f);

    void InitialiseList ()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int z = 1; z < rows - 1; z++)
            {
                gridPositions.Add(new Vector3(x, 0f, z));
            }
        }
    }

    void BoardSetup ()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)
        {
            for (int z = -1; z < rows + 1; z++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || z == -1 || z == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, 0f, z), flatRotation) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    
    }

    Vector3 RandomPosition ()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            GameObject instance = Instantiate(tileChoice, randomPosition, flatRotation) as GameObject;
            instance.transform.SetParent(boardHolder);

        }
    }

    // Use this for initialization
    public void SetupScene (int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        GameObject instance = Instantiate(exit, new Vector3(columns - 1, 0f, rows - 1), flatRotation) as GameObject;
        instance.transform.SetParent(boardHolder);
	}
}
