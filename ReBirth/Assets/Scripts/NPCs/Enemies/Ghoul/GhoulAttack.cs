using UnityEngine;

/*
Purpose:
    To handle attack logic for Ghoul.
    Grabs important info from Bullet.cs.
Last Edited:
    01-13-23.
*/
public class GhoulAttack : MonoBehaviour {

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private EnemyAnimation _enemyAnimation;

    void Update() {
        CheckForPlayerInAttackRange();
    }

    void CheckForPlayerInAttackRange() {
        Collider2D[] _players = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _whatIsPlayer);

        foreach (Collider2D _player in _players) {
            _enemyAnimation.ResetAnimationTriggers();
            _enemyAnimation.GetAnimator().SetTrigger("Attack");
        }
    }

    public Transform GetAttackPoint() { return _attackPoint; }
    public float GetAttackRange() { return _attackRange; }
}
