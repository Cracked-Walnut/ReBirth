using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D _characterController2D;

    float _horizontalMove = 0f;
    [SerializeField] float _runSpeed = 40f;
    bool _jump = false;
    bool _crouch = false;

    // Update is called once per frame
    void Update() {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;

        if (Input.GetButtonDown("Jump")) {
            _jump = true;
        }

        if (Input.GetButtonDown("Crouch")) {
            _crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {
            _crouch = false;
        }
    }

    void FixedUpdate() {
        _characterController2D.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump);
        _jump = false;
    }
}
