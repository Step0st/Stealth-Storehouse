using System;
using UnityEngine;

public class StartWindow : MonoBehaviour
{
    public Action NewGameEvent;

    // private MapGenerator _mapGenerator;
    // private EnemiesSpawner _enemiesSpawner;
    // private Enemy _enemy;
    // private PlayerController _player;
    // private NoiseController _noiseController;
    private GameManager _gameManager;

    public Action InsertMapSize;
    public Action InsertEnemiesCounter;
    public Action InsertMoveSpeed;
    public Action InsertNoiseIncrease;
    public Action InsertNoiseDecrease;
    public Action InsertNoiseMax;
    public Action QuitEvent;

    public int mapSize;
    public int enemiesCounter;
    public int moveSpeed;
    public float noiseIncValue;
    public float noiseDecValue;
    public float noiseMaxValue;


    private void Start()
    {
        _gameManager = (GameManager) FindObjectOfType(typeof(GameManager));
    }

    public void OnNewGame()
    {
        NewGameEvent?.Invoke();
    }

    public void GenerateMap()
    {
        _gameManager.GenerateNewMap();
    }

    public void ReadMapSize(string s)
    {
        mapSize = int.Parse(s);
        InsertMapSize?.Invoke();
    }

    public void ReadEnemiesCounter(string s)
    {
        enemiesCounter = int.Parse(s);
        InsertEnemiesCounter?.Invoke();
    }

    public void ReadMoveSpeed(string s)
    {
        moveSpeed = int.Parse(s);
        InsertMoveSpeed?.Invoke();
    }

    public void ReadNoiseIncValue(string s)
    {
        noiseIncValue = int.Parse(s);
        InsertNoiseIncrease?.Invoke();
    }

    public void ReadNoiseDecValue(string s)
    {
        noiseDecValue = int.Parse(s);
        InsertNoiseDecrease?.Invoke();
    }

    public void ReadNoiseMaxValue(string s)
    {
        noiseMaxValue = int.Parse(s);
        InsertNoiseMax?.Invoke();
    }

    public void OnGameQuit()
    {
        QuitEvent?.Invoke();
    }
}