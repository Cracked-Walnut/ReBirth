using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    To handle player attack logic.
Last Edited:
    12-28-22.
*/
public class PlayerAttack : MonoBehaviour {

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _attackPoint, _attackPoint2, _wallSlideAttackPoint, _wallSlideAttackPoint2;
    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private float _attackRange,
        _attackCoolDown; // implement this
    [SerializeField] private LayerMask _whatIsWall;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    // [SerializeField] bool _canAttack = true; // implement this
    
    
    // [SerializeField] DialogueTrigger _dialogueTrigger;

    // Functions
    void Update() {
        if (!_playerMovement.GetIsRolling()) { // prevent attack while rolling
        if (Input.GetMouseButtonDown(0)) {
            // Debug.Log("Pressed primary button.");
                // CheckBreakWall(); 
                // CheckDialogue();
                if(_characterController2D.GetFacingRight()) { // facing to the right
                    if (_playerMovement.GetIsWallSliding())
                        Instantiate(_bulletPrefab, _wallSlideAttackPoint.transform.position, _attackPoint2.transform.rotation);
                    else
                        Instantiate(_bulletPrefab, _attackPoint.transform.position, _attackPoint.transform.rotation);
                }
                else { // facing to the left
                    if (_playerMovement.GetIsWallSliding())
                        Instantiate(_bulletPrefab, _wallSlideAttackPoint.transform.position, _attackPoint.transform.rotation);
                    else
                        Instantiate(_bulletPrefab, _attackPoint.transform.position, _attackPoint2.transform.rotation);
                }
                // spawn the projectile
                // Instantiate(_bulletPrefab, _attackPoint.transform.position, _attackPoint.transform.rotation);
        }

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
        }
     }

    void CheckBreakWall() {
        Collider2D[] _hitWall = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _whatIsWall);

        foreach(Collider2D _wallsHit in _hitWall) {
            Destroy(_wallsHit.gameObject);
        }
    }

    // void CheckDialogue() {
    //     Collider2D[] _hitWall = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _whatIsWall);
    //     foreach(Collider2D _wallsHit in _hitWall) {
    //         _dialogueTrigger.TriggerDialogue();
    //     }
    // }

    public Transform GetAttackPoint() { return _attackPoint; }
    public float GetAttackRange() { return _attackRange; }
    
    // Stationary Attack
    // Moving Attack
}
