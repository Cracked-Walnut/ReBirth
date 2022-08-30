using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void LateUpdate() {


        if (_characterController2D.GetGrounded()) {
            _spriteRenderer.flipX = false;
            
            _animator.SetBool("IsGrounded", _characterController2D.GetGrounded());

            if (_playerMovement.GetHorizontalMove() == 0) {
                if (_playerMovement.GetCrouch()) {
                    ResetTriggers();
                    _animator.SetTrigger("Crouch");
                }
                else {
                    ResetTriggers();
                    _animator.SetTrigger("Idle");
                }
            }
            else if (_playerMovement.GetHorizontalMove() != 0) {
                if (_playerMovement.GetCrouch()) {
                    ResetTriggers();
                    _animator.SetTrigger("Crouch_Walk");
                }
                else {
                    ResetTriggers();
                    _animator.SetTrigger("Run");
                }
            }
        }
        else {
            _animator.SetBool("IsGrounded", _characterController2D.GetGrounded());

            _animator.SetBool("IsTouchingWallTop", _playerMovement.GetIsTouchingWallTop());
            _animator.SetBool("IsTouchingWallBottom", _playerMovement.GetIsTouchingWallBottom());

            if (_rigidBody2D.velocity.y > 0) {
                ResetTriggers();
                _animator.SetTrigger("Jump_Up");
            }
            else if (_rigidBody2D.velocity.y < 0) {
                ResetTriggers();
                _animator.SetTrigger("Jump_Down");
            }

            if (_playerMovement.GetIsTouchingWallTop() && _playerMovement.GetIsTouchingWallBottom() && !_characterController2D.GetGrounded()) {
                
                // Wall Jump
                if (_rigidBody2D.velocity.y > 5f) {
                    ResetTriggers();
                    _spriteRenderer.flipX = false;
                    _animator.SetTrigger("Jump_Up");
                }
                else {
                    ResetTriggers();
                    _spriteRenderer.flipX = true;
                    _animator.SetTrigger("Wall_Slide");
                }
            }
            else if (!_playerMovement.GetIsTouchingWallTop() && !_playerMovement.GetIsTouchingWallBottom())
                _spriteRenderer.flipX = false;
        }
    }

    void ResetTriggers() {
         string[] _triggers = {"Idle", "Crouch", "Crouch_Walk", "Run", "Jump_Up", "Jump_Down", "Wall_Slide", "Stop_Running"};
    
        for (int i = 0; i < _triggers.Length - 1; i++) {
            _animator.ResetTrigger(_triggers[i]);
        }
    }
}
