using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour {

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private PlayerAttack _playerAttack;

    [SerializeField] private bool _drawJumpBuffer,
        _drawGroundedRadius,
        _drawAttackPoint;

    void OnDrawGizmos() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.white;

        if (_drawJumpBuffer)
            Gizmos.DrawWireSphere(_playerMovement.GetJumpBuffer().transform.position, _playerMovement.GetJumpBufferRadius());

        if (_drawGroundedRadius)
            Gizmos.DrawWireSphere(_characterController2D.GetGroundCheck().transform.position, _characterController2D.GetGroundedRadius());

        if (_drawAttackPoint)
            Gizmos.DrawWireSphere(_playerAttack.GetAttackPoint().position, _playerAttack.GetAttackRange());

    }
}
