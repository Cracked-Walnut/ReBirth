using UnityEngine;

/*
Purpose:
    To hold sensitive data regarding player's possessions.
Last Edited:
    01-12-23.
*/
public class PlayerInventory : MonoBehaviour {

    [SerializeField] private int _totalCoins;

    public int GetTotalCoins() { return _totalCoins; }
    public void AddCoins(int _coinsToAdd) { _totalCoins += _coinsToAdd; }

}
