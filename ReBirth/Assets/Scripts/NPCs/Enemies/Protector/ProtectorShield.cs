using UnityEngine;

/*
Purpose:
    The characteristics of the Protector's shield attribute.
Last Edited:
    01-28-23.
*/
public class ProtectorShield : MonoBehaviour {

    // [SerializeField] private float _shieldRadius;
    // [SerializeField] private bool _isShieldOn;
    // [SerializeField] private GameObject _shieldPoint;
    // [SerializeField] private LayerMask _whatIsEnemy;

    // void Update() => Shield();
    // void Awake() =>_shieldRadius = GetComponent<CircleCollider2D>().radius;

    // private bool Shield() {
    //     if (_isShieldOn) {
    //         Collider2D[] _enemiesShielded = Physics2D.OverlapCircleAll(_shieldPoint.transform.position, _shieldRadius, _whatIsEnemy);
    //         if (_enemiesShielded) {
    //             foreach (Collider2D _enemyShielded in _enemiesShielded) {
    //                 _enemyShielded.GetComponent<EnemyLife>().SetIsShielded(true);
    //                 Debug.Log(_enemyShielded.name + " is shielded");
    //             }
    //             return true;
    //         }

    //     }
    //     return false;
    // }

    void OnTriggerEnter2D(Collider2D _collider2D) {
        if (_collider2D.gameObject.layer == 9 && _collider2D.gameObject.name != "Protector") {
            if (_collider2D.gameObject.GetComponent<EnemyLife>() != null) {
                // set shield on
                _collider2D.gameObject.GetComponent<EnemyLife>().ShieldAnimOn();
                _collider2D.gameObject.GetComponent<EnemyLife>().SetIsShielded(true);
                
                Debug.Log("On");
            }
        }
    }

    void OnTriggerExit2D(Collider2D _collider2D) {
        if (_collider2D.gameObject.layer == 9 && _collider2D.gameObject.name != "Protector") {
            if (_collider2D.gameObject.GetComponent<EnemyLife>() != null) {
                // set shield off
                _collider2D.gameObject.GetComponent<EnemyLife>().ShieldAnimOff();
                _collider2D.gameObject.GetComponent<EnemyLife>().SetIsShielded(false);
                
                Debug.Log("Off");
            }
        }  
    }


}
