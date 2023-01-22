using UnityEngine;

/*
Purpose:
    To handle logic related to Ghoul life.
Last Edited:
    01-05-23.
*/
public class EnemyLife : MonoBehaviour {

    [SerializeField] private int _life;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] EnemyAnimation _enemyAnimation;
    [SerializeField] EnemyMovement _enemyMovement;
    // [SerializeField] private GameObject _player;
    private EnemyState _enemyState;

    void Start() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyState = GetComponent<EnemyState>();
    }

    void Update() {
        if (_life <= 0) {
            /*Play animation, destroy object*/
            _enemyMovement.SetMovementSpeed(0);
            _enemyState.SetState("Dead");
            _boxCollider2D.enabled = false;
        }
    }

    public void TakeDamage(int _damage) {
        _life -= _damage;

        if (_life != 0)
            _enemyState.SetState("Hit");
    }

    public void Dead() {
        // _player.GetComponent<MonsterTally>().IncrementMonsterTally(this.gameObject.tag);
        Destroy(this.gameObject);
    }

    public int GetGhoulLife() { return _life; }
}
