using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private UIManager _uiManager;
    private PlayerSpawner _playerSpawner;
    private PlayerManager _playerManager;
    private Exit _exit;
    private MapGenerator _mapGenerator;
    private EnemiesSpawner _enemiesSpawner;
    private Enemy _enemy;
    private PlayerController _player;
    private NoiseController _noiseController;
    private StartWindow _startWindow;

    public int mapSize;
    public int enemiesCounter;
    public int moveSpeed;
    public float noiseIncValue;
    public float noiseDecValue;
    public float noiseMaxValue;
    public Action LoseGame;
    public Action WinGame;
    public Action MapSizeChanged;

    private void Start()
    {
        _uiManager = GetComponent<UIManager>();
        _playerSpawner = GetComponent<PlayerSpawner>();
        _enemiesSpawner = GetComponent<EnemiesSpawner>();
        _mapGenerator = (MapGenerator) FindObjectOfType(typeof(MapGenerator));
        _startWindow = (StartWindow) FindObjectOfType(typeof(StartWindow));


        //default values
        mapSize = 7;
        enemiesCounter = 2;
        moveSpeed = 3;
        noiseIncValue = 3f;
        noiseDecValue = 2f;
        noiseMaxValue = 10f;

        _uiManager.startWindow.NewGameEvent += () =>
        {
            _playerSpawner.SpawnPlayer();
            _enemiesSpawner.SpawnEnemies();
            _playerManager = (PlayerManager) FindObjectOfType(typeof(PlayerManager));
            _enemy = (Enemy) FindObjectOfType(typeof(Enemy));
            _player = (PlayerController) FindObjectOfType(typeof(PlayerController));

            _playerManager.PlayerCatch += () => { LoseGame?.Invoke(); };

            _exit = (Exit) FindObjectOfType(typeof(Exit));
            _exit.PlayerEscape += () => { WinGame?.Invoke(); };
        };

        _startWindow.InsertMapSize += () =>
        {
            mapSize = _startWindow.mapSize;
            MapSizeChanged?.Invoke();
        };

        _startWindow.InsertEnemiesCounter += () => { _enemiesSpawner.enemiesCounter = _startWindow.enemiesCounter; };

        _startWindow.InsertMoveSpeed += () => { moveSpeed = _startWindow.moveSpeed; };

        _startWindow.InsertNoiseIncrease += () => { noiseIncValue = _startWindow.noiseIncValue; };

        _startWindow.InsertNoiseDecrease += () => { noiseDecValue = _startWindow.noiseDecValue; };

        _startWindow.InsertNoiseMax += () => { noiseMaxValue = _startWindow.noiseMaxValue; };
    }

    public void GenerateNewMap()
    {
        Vector2 newMapSize = new Vector2(mapSize, mapSize);
        _mapGenerator.mapSize = newMapSize;
        int newSeed = Random.Range(-100, 100);
        _mapGenerator.seed = newSeed;
        _mapGenerator.GenerateMap();
    }
}