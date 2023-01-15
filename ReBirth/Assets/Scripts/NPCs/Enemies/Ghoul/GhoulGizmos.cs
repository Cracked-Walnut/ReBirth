using UnityEngine;

/*
Purpose:
    In-Editor gizmos related to Ghoul.
Last Edited:
    01-14-23.
*/
public class GhoulGizmos : MonoBehaviour {

    [SerializeField] private GhoulAttack _ghoulAttack;
    [SerializeField] private bool _drawAttackRange;

    void OnDrawGizmosSelected() {

        Gizmos.color = Color.red;

        if (_drawAttackRange)
            Gizmos.DrawWireSphere(_ghoulAttack.GetAttackPoint().position, _ghoulAttack.GetAttackRange());

        // Gizmos.color = Color.white;
        // Gizmos.DrawLine(_playerDetector.transform.position, new Vector3(_playerDetector.transform.position.x + _visionRange, _playerDetector.transform.position.y, 0f));
        // Gizmos.DrawLine(_playerDetector.transform.position, new Vector3(_playerDetector.transform.position.x - _visionRange, _playerDetector.transform.position.y, 0f));
    }

}
