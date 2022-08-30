using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [Range(0, 80f)] [SerializeField] private float _runSpeed;
    [Range(0, 10f)] [SerializeField] private float _fallMultiplier;
    [Range(0, 10f)] [SerializeField] private float _midMultiplier;
    [Range(0, 10f)] [SerializeField] private float _lowMultiplier;

    private float _horizontalMove = 0f;
    private bool _jump = false;
    private bool _canJump = true;
    private bool _doubleJump = false;
    private bool _canDoubleJump = false;
    private bool _crouch = false;
    [SerializeField] private bool _canMove = true;
    // private bool _turnAround = false;

    [SerializeField] private float _wallCheckRadius;
    [SerializeField] private LayerMask _whatIsWall;

    [SerializeField] private Transform _wallCheckTop;
    private bool _isTouchingWallTop;
    
    // [SerializeField] private Transform _wallCheckMiddle;
    // private bool _isTouchingWallMiddle;

    [SerializeField] private Transform _wallCheckBottom;
    private bool _isTouchingWallBottom;

    [Range(0, 10f)] [SerializeField] private float _wallSlideSpeed;

    [SerializeField] private ParticleSystem _particleSystem;


    // Update is called once per frame
    void Update() {

        
        if (_canMove) {

            _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;
            
            if (_characterController2D.GetGrounded() || (_isTouchingWallBottom && _isTouchingWallTop))
                _canDoubleJump = true;

            if (Input.GetButtonDown("Jump") && _characterController2D.GetGrounded() && _canJump)
                _jump = true;

            if (Input.GetButtonDown("Jump") && !_characterController2D.GetGrounded() && _canDoubleJump && !_isTouchingWallTop && !_isTouchingWallBottom) {
                _doubleJump = true;
                _canDoubleJump = false;
            }

            Crouch();
            WallJump();
        }
        else
            _horizontalMove = 0;
    }

    void FixedUpdate() {
        _characterController2D.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump, _doubleJump);
        _jump = false;
        _doubleJump = false;
    }

    void LateUpdate() {

        _isTouchingWallTop = Physics2D.OverlapCircle(_wallCheckTop.transform.position, _wallCheckRadius, _whatIsWall);
        // _isTouchingWallMiddle = Physics2D.OverlapCircle(_wallCheckMiddle.transform.position, _wallCheckRadius, _whatIsWall);
        _isTouchingWallBottom = Physics2D.OverlapCircle(_wallCheckBottom.transform.position, _wallCheckRadius, _whatIsWall);

        // when the player is falling, apply more gravity
        if (_rigidBody2D.velocity.y < 0)
            _rigidBody2D.velocity += (_fallMultiplier - 1) * Time.deltaTime * Vector2.up * Physics2D.gravity.y;
        
        // when the player is rising and they're holding the jump key, apply gravity
        if (_rigidBody2D.velocity.y > 0 && Input.GetKey(KeyCode.Space))
            _rigidBody2D.velocity += (_midMultiplier - 1) * Time.deltaTime * Vector2.up * Physics2D.gravity.y;

        // when the player is falling and they're not holding the jump key, apply gravity
        if (_rigidBody2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            _rigidBody2D.velocity += (_lowMultiplier - 1) * Time.deltaTime * Vector2.up * Physics2D.gravity.y;

        // terminal velocity
        if (_rigidBody2D.velocity.y < -25f)
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, -25f);

        // // reverse terminal velocity
        if (_rigidBody2D.velocity.x < 25f)
            _rigidBody2D.velocity = new Vector2(25f, _rigidBody2D.velocity.y);

        // Wall Slide
        if (_rigidBody2D.velocity.y < 0 && _isTouchingWallBottom && _isTouchingWallTop) {
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Mathf.Clamp(_rigidBody2D.velocity.y, -_wallSlideSpeed, float.MaxValue));
        }
    }

    private void Crouch() {
        if (_characterController2D.GetGrounded()) {

            if (Input.GetButtonDown("Crouch")) {
                _canJump = false;
                _crouch = true;
            }
            else if (Input.GetButtonUp("Crouch")) {
                _canJump = true;
                _crouch = false;
            }
        }
    }

    private void WallJump() {
        if (_isTouchingWallBottom && _isTouchingWallTop && !_characterController2D.GetGrounded()) {
            if (Input.GetButtonDown("Jump")) {
                _characterController2D.ResetForce();
                _characterController2D.ApplyForce(0, _characterController2D.GetJumpForce());
                CreateDust();
            }
        }
    }

    public void CreateDust() { _particleSystem.Play(); }

    public float GetHorizontalMove() { return _horizontalMove; }

    public bool GetCrouch() { return _crouch; }

    public bool GetJump() { return _jump; }

    public bool GetIsTouchingWallTop() { return _isTouchingWallTop; }

    public bool GetIsTouchingWallBottom() { return _isTouchingWallBottom; }
}
