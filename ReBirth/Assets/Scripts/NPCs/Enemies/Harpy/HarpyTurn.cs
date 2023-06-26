using UnityEngine;
using Pathfinding;

public class HarpyTurn : MonoBehaviour {

    [SerializeField] private AIPath _aiPath;

    void Update() {
        if (_aiPath.desiredVelocity.x >= 0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (_aiPath.desiredVelocity.x <= 0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }
}
