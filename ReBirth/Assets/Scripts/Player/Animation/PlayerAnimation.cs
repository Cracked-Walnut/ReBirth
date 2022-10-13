using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    To handle animation related logic for the player model.
Last Edited:
    10-10-22.
*/
public class PlayerAnimation : MonoBehaviour {

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void LateUpdate() {

        // flip player when they've touched the ground
        if (_characterController2D.GetGrounded()) {
            _spriteRenderer.flipX = false;
            
            _animator.SetBool("IsGrounded", _characterController2D.GetGrounded());

            // set animation
            if (_playerMovement.GetHorizontalMove() == 0) {
                if (_playerMovement.GetCrouch()) {
                    ResetTriggers();
                    _animator.SetTrigger("Crouch");
                }
                // else if (_playerMovement.GetRoll()) {
                //     ResetTriggers();
                //     _animator.SetTrigger("Roll");
                // }
                else if (!_playerMovement.GetCrouch()) {
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
                    if (_playerMovement.GetRoll()) {
                        ResetTriggers();
                        _animator.SetTrigger("Roll");
                    }
                    ResetTriggers();
                    _animator.SetTrigger("Run");
                }
            }
        }
        // if the player isn't grounded
        else {
            _animator.SetBool("IsGrounded", _characterController2D.GetGrounded());
            _animator.SetBool("IsTouchingWallTop", _playerMovement.GetIsTouchingWallTop());
            _animator.SetBool("IsTouchingWallBottom", _playerMovement.GetIsTouchingWallBottom());

            // handle jump animation while airborne
            if (_rigidBody2D.velocity.y > 0) {
                ResetTriggers();
                _animator.SetTrigger("Jump_Up");
            }
            else if (_rigidBody2D.velocity.y < 0) {
                ResetTriggers();
                _animator.SetTrigger("Jump_Down");
            }

            // contact with wall + not grounded
            if (_playerMovement.GetIsTouchingWallTop() && _playerMovement.GetIsTouchingWallBottom() && !_characterController2D.GetGrounded()) {
                
                // wall jump animation update
                if (_rigidBody2D.velocity.y > 15f) {
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
         string[] _triggers = {"Idle", "Crouch", "Crouch_Walk", "Run", "Jump_Up", "Jump_Down", "Wall_Slide", "Stop_Running", "Roll", "Attack1"};
    
        for (int i = 0; i < _triggers.Length - 1; i++) {
            _animator.ResetTrigger(_triggers[i]);
        }
    }
}
