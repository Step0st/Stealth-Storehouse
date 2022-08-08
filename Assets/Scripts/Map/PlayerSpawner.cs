using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private MapGenerator _mapGenerator;
    private Vector3 _positionToSpawn;
    public GameObject playerPrefab;

    public void SpawnPlayer()
    {
        _mapGenerator = GetComponent<MapGenerator>();
        var pos = _mapGenerator.enterTile.position;
        _positionToSpawn = new Vector3(pos.x + 0.5f, pos.y, pos.z + 0.5f);
        Instantiate(playerPrefab, _positionToSpawn, Quaternion.identity);
    }
}