using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour {

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CharacterController2D _characterController2D;

    [SerializeField] private bool _drawJumpBuffer,
        _drawGroundedRadius;

    void OnDrawGizmos() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;

        if (_drawJumpBuffer)
            Gizmos.DrawWireSphere(_playerMovement.GetJumpBuffer().transform.position, _playerMovement.GetJumpBufferRadius());

        Gizmos.color = Color.blue;

        if (_drawGroundedRadius)
            Gizmos.DrawWireSphere(_characterController2D.GetGroundCheck().transform.position, _characterController2D.GetGroundedRadius());

    }
}
