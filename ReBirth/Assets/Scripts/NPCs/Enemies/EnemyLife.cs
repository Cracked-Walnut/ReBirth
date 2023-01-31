using UnityEngine;

/*
Purpose:
    To handle logic related to Ghoul life.
Last Edited:
    01-30-23.
*/
public class EnemyLife : MonoBehaviour {

    [SerializeField] private int _life;
    [SerializeField] private bool _isShielded;
    private BoxCollider2D _boxCollider2D;
    private ShieldAnimation _shieldAnimation;
    [SerializeField] EnemyAnimation _enemyAnimation;
    [SerializeField] EnemyMovement _enemyMovement;
    // [SerializeField] private GameObject _player;

    void Start() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _shieldAnimation = GetComponent<ShieldAnimation>();
    }

    void Update() {
        if (_life <= 0) {
            /*Play animation, destroy object*/
            _enemyMovement.SetMovementSpeed(0);
            _enemyAnimation.GetAnimator().SetTrigger("Dead");
            _boxCollider2D.enabled = false;
        }
    }

    public void TakeDamage(int _damage) {
        _life -= _damage;

        if (_life != 0)
            _enemyAnimation.GetAnimator().SetTrigger("Hit");
    }

    // void IncrementMonsterTally() => _player.GetComponent<MonsterTally>().IncrementMonsterTally(this.gameObject.tag);
    
    public void Dead() => Destroy(this.gameObject);

    public int GetGhoulLife() { return _life; }
    public bool GetIsShielded() { return _isShielded; }

    public void SetShieldAnim(bool _isShieldAnimOn) {
        if (_isShieldAnimOn)
            _shieldAnimation.GetAnimator().SetTrigger("ShieldOn");
        else
            _shieldAnimation.GetAnimator().SetTrigger("ShieldOff");
    }

    public void SetIsShielded(bool _isShielded) => this._isShielded = _isShielded;
}
