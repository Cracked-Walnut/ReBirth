using UnityEngine;

/*
Purpose:
    The characteristics of the Protector's shield attribute.
Last Edited:
    01-26-23.
*/
public class ProtectorShield : MonoBehaviour {

    private float _shieldRadius;
    // [SerializeField] private bool _isShieldOn;
    // [SerializeField] private GameObject _shieldPoint;
    // [SerializeField] private LayerMask _whatIsEnemy;

    // void Update() => Shield();
    void Awake() =>_shieldRadius = GetComponent<CircleCollider2D>().radius;

    // private bool Shield() {
    //     if (_isShieldOn) {
    //         // Collider2D _theShield = Physics2D.OverlapCircle(_shieldPoint.transform.position, _shieldRadius, _whatIsEnemy);
    //         // if (_theShield) {
    //         //     // do stuff here
    //         //     return true;
    //         // }

    //     }
    //     return false;
    // }

    void OnTriggerEnter2D(Collider2D _collider2D) {
        if (_collider2D.gameObject.layer == 9 && _collider2D.gameObject.name != "Protector") {
            if (_collider2D.gameObject.GetComponent<EnemyLife>() != null) {
                _collider2D.gameObject.GetComponent<EnemyLife>().SetIsShielded(true);
                Debug.Log("Enemy in Shield");
            }
        }
    }

    void OnTriggerExit2D(Collider2D _collider2D) {
        if (_collider2D.gameObject.layer == 9 && _collider2D.gameObject.name != "Protector") {
            if (_collider2D.gameObject.GetComponent<EnemyLife>() != null) {
                _collider2D.gameObject.GetComponent<EnemyLife>().SetIsShielded(false);
                Debug.Log("Enemy out of Shield");
            }
        }  
    }
}
