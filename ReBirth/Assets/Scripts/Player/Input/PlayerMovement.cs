using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    To handle all movement related logic for the player model.
Last Edited:
    10-14-22.
*/
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private Transform _ceilingCheck,
        _wallCheckTop,
        _wallCheckBottom;// two wall checks to determine to wall slide

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _jumpBuffer, 
        _wallJumpBuffer;
    [Range(0, 80f)] [SerializeField] private float _runSpeed;

    // variables to manipulate player gravity 
    [Range(0, 10f)] [SerializeField] private float _fallMultiplier, 
        _midMultiplier, 
        _lowMultiplier, 
        _wallSlideSpeed;
    
    [SerializeField] private float _genericTransformRadius, 
        _wallCheckRadius, 
        _wallJumpMultiplier, 
        _jumpBufferRadius, 
        _wallJumpBufferRadius, 
        _rollSpeed,
        _horizontalMove = 0f,
        _hangCounter, 
        _jumpHangCounter;
    private const float HANG_TIME = .2f, // time allowed to jump after walking off ledge
        WALL_HANG_TIME = .2f; // time allowed to single jump after letting go of a ledge

    [SerializeField] private bool _canMove = true,
        _isMoving = false,
        _jump = false,
        _canJump = true,
        _doubleJump = false,
        _canDoubleJump = false,
        _crouch = false,
        _roll = false,
        _isRolling = false,
        _isTouchingCeiling,
        _isTouchingWallTop,
        _isTouchingWallBottom;
    // private bool _turnAround = false;

    [SerializeField] private LayerMask _whatIsWall, // placed on walls which the player can wall slide
        _forceCrouchLayers, // placed on low-hanging, crouchable ceilings
        _whatIsGround; // player ground animations only trigger on this layer

    [SerializeField] private ParticleSystem _particleSystem;

    void Start() { 
        _hangCounter = HANG_TIME; // used for jump assist
        _jumpHangCounter = HANG_TIME;
    }

    void Update() {

        // Jump Assist which lets you jump after you've slipped off a platform and pressed jump within .x seconds
        if (_characterController2D.GetGrounded()) {
            _hangCounter = HANG_TIME;
            _jumpHangCounter = HANG_TIME;
        }
        else
            _hangCounter -= Time.deltaTime;

        if (_canMove && !_isRolling) {
            _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;

            if (_horizontalMove != 0)
                _isMoving = true;
            else
                _isMoving = false;
            
            // reset double jump
            if (_characterController2D.GetGrounded() || (_isTouchingWallBottom && _isTouchingWallTop))
                _canDoubleJump = true;

            if (Input.GetButtonDown("Jump") && _characterController2D.GetGrounded() && _canJump && !_isRolling)
                _jump = true;

            // check if both wall checks are false so wall jump and double jump jumpForce isn't added together before the player is moved
            if (Input.GetButtonDown("Jump") && !_characterController2D.GetGrounded() && _canDoubleJump && !_isTouchingWallTop && !_isTouchingWallBottom) {
                _doubleJump = true;
                _canDoubleJump = false;
            }

            Roll();
            JumpBuffer();
            // WallJumpBuffer();
            Crouch();
            WallJump();
        }
        // if the player input is disabled, make sure to stop the player model
        else if (!_canMove && !_isRolling)
            _horizontalMove = 0;
        else if (_isRolling && _characterController2D.GetFacingRight())// and facing the right
            _horizontalMove = _rollSpeed;
        else if (_isRolling && !_characterController2D.GetFacingRight())// and facing the left
            _horizontalMove = -_rollSpeed;
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
            // if you let go of crouch, check if there's a ceiling above the player...
            if (Input.GetButton("Crouch") && !_isRolling) {
                _canJump = false;
                _crouch = true;
            }
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
    private void JumpBuffer() {
        Collider2D _bufferActive = Physics2D.OverlapCircle(_jumpBuffer.transform.position, _jumpBufferRadius, _whatIsGround);

        if (_bufferActive && !_characterController2D.GetGrounded() && Input.GetButtonDown("Jump") && 
            !_isTouchingWallTop && !_isTouchingWallBottom && _hangCounter > 0f) {
            CreateDust();
            _characterController2D.ResetForce(); // you don't want gravity or other forces applied
            _characterController2D.ApplyForce(0, _characterController2D.GetJumpForce()); // same force as a single jump, since a jump buffer has the same intentions as a single jump
            _canDoubleJump = true; // reset double jump after jump buffer (since a jump buffer is intended to act as a single jump)
            Debug.Log("Jump Buffer");
        }
    }

    // jump assist for when you let go of wall sliding before wall jumping and you intend to wall jump
    private void WallJumpBuffer() {
        Collider2D _bufferActive = Physics2D.OverlapCircle(_wallJumpBuffer.transform.position, _wallJumpBufferRadius, _whatIsWall);

        if (_bufferActive && !_characterController2D.GetGrounded() && Input.GetButtonDown("Jump") && 
            !_isTouchingWallTop && !_isTouchingWallBottom && _jumpHangCounter > 0f) {
            CreateDust();
            _characterController2D.ResetForce(); // you don't want gravity or other forces applied
            _characterController2D.ApplyForce(0, _characterController2D.GetJumpForce()); // same force as a single jump, since a jump buffer has the same intentions as a single jump
            _canDoubleJump = true; // reset double jump after jump buffer (since a jump buffer is intended to act as a single jump)
            Debug.Log("Wall Jump Buffer");
            }
    }

    private void Roll() {
        if (Input.GetButtonDown("Roll") && _characterController2D.GetGrounded() && _isMoving && !_isRolling) {
            _roll = true;
            _isRolling = true;
            CreateDust();
            Debug.Log("Roll");
        }
    }

    public GameObject GetJumpBuffer() { return _jumpBuffer; }

    public void CreateDust() { _particleSystem.Play(); }
    public void SetIsRolling(bool _isRolling) { this._isRolling = _isRolling; }
    public void SetIsRollingFalse() { _isRolling = false; }

    public float GetHorizontalMove() { return _horizontalMove; }
    public float GetHangCounter() { return _hangCounter; }
    public float GetJumpBufferRadius() { return _jumpBufferRadius; }

    public bool GetCrouch() { return _crouch; }
    public bool GetRoll() { return _roll; }
    public bool GetIsRolling() { return _isRolling; }
    public bool GetJump() { return _jump; }
    public bool GetIsTouchingWallTop() { return _isTouchingWallTop; }
    public bool GetIsTouchingWallBottom() { return _isTouchingWallBottom; }

}
