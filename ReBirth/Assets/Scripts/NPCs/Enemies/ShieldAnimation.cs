using UnityEngine;
/*
Purpose:
    To handle animation related shield activation for non-Protector type enemies.
Last Edited:
    01-29-23.
*/
public class ShieldAnimation : MonoBehaviour {

    [SerializeField] private string[] _triggers;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Animator GetAnimator() { return _animator; }

    public void ResetAnimationTriggers() {
        for (int i = 0; i < _triggers.Length; i++)
            _animator.ResetTrigger(_triggers[i]);
    }
}
