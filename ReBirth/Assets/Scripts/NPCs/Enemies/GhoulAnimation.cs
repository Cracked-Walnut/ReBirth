using UnityEngine;
/*
Purpose:
    To handle animation related logic for the Ghoul enemy model.
Last Edited:
    11-26-22.
*/
public class GhoulAnimation : MonoBehaviour {

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Animator GetAnimator() { return _animator; }

}
