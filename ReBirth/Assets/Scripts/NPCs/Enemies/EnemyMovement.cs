using UnityEngine;

/*
Purpose:
    Contains generic information to move enemies.
Last Edited:
    06-25-23.
*/
public class EnemyMovement : MonoBehaviour {

    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isStationary;
    [SerializeField] private bool _isFlying;
    [SerializeField] private GameObject _ledgeDetector,
        _wallDetector;
    [SerializeField] private LayerMask _whatIsGround,
        _whatIsWall;

    void Update() {
        if (_isStationary) {
            _moveSpeed = 0f;
            return;
        }
        else {
            if (!_isFlying) {
            RaycastHit2D _ledgeDetection = Physics2D.Raycast(_ledgeDetector.transform.position, -Vector2.up, .7f, _whatIsGround);
            RaycastHit2D _wallDetection = Physics2D.Raycast(_wallDetector.transform.position, Vector2.right, .7f, _whatIsWall);

            transform.Translate(Vector2.right * _moveSpeed * Time.deltaTime); // movement

            if (_ledgeDetection.collider == null || _wallDetection.collider != null)
                transform.Rotate(0f, 180f, 0f);
            }
            else {
                _ledgeDetector = null;
                _wallDetector = null;
            }
        }
    }

    public float GetMovementSpeed() { return _moveSpeed; }
    public void SetMovementSpeed(float _moveSpeed) { this._moveSpeed = _moveSpeed; }

}
