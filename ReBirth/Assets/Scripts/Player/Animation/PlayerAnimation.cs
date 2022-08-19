using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Animator _animator;

    void LateUpdate() {
        if (_playerMovement.GetHorizontalMove() != 0) {
            // _animator.ResetTriggers();
            _animator.SetTrigger("Run");
        }
        else if (_playerMovement.GetHorizontalMove() == 0){
            // _animator.ResetTriggers();
            _animator.SetTrigger("Idle");
        }
    }


}
