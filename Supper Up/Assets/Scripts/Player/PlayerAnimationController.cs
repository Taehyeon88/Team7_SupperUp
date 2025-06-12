using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public PlayerStateMachine playerStateMachine;

    private const string PARAM_IS_MOVING = "IsMoving";
    private const string PARAM_IS_JUMPING = "IsJumping";
    private const string PARAM_IS_LANDING = "IsLanding";
    private const string PARAM_IS_FALLING = "IsFalling";
    private const string PARAM_IS_SUPPERLANDING = "IsSupperLanding";
    private const string PARAM_IS_Climbing = "IsClimbing";


    public void UpdateAnimationState(PlayerState currentState, PlayerState newState)
    {
        if (newState == null) return;

        ClearAllParameter();

        switch (newState)
        {
            case MoveState:
                animator.SetBool(PARAM_IS_LANDING, false);
                break;
            case JumpingState:
                if (currentState is LandingState || currentState is MoveState) ChangeAnimation("Jumping");
                break;
            case FallingState:
                if (currentState is MoveState) ChangeAnimation_Smooth("Falling", 0.2f);
                else if(currentState is JumpingState) animator.SetBool(PARAM_IS_FALLING, true);
                break;
            case LandingState:
                if (currentState is JumpingState || currentState is MoveState) animator.SetBool(PARAM_IS_LANDING, true); //자동실행
                break;
            case SupperLandingState:
                if (currentState is FallingState) animator.SetTrigger(PARAM_IS_SUPPERLANDING);
                break;
            case ClimbingState:
                if (currentState is MoveState) ChangeAnimation("Climbing");
                else if (currentState is JumpingState) ChangeAnimation_Smooth("Climbing", 0.1f);
                else if (currentState is FallingState) ChangeAnimation("Climbing");
                else if (currentState is LandingState) ChangeAnimation("Climbing");
                break;
        }
    }

    private void ChangeAnimation(string stateName)
    {
        animator.CrossFade(stateName, 0.01f, 0);
    }

    private void ChangeAnimation_Smooth(string stateName, float value)
    {
        animator.CrossFade(stateName, value, 0);
    }


    private void ClearAllParameter()
    {
        animator.SetBool(PARAM_IS_FALLING, false);
    }
}
