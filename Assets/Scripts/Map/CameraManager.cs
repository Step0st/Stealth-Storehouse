using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameManager _gameManager;
    public float offset=0f;

    private void Start()
    {
        _gameManager = (GameManager) FindObjectOfType(typeof(GameManager));
        var initPos = transform.position;
        _gameManager.MapSizeChanged += () =>
        {
            if (_gameManager.mapSize >12)
            {
                offset = _gameManager.mapSize / 1.5f;
                var position = transform.position;
                position = new Vector3(initPos.x, initPos.y+offset,initPos.z);
                transform.position = position;
            }
            else
            {
                transform.position = initPos;
            }
        };
    }
}
