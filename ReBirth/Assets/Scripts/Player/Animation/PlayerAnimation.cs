using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Animator _animator;

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

            if (_playerMovement.GetHorizontalMove() == 0) {
                if (_playerMovement.GetCrouch())
                    _animator.SetTrigger("Crouch");
                else
                    _animator.SetTrigger("Idle");
            }
            else if (_playerMovement.GetHorizontalMove() != 0) {
                if (_playerMovement.GetCrouch())
                    _animator.SetTrigger("Crouch_Walk");
                else
                    _animator.SetTrigger("Run");
            }
        // }
    }


}
