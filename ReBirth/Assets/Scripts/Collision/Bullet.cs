using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    The characteristics of a bullet
*/
/*
Purpose:
    The characteristics of a bullet.
Last Edited:
    12-25-22.
*/
public class Bullet : MonoBehaviour {

    // Variables
    [SerializeField] private float _bulletSpeed,
        _bulletDamage,
        _selfDestructTime;

    [SerializeField] private string _enemyTag,
        _playerTag;

    // [SerializeField] private GameObject _attackPoint; /*_hitEffect*/

    void Update() {
        transform.Translate(Vector2.right * _bulletSpeed * Time.deltaTime);
        CheckSelfDestructTime(_selfDestructTime);
    }

    void OnTriggerEnter2D(Collider2D _collider) {
        
        if (_collider.gameObject.tag == _enemyTag) {
            // _collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(_bulletDamage);
            // if (_collider.gameObject.GetComponent<EnemyHealth>().GetHP() <= 0) {
                // AudioManager.Instance.Play("kunai-hit");
                // PlayerInventoryManager.Instance.AddXP(_collider.gameObject.GetComponent<EnemyXP>().GetXP());
            // }
        }
        else if (_collider.gameObject.tag == _playerTag) {
            // HealthManager.Instance.TakeDamage(_bulletDamage);
        }
    Destroy(this.gameObject);

    }

    void CheckSelfDestructTime(float _seconds) {
        _selfDestructTime -= Time.deltaTime;
        if (_selfDestructTime <= 0.0f)
            Destroy(this.gameObject);
    }
}
