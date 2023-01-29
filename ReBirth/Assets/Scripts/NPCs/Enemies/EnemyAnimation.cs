using UnityEngine;
/*
Purpose:
    To handle animation related logic for the Ghoul enemy model.
Last Edited:
    01-13-23.
*/
public class EnemyAnimation : MonoBehaviour {

    [SerializeField] private string[] _triggers;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Animator GetAnimator() { return _animator; }

    public void ResetAnimationTriggers() {
        for (int i = 0; i < _triggers.Length; i++)
            _animator.ResetTrigger(_triggers[i]);
    }
}
