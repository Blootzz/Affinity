using UnityEngine;

public class QuickAnimatorController : MonoBehaviour
{
    Animator animator;
    [SerializeField] AnimationClip animationToPlay;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        animator.Play(animationToPlay.name);
    }
}
