using UnityEngine;
using Pathfinding;

/*
Purpose:
    To find and attack Player.
Last Edited:
    06-25-23.
*/
public class HarpyAttack : MonoBehaviour {
    [SerializeField] private Transform player; // Reference to the player object
    [SerializeField] private float movementSpeed; // Speed at which the monster moves towards the player
    [SerializeField] private float bounceForce; // Force applied to the monster when colliding with the player

    private Rigidbody2D rb; // Reference to the Rigidbody2D component of the monster
    private AIPath aiPath; // Reference to the AIPath component of the monster for pathfinding

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
    }

    private void Update() {
        if (player != null) {
            // Calculate the direction from the monster to the player and normalize it
            Vector2 direction = (player.position - transform.position).normalized;
            
            // Apply the direction multiplied by the movement speed as velocity to the monster
            rb.velocity = direction * movementSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // Calculate the bounce direction by subtracting the player's position from the monster's position and normalize it
            Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            
            // Apply an impulse force to the monster in the bounce direction
            rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
        }
    }
}
