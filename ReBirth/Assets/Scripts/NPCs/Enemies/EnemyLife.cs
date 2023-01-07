using UnityEngine;

/*
Purpose:
    To handle logic related to Ghoul life.
Last Edited:
    01-05-23.
*/
public class EnemyLife : MonoBehaviour {

    private int _life;
    [SerializeField] EnemyAnimation _enemyAnimation;
    [SerializeField] EnemyMovement _enemyMovement;

    void Start() {
        _life = 1;
    }

    void Update() {
        if (_life <= 0) {
            /*Play animation, destroy object*/
            _enemyMovement.SetMovementSpeed(0);
            _enemyAnimation.GetAnimator().SetTrigger("Dead");
        }
    }

    public void TakeDamage(int _damage) {
        _life -= _damage;
    }

    public void Dead() {
        Destroy(this.gameObject);
    }

    public int GetGhoulLife() { return _life; }
}
