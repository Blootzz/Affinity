using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerBaseState currentState;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentState = new PlayerStateIdle(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
