using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    To handle logic for enemy spawning.
Last Edited:
    01-21-23.
*/
public class EnemySpawn : MonoBehaviour {

    [SerializeField] private EnemyAnimation _enemyAnimation;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Transform _sightPoint;
    [SerializeField] private float _sightRadius;
    [SerializeField] private EnemyState _enemyState;
    [SerializeField] private bool _isAggroed;

    void Update() => FindPlayer();

    void Awake() {
        _isAggroed = false;
        _enemyState = GetComponent<EnemyState>();
    }
    bool FindPlayer() {
        Collider2D _playerSeen = Physics2D.OverlapCircle(_sightPoint.transform.position, _sightRadius, _whatIsPlayer);
        
        // foreach(Collider2D _playerSeen in _playersSeen) {
        if (_playerSeen && !_isAggroed) {
            _isAggroed = true;
            StartCoroutine(Agro());
            return true;
        }
        return false;
    }

    private IEnumerator Agro() {
        // _enemyState.SetState("Spawn");
        _enemyAnimation.GetAnimator().SetTrigger("Spawn");
        yield return new WaitForSeconds(.67f);
        // _enemyState.SetState("Walking");
        _enemyAnimation.GetAnimator().SetTrigger("Walking");
    }

    public Transform GetSightPoint() { return _sightPoint; }
    public float GetSightRadius() { return _sightRadius; }

}
