using UnityEngine;

public class KillPlayer : MonoBehaviour {

    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _respawnLocations;

    void OnCollisionEnter2D(Collision2D _collision2D) {

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (_collision2D.gameObject.tag == "Player") {
            _player.transform.position = _respawnLocations[0].transform.position;
        }
    }
}
