using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody2D;

    void LateUpdate() {

        // if (_characterController2D.GetGrounded()) {
            // if (_playerMovement.GetHorizontalMove() != 0) {
            //     // _animator.ResetTriggers();
            //     _animator.SetTrigger("Run");
            // }
            // if (_playerMovement.GetHorizontalMove() == 0) {
            //     // _animator.ResetTriggers();
            //     _animator.SetTrigger("Idle");
            // }

            // if (_playerMovement.GetCrouch() && _playerMovement.GetHorizontalMove() == 0)
            //     _animator.SetTrigger("Crouch");

            // else if (_playerMovement.GetCrouch() && _playerMovement.GetHorizontalMove() != 0)
            //     _animator.SetTrigger("Crouch_Walk");

            if (_characterController2D.GetGrounded()) {
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
        // }
        else {
            _animator.SetBool("IsGrounded", _characterController2D.GetGrounded());

            if (_rigidBody2D.velocity.y > 0) {
                ResetTriggers();
                _animator.SetTrigger("Jump_Up");
            }

            else if (_rigidBody2D.velocity.y < 0) {
                ResetTriggers();
                _animator.SetTrigger("Jump_Down");
            }
        }
    }

    void ResetTriggers() {
         string[] _triggers = {"Idle", "Crouch", "Crouch_Walk", "Run", "Jump_Up", "Jump_Down"};
    
        for (int i = 0; i < _triggers.Length - 1; i++) {
            _animator.ResetTrigger(_triggers[i]);
        }
    }
}
