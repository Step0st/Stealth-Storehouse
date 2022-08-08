using UnityEngine;
using UnityEngine.UI;

public class EnemyHearing : MonoBehaviour
{
    private Slider _noiseSlider;
    private Enemy _enemy;
    private PlayerController _player;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _noiseSlider = (Slider) FindObjectOfType(typeof(Slider));
        _player = (PlayerController) FindObjectOfType(typeof(PlayerController));
    }

    private void Update()
    {
        if (_noiseSlider.value >= 100)
        {
            _enemy.FollowThePlayer(_player.transform);
        }
    }
}