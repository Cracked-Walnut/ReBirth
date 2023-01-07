using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    The characteristics of a bullet.
Last Edited:
    12-28-22.
*/
public class Bullet : MonoBehaviour {

    [SerializeField] private float _bulletSpeed,
        _selfDestructTime;
        
    [SerializeField] private int _bulletDamage;

    [SerializeField] private int _enemyLayer;

    [SerializeField] private GameObject _enemyHitEffect,
        _wallHitEffect;

    // [SerializeField] private GameObject _attackPoint; /*_hitEffect*/

    void Update() {
        transform.Translate(Vector2.right * _bulletSpeed * Time.deltaTime);
        CheckSelfDestructTime(_selfDestructTime);
    }

    void OnCollisionEnter2D(Collision2D _collision2D) {
        if (_collision2D.gameObject.layer == _enemyLayer) {
            // _collision2D.gameObject.GetComponent<EnemyHealth>().TakeDamage(_bulletDamage);
            // if (_collision2D.gameObject.GetComponent<EnemyHealth>().GetHP() <= 0) {
            //     AudioManager.Instance.Play("kunai-hit");
            //     PlayerInventoryManager.Instance.AddXP(_collision2D.gameObject.GetComponent<EnemyXP>().GetXP());
            // }
            _collision2D.gameObject.GetComponent<EnemyLife>().TakeDamage(_bulletDamage);
            Instantiate(_enemyHitEffect, this.transform.position, this.transform.rotation);
            // Instantiate(_bulletPrefab, _attackPoint.transform.position, _attackPoint.transform.rotation);
        } else {
            Instantiate(_wallHitEffect, this.transform.position, this.transform.rotation);
            // Instantiate(_bulletPrefab, _attackPoint.transform.position, _attackPoint.transform.rotation);
        }
        // else if (_collision2D.gameObject.tag == _playerTag) {
        //     HealthManager.Instance.TakeDamage(_bulletDamage);
        // }
        Destroy(this.gameObject);
    }

    void CheckSelfDestructTime(float _seconds) {
        _selfDestructTime -= Time.deltaTime;
        if (_selfDestructTime <= 0.0f)
            Destroy(this.gameObject);
    }
}
