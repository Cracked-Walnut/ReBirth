using UnityEngine;
/*
Purpose:
    To handle animation related logic for the Ghoul enemy model.
Last Edited:
    1-05-23.
*/
public class EnemyAnimation : MonoBehaviour {

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Animator GetAnimator() { return _animator; }

}
