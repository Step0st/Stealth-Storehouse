using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform _transformToFollow;
    private bool _isFollowing;
    private ColorChange _colorManager;
    private GameManager _gameManager;

    private void Start()
    {
        _colorManager = GetComponentInChildren<ColorChange>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _gameManager = (GameManager) FindObjectOfType(typeof(GameManager));
        _agent.speed = _gameManager.moveSpeed;
    }

    private void Update()
    {
        if (_isFollowing)
        {
            _agent.SetDestination(_transformToFollow.position);
        }

        if (_agent.velocity.magnitude >= 0.9f)
        {
            _animator.SetInteger("State", 1);
        }
        else
        {
            _animator.SetInteger("State", 0);
        }
    }

    public void FollowThePlayer(Transform playerPos)
    {
        _transformToFollow = playerPos;
        _isFollowing = true;
        _colorManager.ChangeColor();
    }
}