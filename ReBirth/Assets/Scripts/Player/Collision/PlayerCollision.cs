using UnityEngine;

/*
Purpose:
    The way collision data is handled for the player.
Last Edited:
    01-12-23.
*/
public class PlayerCollision : MonoBehaviour {

    [SerializeField] private int _coinLayer;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerMovement _playerMovement;

    void OnTriggerEnter2D(Collider2D _collider2D) {
        if (_collider2D.gameObject.layer == _coinLayer) {

            int _coinValue = _collider2D.gameObject.GetComponent<CoinValue>().GetValue();
            _playerInventory.AddCoins(_coinValue);
            Debug.Log("Total Coins: " + _playerInventory.GetTotalCoins());
            Destroy(_collider2D.gameObject);
            
        }
        // else if (_playerMovement.GetIsRolling() && _collider2D.gameObject.layer == 9) {
        //     Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), _collider2D.gameObject.GetComponent<BoxCollider2D>(), true);
        // }
    }

}
