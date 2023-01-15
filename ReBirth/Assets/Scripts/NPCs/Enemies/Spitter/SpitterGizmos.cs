using UnityEngine;

/*
Purpose:
    In-Editor gizmos related to Spitter.
Last Edited:
    01-14-23.
*/
public class SpitterGizmos : MonoBehaviour {

    [SerializeField] private SpitterAttack _spitterAttack;
    [SerializeField] private bool _drawAttackRange;

    // Doesn't work. Incorrect arguments for DrawLine
    // void OnDrawGizmosSelected() {
    //     if (_drawAttackRange)
    //         Gizmos.DrawLine(_spitterAttack.GetAttackPoint().position, _spitterAttack.GetAttackRange());
    // }    

}
