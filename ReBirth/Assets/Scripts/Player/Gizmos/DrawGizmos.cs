using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    To draw gizmos around objects for visualization/ debugging purposes.

Last Edited:
    10-17-22.

How to Use:
    1. Declare a boolean for the gizmos you wish to draw.
    2. Write a conditional statement to turn the gizmos on and off.
    3. 
        a. Call in any objects you want associated with your gizmos.
        b. Draw your preffered gizmos.
*/

public class DrawGizmos : MonoBehaviour {

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private PlayerAttack _playerAttack;

    // [SerializeField] private bool _drawJumpBuffer,
    [SerializeField] private bool _drawGroundedRadius,
        _drawAttackPoint;

    void OnDrawGizmos() {
        // Draw a yellow circle at the transform's position
        Gizmos.color = Color.white;

        // if (_drawJumpBuffer)
        //     Gizmos.DrawWireCircle(_playerMovement.GetJumpBuffer().transform.position, _playerMovement.GetJumpBufferRadius());

        if (_drawGroundedRadius)
            Gizmos.DrawWireSphere(_characterController2D.GetGroundCheck().transform.position, _characterController2D.GetGroundedRadius());

        if (_drawAttackPoint)
            Gizmos.DrawWireSphere(_playerAttack.GetAttackPoint().position, _playerAttack.GetAttackRange());

    }
}
