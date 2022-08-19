using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D _characterController2D;

    [SerializeField] Rigidbody2D _rigidBody2D;

    float _horizontalMove = 0f;
    [SerializeField] float _runSpeed = 40f;
    bool _jump = false;
    bool _crouch = false;
    [SerializeField] float _fallMultiplier;
    // [SerializeField] float _midMultiplier;
    [SerializeField] float _lowMultiplier;
    

    // Update is called once per frame
    void Update() {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;

        if (Input.GetButtonDown("Jump") && _characterController2D.GetGrounded())
            _jump = true;

        if (Input.GetButtonDown("Crouch"))
            _crouch = true;
        else if (Input.GetButtonUp("Crouch"))
            _crouch = false;
    }

    void FixedUpdate() {
        _characterController2D.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump);
        _jump = false;
    }

    void LateUpdate() {
        // when the player is falling, apply more gravity
        if (_rigidBody2D.velocity.y < 0)
            _rigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        
        // when the player is rising and they're holding the jump key, apply gravity
        // if (_rigidBody2D.velocity.y > 0 && Input.GetKey(KeyCode.Space))
        //     _rigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (_midMultiplier - 1) * Time.deltaTime;

        // when the player is falling and they're not holding the jump key, apply gravity
        if (_rigidBody2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            _rigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (_lowMultiplier - 1) * Time.deltaTime;

        // // terminal velocity
        if (_rigidBody2D.velocity.y < -25f)
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, -25f);

        // // reverse terminal velocity
        if (_rigidBody2D.velocity.x < 25f)
            _rigidBody2D.velocity = new Vector2(25f, _rigidBody2D.velocity.y);
    }
}
