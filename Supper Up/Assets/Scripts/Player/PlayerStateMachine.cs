using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState;
    public PlayerController playerController;
    private PlayerAnimationController animationController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    private void Start()
    {
        TransitionToState(MoveState.GetInstance());
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
    }

    public void TransitionToState(PlayerState newState)
    {
        if (currentState?.GetType() == newState.GetType())
        {
            return;
        }
        currentState?.Exit();

        if (currentState != null)
        {
            animationController.UpdateAnimationState(currentState, newState);
        }

        currentState = newState;

        currentState.Init(this);
        currentState.Enter();

        Debug.Log($"상태전환되는 상태 : {newState.GetType().Name}");
    }
}
