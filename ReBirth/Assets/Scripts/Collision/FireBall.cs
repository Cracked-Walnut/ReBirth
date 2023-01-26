using UnityEngine;

/*
Purpose:
    The characteristics of a FireBall projectile.
Last Edited:
    01-25-23.
*/
public class FireBall : MonoBehaviour {

    [SerializeField] private Bullet _bullet;

    // This Bullet-like object's goal is to be interacting with game objects on the enemy layer
    void OnCollisionEnter2D(Collision2D _collision2D) {
        
        // Primary interaction with Enemy Layer
        if (_collision2D.gameObject.layer == _bullet.GetPlayerLayer()) {
            // _collision2D.gameObject.GetComponent<EnemyLife>().TakeDamage(_bullet.GetBulletDamage());
            Instantiate(_bullet.GetEnemyHitEffect(), this.transform.position, this.transform.rotation);
        
        // Secondary interaction with Wall Layer
        } else if (_collision2D.gameObject.layer == _bullet.GetWallLayer()) {

            _collision2D.gameObject.GetComponent<BreakableWallLife>().TakeDamage(_bullet.GetBulletDamage());
            Instantiate(_bullet.GetWallHitEffect(), this.transform.position, this.transform.rotation);
            
        // Anything else
        } else {
            Instantiate(_bullet.GetWallHitEffect(), this.transform.position, this.transform.rotation);
        }
        Destroy(this.gameObject);
    }
}
