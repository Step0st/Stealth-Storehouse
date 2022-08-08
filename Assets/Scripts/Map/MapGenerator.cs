using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform navMeshMaskPrefab;
    public Vector2 mapSize;
    public Vector2 maxMapSize;
    public int seed = 10;
    [HideInInspector] public Transform enterTile;
    [Range(0, 1)] public float obstaclePercent;

    private List<Coord> _allTileCoords;
    private Queue<Coord> _shuffleTileCoords;
    private Transform[,] _transforms;
    private List<Coord> _allOpenCoords;
    private Coord _mapCenter;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        //currentMap = maps
        _transforms = new Transform[(int) mapSize.x, (int) mapSize.y];

        //Generating coords
        _allTileCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                _allTileCoords.Add(new Coord(x, y));
            }
        }

        _shuffleTileCoords = new Queue<Coord>(Utility.ShuffleArray(_allTileCoords.ToArray(), seed));
        _mapCenter = new Coord((int) (mapSize.x / 2), (int) (mapSize.y / 2));

        //Creating map holder obj
        string holderName = "GeneratedMap";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        //Spawning tiles
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right)) as Transform;
                newTile.parent = mapHolder;
                _transforms[x, y] = newTile;
            }
        }

        //spawning obstacles
        bool[,] obstacleMap = new bool[(int) mapSize.x, (int) mapSize.y];

        int obstacleCount = (int) (mapSize.x * mapSize.y * obstaclePercent);
        int currentObstacleCount = 0;
        _allOpenCoords = new List<Coord>(_allTileCoords);

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (randomCoord != _mapCenter && MapIsFullyAccesible(obstacleMap, currentObstacleCount))
            {
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x + 0.5f, randomCoord.y + 0.5f);
                Transform newObstacle =
                    Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;

                _allOpenCoords.Remove(randomCoord);
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }

        enterTile = FindPlaceForEnter(_allOpenCoords);
        enterTile.gameObject.GetComponent<Renderer>().material.color = new Color(0, 128, 0);

        var exitTile = FindPlaceForExit(_allOpenCoords);
        exitTile.gameObject.GetComponent<Renderer>().material.color = new Color(128, 0, 0);
        exitTile.gameObject.AddComponent(typeof(Exit));

        BuildBoundaries(mapHolder);
    }


    bool MapIsFullyAccesible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(_mapCenter);
        mapFlags[_mapCenter.x, _mapCenter.y] = true;

        int accesibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 &&
                            neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accesibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAcccesibleTileCount = (int) (mapSize.x * mapSize.y - currentObstacleCount);
        return targetAcccesibleTileCount == accesibleTileCount;
    }

    Vector3 CoordToPosition(float x, float y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }

    private Transform FindPlaceForEnter(List<Coord> allOpenCoords)
    {
        var enterTile = allOpenCoords.First();
        var minx = enterTile.x;
        var miny = enterTile.y;
        return _transforms[minx, miny];
    }

    private Transform FindPlaceForExit(List<Coord> allOpenCoords)
    {
        var exitTile = allOpenCoords.Last();
        var maxx = exitTile.x;
        var maxy = exitTile.y;
        return _transforms[maxx, maxy];
    }

    public Transform FindRandomOpenSpot()
    {
        var maxTile = _allOpenCoords.Last();
        int spawnDistanceX = (int) ((int) mapSize.x * 0.75f); //distance from exit for enemies to spawn
        int spawnDistanceY = (int) ((int) mapSize.y * 0.75f);
        Coord checkCoord = new Coord(maxTile.x - Random.Range(1, spawnDistanceX),
            maxTile.y - Random.Range(1, spawnDistanceY));
        while (_allOpenCoords.Contains(checkCoord) == false)
        {
            checkCoord = new Coord(maxTile.x - Random.Range(1, spawnDistanceX),
                maxTile.y - Random.Range(1, spawnDistanceY));
        }

        return _transforms[checkCoord.x, checkCoord.y];
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = _shuffleTileCoords.Dequeue();
        _shuffleTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coord c1, Coord c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }
    }

    public void BuildBoundaries(Transform mapHolder)
    {
        Vector3 leftMaskPos = Vector3.left * (mapSize.x + maxMapSize.x) / 4;
        leftMaskPos = new Vector3(leftMaskPos.x + 0.5f, leftMaskPos.y, leftMaskPos.z + 0.5f);
        Transform maskLeft = Instantiate(navMeshMaskPrefab, leftMaskPos, Quaternion.identity) as Transform;
        maskLeft.parent = mapHolder;
        maskLeft.localScale = new Vector3((maxMapSize.x - mapSize.x) / 2, 1, mapSize.y);

        Vector3 rightMaskPos = Vector3.right * (mapSize.x + maxMapSize.x) / 4;
        rightMaskPos = new Vector3(rightMaskPos.x + 0.5f, rightMaskPos.y, rightMaskPos.z + 0.5f);
        Transform maskRight = Instantiate(navMeshMaskPrefab, rightMaskPos, Quaternion.identity) as Transform;
        maskRight.parent = mapHolder;
        maskRight.localScale = new Vector3((maxMapSize.x - mapSize.x) / 2, 1, mapSize.y);

        Vector3 topMaskPos = Vector3.forward * (mapSize.y + maxMapSize.y) / 4;
        topMaskPos = new Vector3(topMaskPos.x + 0.5f, topMaskPos.y, topMaskPos.z + 0.5f);
        Transform maskTop = Instantiate(navMeshMaskPrefab, topMaskPos, Quaternion.identity) as Transform;
        maskTop.parent = mapHolder;
        maskTop.localScale = new Vector3(mapSize.x, 1, (maxMapSize.y - mapSize.y) / 2);

        Vector3 botMaskPos = Vector3.back * (mapSize.y + maxMapSize.y) / 4;
        botMaskPos = new Vector3(botMaskPos.x + 0.5f, botMaskPos.y, botMaskPos.z + 0.5f);
        Transform maskBottom = Instantiate(navMeshMaskPrefab, botMaskPos, Quaternion.identity) as Transform;
        maskBottom.parent = mapHolder;
        maskBottom.localScale = new Vector3(mapSize.x, 1, (maxMapSize.y - mapSize.y) / 2);
    }
}