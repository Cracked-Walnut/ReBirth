using UnityEngine;

/*
Purpose:
    To handle animation logic for enemies.
Last Edited:
    01-24-23.
*/
public class EnemyState : MonoBehaviour {

    [SerializeField] private EnemyAnimation _enemyAnimation;
    [SerializeField] private string _state;

    void Awake() => _state = "Walking";

    void Update() {
        switch (_state) {
            case "Spawn":
                // _enemyAnimation.ResetAnimationTriggers();
                _enemyAnimation.GetAnimator().SetTrigger("Spawn");
                break;
            case "Walking":
                // _enemyAnimation.ResetAnimationTriggers();
                _enemyAnimation.GetAnimator().SetTrigger("Walking");
                break;
            case "Hit":
                _enemyAnimation.ResetAnimationTriggers();
                _enemyAnimation.GetAnimator().SetTrigger("Hit");
                break;
            case "Attack":
                _enemyAnimation.ResetAnimationTriggers();
                _enemyAnimation.GetAnimator().SetTrigger("Attack");
                break;
            case "Dead":
                _enemyAnimation.ResetAnimationTriggers();
                _enemyAnimation.GetAnimator().SetTrigger("Dead");
                break;

        }
    }

    public string GetState() { return _state; }
    public void SetState(string _newState) { _state = _newState; }
}
