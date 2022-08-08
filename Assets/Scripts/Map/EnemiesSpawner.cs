using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    private MapGenerator _mapGenerator;
    private Vector3 _positionToSpawn;

    public GameObject enemyPrefab;
    public int enemiesCounter;

    public void SpawnEnemies()
    {
        _mapGenerator = GetComponent<MapGenerator>();

        for (int i = 0; i < enemiesCounter; i++)
        {
            var pos = _mapGenerator.FindRandomOpenSpot().position;
            _positionToSpawn = new Vector3(pos.x + 0.5f, pos.y, pos.z + 0.5f);
            Instantiate(enemyPrefab, _positionToSpawn, Quaternion.identity);
        }
    }
}