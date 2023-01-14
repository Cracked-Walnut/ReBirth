using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    The characteristics of a bullet.
Last Edited:
    01-13-23.
*/
public class Bullet : MonoBehaviour {

    [SerializeField] private float _bulletSpeed,
        _selfDestructTime;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private int _enemyLayer,
        _wallLayer,
        _playerLayer;
    [SerializeField] private GameObject _enemyHitEffect,
        _wallHitEffect;

    void Update() {
        transform.Translate(Vector2.right * _bulletSpeed * Time.deltaTime);
        CheckSelfDestructTime(_selfDestructTime);
    }

    void CheckSelfDestructTime(float _seconds) {
        _selfDestructTime -= Time.deltaTime;
        if (_selfDestructTime <= 0.0f)
            Destroy(this.gameObject);
    }

    public int GetBulletDamage() { return _bulletDamage; }
    public int GetWallLayer() { return _wallLayer; }
    public int GetEnemyLayer() { return _enemyLayer; }
    public int GetPlayerLayer() { return _playerLayer; }
    public float GetBulletSpeed() { return _bulletSpeed; }
    public float GetSelfDestructTime() { return _selfDestructTime; }
    public GameObject GetEnemyHitEffect() { return _enemyHitEffect; }
    public GameObject GetWallHitEffect() { return _wallHitEffect; }

}

    // [SerializeField] private GameObject _attackPoint; /*_hitEffect*/

    // void OnCollisionEnter2D(Collision2D _collision2D) {
        
    //     if (_collision2D.gameObject.layer == _enemyLayer) {
    //         // _collision2D.gameObject.GetComponent<EnemyHealth>().TakeDamage(_bulletDamage);
    //         // if (_collision2D.gameObject.GetComponent<EnemyHealth>().GetHP() <= 0) {
    //         //     AudioManager.Instance.Play("kunai-hit");
    //         //     PlayerInventoryManager.Instance.AddXP(_collision2D.gameObject.GetComponent<EnemyXP>().GetXP());
    //         // }
    //         _collision2D.gameObject.GetComponent<EnemyLife>().TakeDamage(_bulletDamage);
    //         Instantiate(_enemyHitEffect, this.transform.position, this.transform.rotation);
    //         // Instantiate(_bulletPrefab, _attackPoint.transform.position, _attackPoint.transform.rotation);
        
    //     } else if (_collision2D.gameObject.layer == _wallLayer) {

    //         _collision2D.gameObject.GetComponent<BreakableWallLife>().TakeDamage(_bulletDamage);
    //         Instantiate(_enemyHitEffect, this.transform.position, this.transform.rotation);
            
    //     } else {
    //         Instantiate(_wallHitEffect, this.transform.position, this.transform.rotation);
    //         // Instantiate(_bulletPrefab, _attackPoint.transform.position, _attackPoint.transform.rotation);
    //     }
    //     // else if (_collision2D.gameObject.tag == _playerTag) {
    //     //     HealthManager.Instance.TakeDamage(_bulletDamage);
    //     // }
    //     Destroy(this.gameObject);
    // }

