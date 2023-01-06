using UnityEngine;

/*
Purpose:
    To handle Ghoul movement.
Last Edited:
    01-05-23.
*/
public class GhoulMovement : MonoBehaviour {

    // [SerializeField] private GhoulAnimation _ghoulAnimation;
    [SerializeField] private float _moveSpeed;

    // [SerializeField] private LedgeRange _lr;

    // [SerializeField] private float _visionRange;
    // [SerializeField] private string _AIType;

    // [SerializeField] private GameObject _playerDetector;
    [SerializeField] private GameObject _ledgeDetector, _wallDetector;

    // [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private LayerMask _whatIsGround, _whatIsWall, _whatIsCeiling;

    void Update() {
        RaycastHit2D _ledgeDetection = Physics2D.Raycast(_ledgeDetector.transform.position, -Vector2.up, .7f, _whatIsGround);
        RaycastHit2D _wallDetection = Physics2D.Raycast(_wallDetector.transform.position, Vector2.right, .7f, _whatIsWall);

        transform.Translate(Vector2.right * _moveSpeed * Time.deltaTime); // movement

        if (_ledgeDetection.collider == null || _wallDetection.collider != null)
            transform.Rotate(0f, 180f, 0f);

    }

    public void SetMovementSpeed(float _moveSpeed) { this._moveSpeed = _moveSpeed; }

}
