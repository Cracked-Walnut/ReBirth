using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Purpose:
    To handle all movement related logic for the player model.
Last Edited:
    01-17-23.
*/
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private Transform _ceilingCheck,
        // two wall checks to determine to wall slide
        _wallCheckTop,
        _wallCheckBottom,

        // two transform positions, where raycasts will be placed to determine LedgeJump() function
        _midLowerCheck,
        _feetCheck;
    [SerializeField] private RaycastHit2D _rMidLower, _rFeet;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _jumpBuffer, 
        _wallJumpBuffer;

    [Range(0, 80f)] [SerializeField] private float _runSpeed;

    // variables to manipulate player gravity 
    [Range(0, 10f)] [SerializeField] private float _fallMultiplier, 
        _midMultiplier, 
        _lowMultiplier, 
        _wallSlideSpeed;

    [Range(2f, 4f)] [SerializeField] private float _wallSlideSpeedMultiplier;
    
    [SerializeField] private float _genericTransformRadius, 
        _wallCheckRadius, 
        _wallJumpMultiplier, 
        _jumpBufferRadius, 
        _wallJumpBufferRadius, 
        _rollSpeed,
        _dashSpeedX,
        _dashSpeedY,
        _horizontalMove = 0f,
        _terminalVelocity, // max -y velocity
        _reverseTerminalVelocity, // max +y velocity
        _hangCounter,
        _jumpHangCounter,
        _ledgeDetectionDistance;

    private const float HANG_TIME = .2f, // time allowed to jump after walking off ledge
        WALL_HANG_TIME = .2f; // time allowed to single jump after letting go of a wall

    private Collider2D _jumpBufferActive, 
        _wallJumpBufferActive;

    [SerializeField] private bool _canMove,
        _canJump, 
        _canDoubleJump,
        _canMidAirDash,
        _canRoll,
        _canWallDash,
        _canJumpBuffer, 
        _crouch,
        _canWallJumpBuffer,
        _jump, 
        _doubleJump, 
        _midAirDash, 
        _isMidAirDashing , 
        _wallDash, 
        _isMoving,
        _isRolling, 
        _isWallDashing, 
        _isTouchingCeiling, 
        _isTouchingWallTop, 
        _isTouchingWallBottom, 
        _isWallSliding,
        _roll;

    // private bool _turnAround = false;

    [SerializeField] private LayerMask _whatIsWall, // placed on walls which the player can wall slide
        _forceCrouchLayers, // placed on low-hanging, crouchable ceilings
        _whatIsGround, // player ground animations only trigger on this layer
        _whatIsLedge;

    [SerializeField] private ParticleSystem _particleSystem;

    void Start() { 
        // both used for jump assist
        _hangCounter = HANG_TIME;
        _jumpHangCounter = HANG_TIME;

        _canMove = true;
        _canJump = true;
        _canDoubleJump = false;
        _canMidAirDash = true;
        _canRoll = true;
        _canWallDash = true;
        _canJumpBuffer = true;
        _crouch = false;
        _canWallJumpBuffer = true;
        _jump = false;
        _doubleJump = false;
        _midAirDash = false;
        _isMidAirDashing = false;
        _wallDash = false;
        _isMoving = false;
        _isRolling = false;
        _isWallDashing = false;
        _roll = false;
    }

    void Update() {
        if (_characterController2D.GetGrounded()) {
            
            // reset the jump counters when player touches ground so they can work again
            _hangCounter = HANG_TIME;
            _jumpHangCounter = HANG_TIME;
        }
        else
            DecrementVariable(_hangCounter);

        if (_canMove && !_isRolling) {
            _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;

            if (_horizontalMove != 0)
                _isMoving = true;
            else
                _isMoving = false;

            if (_characterController2D.GetGrounded() || _isWallSliding)
                ResetDoubleJump();

            CheckJump();
            CheckDoubleJump();
            Roll();
            StartCoroutine(MidAirDash());
            WallDash();
            JumpBuffer();
            WallJumpBuffer();
            // Debug.Log(LedgeJump());
            // JumpBuffer(_jumpBufferActive, _jumpBuffer, _jumpBufferRadius, _whatIsGround, _canJumpBuffer, "Jump Buffer");
            // JumpBuffer(_wallJumpBufferActive, _wallJumpBuffer, _wallJumpBufferRadius, _whatIsWall, _canWallJumpBuffer, "Wall Jump Buffer");
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

        JumpHeightControl();
        TerminalVelocity();
        ReverseTerminalVelocity();

        if (!_characterController2D.GetGrounded()) {
            // wall slide
            if (_isTouchingWallBottom && _isTouchingWallTop) {
                if (Input.GetButton("Crouch"))
                    _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Mathf.Clamp(_rigidBody2D.velocity.y, -_wallSlideSpeed * _wallSlideSpeedMultiplier, float.MaxValue));
                else {
                    _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Mathf.Clamp(_rigidBody2D.velocity.y, -_wallSlideSpeed, float.MaxValue));
                    _isWallSliding = true;
                }
            }
        }

        if (!_isTouchingWallBottom && !_isTouchingWallTop)
            _isWallSliding = false;
    }

    private void Crouch() {
        if (_characterController2D.GetGrounded()) {
            if (Input.GetButton("Crouch") && !_isRolling) {
                _canJump = false;
                _crouch = true;
                _canRoll = false;
            }
            // if you let go of crouch, check if there's a ceiling above the player...
            else {
                // ... if there is, keep crouched and disable jump and roll
                if (_isTouchingCeiling) {
                    _canJump = false;
                    _crouch = true;
                    _canRoll = false;
                }
                // ... otherwise, stand up and enable jump and roll
                else {
                    _canJump = true;
                    _crouch = false;
                    _canRoll = true;
                }
            }
        }
    }

    private void WallJump() {
        if (_isWallSliding && !_characterController2D.GetGrounded()) {
            if (Input.GetButtonDown("Jump")) {
                _characterController2D.ResetForce();
                _characterController2D.ApplyForce(0, _characterController2D.GetJumpForce() * _wallJumpMultiplier);
                CreateDust();
            }
        }
    }

    private void ResetDoubleJump() {
        _isMidAirDashing = false;
        _canMidAirDash = true;
        _canDoubleJump = true;
    }

    // public void JumpBuffer(Collider2D _bufferActive, GameObject _jumpBuffer, float _radius, LayerMask _layerInteraction, bool _canBuffer, string _debugLog) {
    //     _bufferActive = Physics2D.OverlapCircle(_jumpBuffer.transform.position, _radius, _layerInteraction);

    //     if (_bufferActive && !_characterController2D.GetGrounded() && !_isWallSliding && _hangCounter > 0f) {
    //         _canBuffer = true;

    //         if (Input.GetButtonDown("Jump")) {

    //             CreateDust();
    //             _characterController2D.ResetForce(); // you don't want gravity or other forces applied
    //             _characterController2D.ApplyForce(0, _characterController2D.GetJumpForce()); // same force as a single jump, since a jump buffer has the same intentions as a single jump
    //             _canDoubleJump = true; // reset double jump after jump buffer (since a jump buffer is intended to act as a single jump)
    //             Debug.Log(_debugLog);
    //         }
    //     }
    //     else
    //         _canBuffer = false;
    // }

    // jump assist for when you walk off a ledge
    public void JumpBuffer() {
        Collider2D _bufferActive = Physics2D.OverlapCircle(_jumpBuffer.transform.position, _jumpBufferRadius, _whatIsGround);

        if (_bufferActive && !_characterController2D.GetGrounded() && !_isWallSliding && _hangCounter > 0f) {
            _canJumpBuffer = true;

            if (Input.GetButtonDown("Jump")) {
                
                CreateDust();
                _characterController2D.ResetForce(); // you don't want gravity or other forces applied
                _characterController2D.ApplyForce(0, _characterController2D.GetJumpForce()); // same force as a single jump, since a jump buffer has the same intentions as a single jump
                _canDoubleJump = true; // reset double jump after jump buffer (since a jump buffer is intended to act as a single jump)
                Debug.Log("Jump Buffer");
            }
        }
        else
            _canJumpBuffer = false;
    }

    // jump assist for when you let go of wall sliding before wall jumping and you intend to wall jump
    private void WallJumpBuffer() {
        Collider2D _bufferActive = Physics2D.OverlapCircle(_wallJumpBuffer.transform.position, _wallJumpBufferRadius, _whatIsWall);

        if (_bufferActive && !_characterController2D.GetGrounded() && !_isWallSliding && _jumpHangCounter > 0f) {
            _canWallJumpBuffer = true;

            if (Input.GetButtonDown("Jump")) {
                
                CreateDust();
                _characterController2D.ResetForce(); // you don't want gravity or other forces applied
                _characterController2D.ApplyForce(0, _characterController2D.GetJumpForce()); // same force as a single jump, since a jump buffer has the same intentions as a single jump
                _canDoubleJump = true; // reset double jump after jump buffer (since a jump buffer is intended to act as a single jump)
                Debug.Log("Wall Jump Buffer");
            }
        }
        else
            _canWallJumpBuffer = false;
    }

    private void Roll() {
        if (Input.GetButtonDown("Roll") && _characterController2D.GetGrounded() && 
            _isMoving && !_isRolling && _canRoll) {

            _roll = true;
            _isRolling = true;
            CreateDust();
        }
    }

    private IEnumerator MidAirDash() {
        if (Input.GetButtonDown("Roll") && !_characterController2D.GetGrounded() && !_isMidAirDashing && 
            _canMidAirDash && !_isWallSliding) {

            _midAirDash = true;
            _isMidAirDashing = true;
            _canMidAirDash = false; // becomes true when grounded
            CreateDust();
        }

        if (_isMidAirDashing && _canMove) {
            if (_characterController2D.GetFacingRight()) {
                _characterController2D.ResetForce(); // reset velocity
                _horizontalMove = _dashSpeedX; // dash
                // _rigidBody2D.gravityScale = 0f;
                yield return new WaitForSeconds(.125f);
                // _rigidBody2D.gravityScale = 4f;
                _isMidAirDashing = false;
            }
            else {
                _characterController2D.ResetForce(); // reset velocity
                _horizontalMove = -_dashSpeedX; // dash
                yield return new WaitForSeconds(.125f);
                _isMidAirDashing = false;
            }
        }
    }

    private void WallDash() {
        if (Input.GetButtonDown("Roll") && !_characterController2D.GetGrounded() && !_isMidAirDashing && 
            _canMidAirDash && _isWallSliding) {

            _wallDash = true;
            _isWallDashing = true;
            _canWallDash = false; // becomes true when you touch a wall slide, or the ground
            CreateDust();
        }

        if (_isWallDashing && _canMove) {
            if (!_characterController2D.GetFacingRight()) {
                _characterController2D.ResetForce(); // reset velocity
                _characterController2D.ApplyForce(_dashSpeedX, _dashSpeedY); // left wall dash
                _isWallDashing = false;
            }
            else {
                _characterController2D.ResetForce(); // reset velocity
                _characterController2D.ApplyForce(-_dashSpeedX, _dashSpeedY); // right wall dash
                _isWallDashing = false;
            }
        }
    }

    private void WallClimb() {
        /*
        Use 2 Raycast2Ds or OverlapCircle2Ds to detect a wall to climb
            2nd Raycast will look for ground to climb
        If the 1st is false and 2nd is true:
            Initiate wall climb:
                Hold player in place
                Play wall climb animation
                Set player's position to the top

        Exceptions:
            you can only climb walls you can stand on, so ground layer only
        */
    }

    private void DecrementVariable(float _v) => _v -= Time.deltaTime;

    private void CheckJump() {
        if (Input.GetButtonDown("Jump") && _characterController2D.GetGrounded() && _canJump && !_isRolling)
            _jump = true;
    }

    private void CheckDoubleJump() {
        // check if both wall checks are false so wall jump and double jump jumpForce isn't added together before the player is moved
        if (Input.GetButtonDown("Jump") && !_characterController2D.GetGrounded() && _canDoubleJump && !_isWallSliding) {
            _doubleJump = true;
            _canDoubleJump = false;
        }
    }

    private void TerminalVelocity() {
        if (_rigidBody2D.velocity.y < -25f)
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, _terminalVelocity);
    }

    private void ReverseTerminalVelocity() {
        if (_rigidBody2D.velocity.y > 25f)
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, _reverseTerminalVelocity);
    }

    private void JumpHeightControl() {
        // when the player is falling, apply more gravity
        if (_rigidBody2D.velocity.y < 0)
            _rigidBody2D.velocity += (_fallMultiplier - 1) * Time.deltaTime * Vector2.up * Physics2D.gravity.y;
        
        // when the player is rising and they're holding the jump key, apply gravity
        if (_rigidBody2D.velocity.y > 0 && Input.GetKey(KeyCode.Space))
            _rigidBody2D.velocity += (_midMultiplier - 1) * Time.deltaTime * Vector2.up * Physics2D.gravity.y;

        // when the player is falling and they're not holding the jump key, apply gravity
        if (_rigidBody2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            _rigidBody2D.velocity += (_lowMultiplier - 1) * Time.deltaTime * Vector2.up * Physics2D.gravity.y;
    }

    private bool LedgeJump() {
        /*
        make 2 Raycasts. One at the feet and one at the mid-lower seciton
        If the head cast is null and mid-lower isn't and the player jumps, the jump force should be smaller than a normal jump
        */
        _rMidLower = Physics2D.Raycast(_midLowerCheck.position, Vector2.right, _ledgeDetectionDistance, _whatIsLedge);
        _rFeet = Physics2D.Raycast(_feetCheck.position, Vector2.right, _ledgeDetectionDistance, _whatIsLedge);

        if (_rMidLower == null && _rFeet != null)
            return true;
        return false;
    }

    public GameObject GetJumpBuffer() { return _jumpBuffer; }
    public GameObject GetWallJumpBuffer() { return _wallJumpBuffer; }

    public Transform GetWallCheckTop() { return _wallCheckTop; }
    public Transform GetWallCheckBottom() { return _wallCheckBottom; }
    // public Transform GetMidLowerCheck() { return _midLowerCheck; }
    // public Transform GetFeetCheck() { return _feetCheck; }

    public void CreateDust() { _particleSystem.Play(); }

    public float GetHorizontalMove() { return _horizontalMove; }
    public float GetHangCounter() { return _hangCounter; }
    public float GetJumpBufferRadius() { return _jumpBufferRadius; }
    public float GetWallJumpBufferRadius() { return _wallJumpBufferRadius; }
    public float GetWallCheckRadius() { return _wallCheckRadius; }
    // public float GetLedgeDetectionDistance() { return _ledgeDetectionDistance; }

    public bool GetCrouch() { return _crouch; }
    public bool GetRoll() { return _roll; }
    public bool GetIsRolling() { return _isRolling; }
    public bool GetIsMidAirDashing() { return _isMidAirDashing; }
    public bool GetJump() { return _jump; }
    public bool GetCanJumpBuffer() { return _canJumpBuffer; }
    public bool GetCanWallJumpBuffer() { return _canWallJumpBuffer; }
    public bool GetIsWallSliding() { return _isWallSliding; }
    public bool GetIsTouchingWallTop() { return _isTouchingWallTop; }
    public bool GetIsTouchingWallBottom() { return _isTouchingWallBottom; }

    public void SetIsRolling(bool _isRolling) => this._isRolling = _isRolling;
    public void SetRollingFalse() => _isRolling = false; // for the end of the rolling animation. Can't use conventional setter
    public void SetMidAirDashFalse() => _midAirDash = false; // for the end of the mid-air dash animation
}
