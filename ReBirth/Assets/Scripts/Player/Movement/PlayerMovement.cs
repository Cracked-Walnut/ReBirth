using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private float _runSpeed = 40f;
    [SerializeField] private float _fallMultiplier;
    // [SerializeField] private float _midMultiplier;
    [SerializeField] private float _lowMultiplier;

    private float _horizontalMove = 0f;
    private bool _jump = false;
    private bool _crouch = false;
    

    // Update is called once per frame
    void Update() {

        _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;

        if (Input.GetButtonDown("Jump") && _characterController2D.GetGrounded())
            _jump = true;

        if (_characterController2D.GetGrounded()) {
            if (Input.GetButtonDown("Crouch"))
                _crouch = true;
            else if (Input.GetButtonUp("Crouch"))
                _crouch = false;
        }
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

    public float GetHorizontalMove() {
        return _horizontalMove;
    }

    public bool GetCrouch() {
        return _crouch;
    }
}
