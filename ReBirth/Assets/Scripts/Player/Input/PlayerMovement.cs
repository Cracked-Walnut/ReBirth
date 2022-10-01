using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    To handle all movement related logic for the player model.
Last Edited:
    10-01-22.
*/
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private Transform _ceilingCheck;

    // two wall checks to determine to wall slide
    [SerializeField] private Transform _wallCheckTop;
    [SerializeField] private Transform _wallCheckBottom;
    

    [SerializeField] private GameObject _jumpBuffer;

    [Range(0, 80f)] [SerializeField] private float _runSpeed;

    // variables to manipulate player gravity 
    [Range(0, 10f)] [SerializeField] private float _fallMultiplier;
    [Range(0, 10f)] [SerializeField] private float _midMultiplier;
    [Range(0, 10f)] [SerializeField] private float _lowMultiplier;
    
    [Range(0, 10f)] [SerializeField] private float _wallSlideSpeed;
    [SerializeField] private float _genericTransformRadius;
    [SerializeField] private float _wallCheckRadius;
    [SerializeField] private float _wallJumpMultiplier;
    [SerializeField] private float _jumpBufferRadius;
    [SerializeField] private float _rollSpeed;
    private const float HANG_TIME = .2f; // time allowed to jump after walking off ledge
    private float _horizontalMove = 0f;
    private float _hangCounter;

    [SerializeField] private bool _canMove = true;
    private bool _jump = false;
    private bool _canJump = true;
    private bool _doubleJump = false;
    private bool _canDoubleJump = false;
    private bool _crouch = false;
    private bool _roll = false;
    private bool _isRolling = false;
    private bool _isTouchingCeiling;
    private bool _isTouchingWallTop;
    private bool _isTouchingWallBottom;
    // private bool _turnAround = false;

    [SerializeField] private LayerMask _whatIsWall; // placed on walls which the player can wall slide
    [SerializeField] private LayerMask _forceCrouchLayers; // placed on low-hanging, crouchable ceilings
    [SerializeField] private LayerMask _whatIsGround; // player ground animations only trigger on this layer

    [SerializeField] private ParticleSystem _particleSystem;

    void Start() => _hangCounter = HANG_TIME; // used for jump assist

    void Update() {

        // Jump Assist which lets you jump after you've slipped off a platform and pressed jump within .x seconds
        if (_characterController2D.GetGrounded())
            _hangCounter = HANG_TIME;
        else
            _hangCounter -= Time.deltaTime;

        
        if (_canMove) {

            _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;
            
            // reset double jump
            if (_characterController2D.GetGrounded() || (_isTouchingWallBottom && _isTouchingWallTop))
                _canDoubleJump = true;

            if (Input.GetButtonDown("Jump") && _characterController2D.GetGrounded() && _canJump)
                _jump = true;

            // check if both wall checks are false so wall jump and double jump jumpForce isn't added together before the player is moved
            if (Input.GetButtonDown("Jump") && !_characterController2D.GetGrounded() && _canDoubleJump && !_isTouchingWallTop && !_isTouchingWallBottom) {
                _doubleJump = true;
                _canDoubleJump = false;
            }

            // Roll();

            JumpBuffer();
            Crouch();
            WallJump();
        }
        // if the player input is disabled, make sure to stop the player model
        else
            _horizontalMove = 0;
    }

    void FixedUpdate() {
        _characterController2D.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump, _doubleJump, _roll);
        
        // quickly reset these variables so the action isn't executed forever
        _jump = false;
        _doubleJump = false;
        _roll = false;
    }

    void LateUpdate() {

        _isTouchingCeiling = Physics2D.OverlapCircle(_ceilingCheck.transform.position, _genericTransformRadius, _forceCrouchLayers);
        _isTouchingWallTop = Physics2D.OverlapCircle(_wallCheckTop.transform.position, _wallCheckRadius, _whatIsWall);
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

        // reverse terminal velocity
        if (_rigidBody2D.velocity.x < 25f)
            _rigidBody2D.velocity = new Vector2(25f, _rigidBody2D.velocity.y);

        // wall slide
        if (_rigidBody2D.velocity.y < 0 && _isTouchingWallBottom && _isTouchingWallTop) {
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Mathf.Clamp(_rigidBody2D.velocity.y, -_wallSlideSpeed, float.MaxValue));
        }
    }

    private void Crouch() {
        if (_characterController2D.GetGrounded()) {
            if (Input.GetButton("Crouch")) {
                _canJump = false;
                _crouch = true;
            }
            // if you let go of crouch, check if there's a ceiling above the player...
            else {
                // ... if there is, keep crouched and disable jump
                if (_isTouchingCeiling) {
                    _canJump = false;
                    _crouch = true;
                }
                // ... otherwise, stand up and enable jump
                else {
                    _canJump = true;
                    _crouch = false;
                }
            }
        }
    }

    private void WallJump() {
        if (_isTouchingWallBottom && _isTouchingWallTop && !_characterController2D.GetGrounded()) {
            if (Input.GetButtonDown("Jump")) {
                _characterController2D.ResetForce();
                _characterController2D.ApplyForce(0, _characterController2D.GetJumpForce() * _wallJumpMultiplier);
                CreateDust();
            }
        }
    }

    // jump assist for when you walk off a ledge
    void JumpBuffer() {
        Collider2D _bufferActive = Physics2D.OverlapCircle(_jumpBuffer.transform.position, _jumpBufferRadius, _whatIsGround);

        if (_bufferActive && !_characterController2D.GetGrounded() && Input.GetButtonDown("Jump") && !_isTouchingWallTop && !_isTouchingWallBottom && _hangCounter > 0f && !_doubleJump) {
            CreateDust();
            _characterController2D.ResetForce(); // you don't want gravity or other forces applied
            _characterController2D.ApplyForce(0, _characterController2D.GetJumpForce()); // same force as a single jump, since a jump buffer has the same intentions as a single jump
            _canDoubleJump = true; // reset double jump after jump buffer (since a jump buffer is intended to act as a single jump)
            Debug.Log("Jump Buffer Performed!");
        }
    }

    void Roll() {
        if (Input.GetButtonDown("Roll") && _characterController2D.GetGrounded() && !_crouch) {
            _roll = true;
            _isRolling = true;
            CreateDust();
            Debug.Log("roll");
        }
    }

    public void CreateDust() { _particleSystem.Play(); }

    public float GetHorizontalMove() { return _horizontalMove; }

    public bool GetCrouch() { return _crouch; }

    public bool GetRoll() { return _roll; }

    public bool GetJump() { return _jump; }

    public bool GetIsTouchingWallTop() { return _isTouchingWallTop; }

    public bool GetIsTouchingWallBottom() { return _isTouchingWallBottom; }
}
