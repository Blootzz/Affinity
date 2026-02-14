using UnityEngine;

public class QuickAnimatorController : MonoBehaviour
{
    Animator animator;
    [SerializeField] AnimationClip animationToPlay;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // possibly called by UnityEvent
    public void LISTENER_PlayAnimation()
    {
        animator.Play(animationToPlay.name);
    }
}
