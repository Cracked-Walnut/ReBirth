using UnityEngine;

/*
Purpose:
    To handle attack logic for Ghoul.
    Grabs important info from Bullet.cs.
Last Edited:
    01-25-23.
*/
public class SpitterAttack : MonoBehaviour {

    [SerializeField] private Transform _attackPoint, _sightPoint;
    [SerializeField] private GameObject _fireBallPrefab;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private EnemyAnimation _enemyAnimation;
    private RaycastHit2D _spotPlayer;

    void Update() => CheckForPlayerInAttackRange();

    void CheckForPlayerInAttackRange() {
        if (transform.eulerAngles == new Vector3(0f, 0f, 0f))
            _spotPlayer = Physics2D.Raycast(_sightPoint.position, Vector2.right, _attackRange, _whatIsPlayer);
        
        if (transform.eulerAngles == new Vector3(0f, 180f, 0f))
            _spotPlayer = Physics2D.Raycast(_sightPoint.position, -Vector2.right, _attackRange, _whatIsPlayer);

        if (_spotPlayer.collider != null) {
            // _enemyAnimation.ResetAnimationTriggers();
            _enemyAnimation.GetAnimator().SetTrigger("Attack");
        }
    }

    public void FireBall() {
        Instantiate(_fireBallPrefab, _attackPoint.transform.position, _attackPoint.transform.rotation);
    }

    public Transform GetAttackPoint() { return _attackPoint; }
    public float GetAttackRange() { return _attackRange; }
}
