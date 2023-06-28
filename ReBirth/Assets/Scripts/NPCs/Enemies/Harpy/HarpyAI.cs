using UnityEngine;
using Pathfinding;
/*
Purpose:
    To handle flying enemy path finding.
Last Edited:
    06-27-23.
*/
public class HarpyAI : MonoBehaviour {

    [SerializeField] private Transform _target;

    [SerializeField] private float _speed = 200f;
    [SerializeField] private  float _nextWaypointDistance = 3f;

    Path _path;
    int _currentWaypoint = 0;
    bool _reachedEndOfPath = false;

    Seeker _seeker;
    Rigidbody2D _rigidbody2D;

    [SerializeField] private Transform _enemyGFX;

    void Start() {
        _seeker = GetComponent<Seeker>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        _seeker.StartPath(_rigidbody2D.position, _target.position, OnPathComplete);
    }

    void UpdatePath() {
        if (_seeker.IsDone())
            _seeker.StartPath(_rigidbody2D.position, _target.position, OnPathComplete);
    }

    void OnPathComplete(Path _p) {
        if(!_p.error) {
            _path = _p;
            _currentWaypoint = 0;
        }
    }

    void FixedUpdate() {
        if (_path == null)
            return;
        
        if (_currentWaypoint >= _path.vectorPath.Count) {
            _reachedEndOfPath = true;
            return;
        }
        else
            _reachedEndOfPath = false;

        Vector2 _direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody2D.position).normalized;
        Vector2 _force = _direction * _speed * Time.deltaTime;

        _rigidbody2D.AddForce(_force);

        float _distance = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWaypoint]);

        if (_distance < _nextWaypointDistance) {
            _currentWaypoint++;
        }

        if (_force.x >= 0.01f)
            _enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        else if (_force.x <= 0.01f)
            _enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
    }

}
