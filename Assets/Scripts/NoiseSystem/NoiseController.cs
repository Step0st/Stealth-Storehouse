using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NoiseController : MonoBehaviour
{
    private Slider _noiseSlider;
    private PlayerController _playerController;
    private GameManager _gameManager;
    public float noiseIncValue;
    public float noiseDecValue;
    public float noiseMaxValue;


    void Start()
    {
        _noiseSlider = (Slider) FindObjectOfType(typeof(Slider));
        _gameManager = (GameManager) FindObjectOfType(typeof(GameManager));
        _playerController = GetComponent<PlayerController>();
        _noiseSlider.value = 0;

        noiseIncValue = _gameManager.noiseIncValue;
        noiseDecValue = _gameManager.noiseDecValue;
        noiseMaxValue = _gameManager.noiseMaxValue;

        StartCoroutine(NoiseRising());
    }

    public IEnumerator NoiseRising()
    {
        while (_noiseSlider.value <= noiseMaxValue * 10)
        {
            if (_playerController.isMoving)
            {
                _noiseSlider.value += noiseIncValue;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                _noiseSlider.value -= noiseDecValue;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}