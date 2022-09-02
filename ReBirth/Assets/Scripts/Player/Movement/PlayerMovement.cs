using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D _characterController2D;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private Transform _wallCheckTop;
    [SerializeField] private Transform _wallCheckBottom;
    [SerializeField] private GameObject _jumpBuffer;

    [SerializeField] private float _genericTransformRadius;
    [SerializeField] private float _wallCheckRadius;
    [SerializeField] private float _wallJumpMultiplier;
    [SerializeField] private float _jumpBufferRadius;
    [Range(0, 80f)] [SerializeField] private float _runSpeed;
    [Range(0, 10f)] [SerializeField] private float _fallMultiplier;
    [Range(0, 10f)] [SerializeField] private float _midMultiplier;
    [Range(0, 10f)] [SerializeField] private float _lowMultiplier;
    [Range(0, 10f)] [SerializeField] private float _wallSlideSpeed;
    private float _horizontalMove = 0f;
    private float _hangCounter;
    private const float HANG_TIME = .2f;

    [SerializeField] private bool _canMove = true;
    private bool _jump = false;
    private bool _canJump = true;
    private bool _doubleJump = false;
    private bool _canDoubleJump = false;
    private bool _crouch = false;
    private bool _roll = false;
    private bool _isTouchingCeiling;
    private bool _isTouchingWallTop;
    private bool _isTouchingWallBottom;
    // private bool _turnAround = false;

    [SerializeField] private LayerMask _whatIsWall;
    [SerializeField] private LayerMask _forceCrouchLayers;
    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private ParticleSystem _particleSystem;

    void Start() => _hangCounter = HANG_TIME; // used for jump assist

    // Update is called once per frame
    void Update() {

        // Jump Assist which lets you jump after you've slipped off a platform and pressed jump within .x seconds
        if (_characterController2D.GetGrounded())
            _hangCounter = HANG_TIME;
        else
            _hangCounter -= Time.deltaTime;

        
        if (_canMove) {

            _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;
            
            if (_characterController2D.GetGrounded() || (_isTouchingWallBottom && _isTouchingWallTop))
                _canDoubleJump = true;

            if (Input.GetButtonDown("Jump") && _characterController2D.GetGrounded() && _canJump)
                _jump = true;

            // Check if both Wall Checks are false so Wall Jump and Double Jump JumpForce isn't added together before the player is moved
            if (Input.GetButtonDown("Jump") && !_characterController2D.GetGrounded() && _canDoubleJump && !_isTouchingWallTop && !_isTouchingWallBottom) {
                _doubleJump = true;
                _canDoubleJump = false;
            }

            if (Input.GetButtonDown("Roll") && _characterController2D.GetGrounded() && !_crouch) {
                _roll = true;
                Debug.Log("roll");
            }

            JumpBuffer();
            Crouch();
            WallJump();
        }
        else
            _horizontalMove = 0;
    }

    void FixedUpdate() {
        _characterController2D.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump, _doubleJump, _roll);
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

        // // reverse terminal velocity
        if (_rigidBody2D.velocity.x < 25f)
            _rigidBody2D.velocity = new Vector2(25f, _rigidBody2D.velocity.y);

        // Wall Slide
        if (_rigidBody2D.velocity.y < 0 && _isTouchingWallBottom && _isTouchingWallTop) {
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Mathf.Clamp(_rigidBody2D.velocity.y, -_wallSlideSpeed, float.MaxValue));
        }
    }

    private void Crouch() {
            if (Input.GetButton("Crouch")) {
                _canJump = false;
                _crouch = true;
            }
            else {
                if (_isTouchingCeiling) {
                    _canJump = false;
                    _crouch = true;
                }
                else {
                    _canJump = true;
                    _crouch = false;
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

    public void CreateDust() { _particleSystem.Play(); }

    public float GetHorizontalMove() { return _horizontalMove; }

    public bool GetCrouch() { return _crouch; }

    public bool GetRoll() { return _roll; }

    public bool GetJump() { return _jump; }

    public bool GetIsTouchingWallTop() { return _isTouchingWallTop; }

    public bool GetIsTouchingWallBottom() { return _isTouchingWallBottom; }
}
