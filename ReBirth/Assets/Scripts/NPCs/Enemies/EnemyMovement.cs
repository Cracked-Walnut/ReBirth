using UnityEngine;

/*
Purpose:
    To handle Ghoul movement.
Last Edited:
    01-06-23.
*/
public class EnemyMovement : MonoBehaviour {

    [SerializeField] private bool _isStationary;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _ledgeDetector,
        _wallDetector;
    [SerializeField] private LayerMask _whatIsGround,
        _whatIsWall;
    [SerializeField] private EnemyState _enemyState;

    void Update() {
        if (_isStationary)
            return;
        else {
            if (_enemyState.GetState() == "Walking") {
                _moveSpeed = 3f;
                RaycastHit2D _ledgeDetection = Physics2D.Raycast(_ledgeDetector.transform.position, -Vector2.up, .7f, _whatIsGround);
                RaycastHit2D _wallDetection = Physics2D.Raycast(_wallDetector.transform.position, Vector2.right, .7f, _whatIsWall);

                transform.Translate(Vector2.right * _moveSpeed * Time.deltaTime); // movement

                if (_ledgeDetection.collider == null || _wallDetection.collider != null)
                    transform.Rotate(0f, 180f, 0f);
            }
            else {
                _moveSpeed = 0f;
            }
        }
    }

    public void SetMovementSpeed(float _moveSpeed) { this._moveSpeed = _moveSpeed; }

}
