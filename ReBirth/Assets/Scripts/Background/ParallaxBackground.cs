using UnityEngine;

/*
Purpose:
    To handle 2D moving backgrounds.
Last Edited:
    11-25-22.
*/

public class ParallaxBackground : MonoBehaviour {

    [SerializeField] private Vector2 _parallaxEffectMultiplier; // used to scroll the background when the camera moves
    
    // should the background be infinite horizontally/ vertically?
    [SerializeField] private bool _infiniteHorizontal;
    [SerializeField] private bool _infiniteVertical;

    [SerializeField] private float _parallaxScale;
    
    [SerializeField] private GameObject _camera;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private float _textureUnitSizeX;
    private float _textureUnitSizeY;

    void Start() {
        _cameraTransform = _camera.transform;
        _lastCameraPosition = _cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;

        // Parallax Scale must match your image's Transform X scale
        _textureUnitSizeX = (texture.width * _parallaxScale) / sprite.pixelsPerUnit; 
        _textureUnitSizeY = (texture.height * _parallaxScale) / sprite.pixelsPerUnit; 
    }

    void LateUpdate() {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * _parallaxEffectMultiplier.x, deltaMovement.y * _parallaxEffectMultiplier.y);
        _lastCameraPosition = _cameraTransform.position;


        if (_infiniteHorizontal) {
            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX) {
                float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (_infiniteVertical) {
            if (Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureUnitSizeY) {
                float offsetPositionY = (_cameraTransform.position.y - transform.position.y) % _textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, _cameraTransform.position.y + offsetPositionY);
            }
        }
    }
}
