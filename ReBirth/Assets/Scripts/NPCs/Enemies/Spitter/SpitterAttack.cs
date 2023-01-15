using UnityEngine;

/*
Purpose:
    To handle attack logic for Ghoul.
    Grabs important info from Bullet.cs.
Last Edited:
    01-14-23.
*/
public class SpitterAttack : MonoBehaviour {

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private EnemyAnimation _enemyAnimation;
    private RaycastHit2D _spotPlayer;

    void Update() => CheckForPlayerInAttackRange();

    void CheckForPlayerInAttackRange() {
        if (transform.rotation.y == 0)
            _spotPlayer = Physics2D.Raycast(_attackPoint.position, Vector2.right, _attackRange, _whatIsPlayer);
        
        else if (transform.rotation.y == 180)
            _spotPlayer = Physics2D.Raycast(_attackPoint.position, -Vector2.right, _attackRange, _whatIsPlayer);

        if (_spotPlayer.collider != null) {
            _enemyAnimation.ResetAnimationTriggers();
            _enemyAnimation.GetAnimator().SetTrigger("Attack");
        }
    }

    public Transform GetAttackPoint() { return _attackPoint; }
    public float GetAttackRange() { return _attackRange; }
}
