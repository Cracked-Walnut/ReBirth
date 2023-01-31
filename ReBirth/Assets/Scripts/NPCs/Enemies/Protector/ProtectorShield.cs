using UnityEngine;

/*
Purpose:
    The characteristics of the Protector's shield attribute.
Last Edited:
    01-30-23.
*/
public class ProtectorShield : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D _collider2D) {
        if (_collider2D.gameObject.layer == 9 && _collider2D.gameObject.name != "Protector") {
            if (_collider2D.gameObject.GetComponent<EnemyLife>() != null) {
                
                _collider2D.gameObject.GetComponent<EnemyLife>().SetShieldAnim(true);
                _collider2D.gameObject.GetComponent<EnemyLife>().SetIsShielded(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D _collider2D) {
        if (_collider2D.gameObject.layer == 9 && _collider2D.gameObject.name != "Protector") {
            if (_collider2D.gameObject.GetComponent<EnemyLife>() != null) {
                
                _collider2D.gameObject.GetComponent<EnemyLife>().SetShieldAnim(false);
                _collider2D.gameObject.GetComponent<EnemyLife>().SetIsShielded(false);
            }
        }  
    }
}
