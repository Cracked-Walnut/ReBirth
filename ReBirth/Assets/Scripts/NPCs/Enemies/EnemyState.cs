using UnityEngine;

public class EnemyState : MonoBehaviour {

    [SerializeField] private EnemyAnimation _enemyAnimation;
    private string _state;

    void Start() => _state = "Spawn_Idle";

    void Update() {
        switch (_state) {
            case "Spawn":
                _enemyAnimation.ResetAnimationTriggers();
                _enemyAnimation.GetAnimator().SetTrigger("Spawn");
                break;
            case "Walking":
                _enemyAnimation.ResetAnimationTriggers();
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
        Debug.Log(_state);
    }

    public string GetState() { return _state; }
    public void SetState(string _newState) { _state = _newState; }
}
