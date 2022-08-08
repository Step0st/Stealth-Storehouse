using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private GameManager _gameManager;
    
    private float _playerSpeed;
    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    private Animator _animator;
    [HideInInspector]
    public bool isMoving;
    
    

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _gameManager = (GameManager) FindObjectOfType(typeof(GameManager));
        _playerSpeed = _gameManager.moveSpeed;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        
       if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);
            _animator.SetInteger("State", 1);
            _controller.Move(moveDirection * _playerSpeed * Time.deltaTime);
            isMoving = true;
        }
        else
        {
            _animator.SetInteger("State", 0);
            isMoving = false;
        }
    }
}