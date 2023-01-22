using UnityEngine;

/*
Purpose:
    To handle logic for spawning.
Last Edited:
    01-21-23.
*/
public class EnemySpawn : MonoBehaviour {

    [SerializeField] private EnemyAnimation _enemyAnimation;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Transform _sightPoint;
    [SerializeField] private float _sightRadius;
    [SerializeField] private EnemyState _enemyState;

    void Update() => FindPlayer();

    void Awake() => _enemyState = GetComponent<EnemyState>();

    bool FindPlayer() {
        Collider2D _playerSeen = Physics2D.OverlapCircle(_sightPoint.transform.position, _sightRadius, _whatIsPlayer);
        
        // foreach(Collider2D _playerSeen in _playersSeen) {
        if (_playerSeen) {
            _enemyState.SetState("Spawn");
            return true;
        }
        return false;
    }

    public Transform GetSightPoint() { return _sightPoint; }
    public float GetSightRadius() { return _sightRadius; }

}
