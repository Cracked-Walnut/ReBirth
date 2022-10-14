using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    To handle player attack logic.
Last Edited:
    10-13-22.
*/
public class PlayerAttack : MonoBehaviour {

    // TODOs

    // Variables
    /*
    List/array to hold attack animations
    CanAttack
    Attack Cool Down
    */
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _whatIsWall;

    // Functions
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Pressed primary button.");
            if (!_playerMovement.GetIsRolling()) // prevent attack while rolling
                CheckBreakWall(); 
        }

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");

     }

     void CheckBreakWall() {
        Collider2D[] _hitWall = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _whatIsWall);

        foreach(Collider2D _wallsHit in _hitWall) {
            Destroy(_wallsHit.gameObject);
        }
     }
    
    // Stationary Attack
    // Moving Attack
}
