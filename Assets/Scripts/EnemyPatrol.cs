using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    //[SerializeField] private bool _patrolWaiting;
    [SerializeField] private float _totalWaitTime = 2f;
    [SerializeField] private List<Vector3> _patrolPoints;

    private NavMeshAgent _navMeshAgent;
    private int _currentPatrolIndex;
    private bool _traveling;
    private bool _waiting;
    private bool _patrolForward;
    private float _waitTimer;
    private MapGenerator _map;
    private Vector3 _positionToPatrol;

    private void Start()
    {
        _map = (MapGenerator) FindObjectOfType(typeof(MapGenerator));
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _patrolPoints.Add(transform.position);

        var pos = _map.FindRandomOpenSpot().position;
        _positionToPatrol = new Vector3(pos.x + 0.5f, pos.y, pos.z + 0.5f);
        _patrolPoints.Add(_positionToPatrol);

        if (_patrolPoints != null && _patrolPoints.Count >= 2)
        {
            _currentPatrolIndex = 0;
            SetDestination();
        }
    }

    private void Update()
    {
        if (_traveling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _traveling = false;
            _waiting = true;
            _waitTimer = 0f;
        }

        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalWaitTime)
            {
                _waiting = false;
                SetDestination();
                ChangePatrolPoint();
            }
        }
    }

    private void SetDestination()
    {
        Vector3 targetVector = _patrolPoints[_currentPatrolIndex];
        _navMeshAgent.SetDestination(targetVector);
        _traveling = true;
    }

    private void ChangePatrolPoint()
    {
        if (_currentPatrolIndex == 0)
        {
            _currentPatrolIndex++;
        }
        else
        {
            _currentPatrolIndex--;
        }
    }
}