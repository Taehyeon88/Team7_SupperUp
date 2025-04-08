using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerController playerController;
    protected PlayerAnimationController animationController;

    //초기화메서드
    public void Init(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.playerController = stateMachine.playerController;
        this.animationController = stateMachine.GetComponent<PlayerAnimationController>();
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    protected void CheckTransitions()
    {
        if (playerController.IsGrounded())
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.TransitionToState(JumpingState.GetInstance());
            }
            else if(playerController.isLanding)
            {
                stateMachine.TransitionToState(LandingState.GetInstance());
                Debug.Log("된다1");
            }
            else if (!playerController.isFalling && !playerController.isLanding)
            {
                stateMachine.TransitionToState(MoveState.GetInstance());
            }
        }
        else
        {
            if (playerController.CheckDistance() > 30f && !playerController.isFalling)
            {
                stateMachine.TransitionToState(FallingState.GetInstance());
            }
            else if (playerController.CheckDistance() < 2.2f && playerController.isFalling)
            {
                stateMachine.TransitionToState(SupperLandingState.GetInstance());
            }
        }
    }
}

public class MoveState : PlayerState
{
    private static MoveState instance = new MoveState();
    private MoveState() { }
    public static MoveState GetInstance() { return instance; }
    public override void Update()
    {
        playerController.Rotate(true);
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.Move();
    }
}

public class JumpingState : PlayerState
{
    private static JumpingState instance = new JumpingState();
    private JumpingState() { }
    public static JumpingState GetInstance() { return instance; }

    public override void Enter()
    {
        playerController.Jumping();
        playerController.isJumping = true;
    }
    public override void Update()
    {
        playerController.Rotate(false);
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.Move();
    }
}

public class FallingState : PlayerState
{
    private static FallingState instance = new FallingState();
    private FallingState() { }
    public static FallingState GetInstance() { return instance; }

    public override void Enter() { playerController.isFalling = true; }
    public override void Update()
    {
        playerController.Rotate(false);
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.Move();
    }
}

public class LandingState : PlayerState
{
    private static LandingState instance = new LandingState();
    private LandingState() { }
    public static LandingState GetInstance() { return instance; }

    public override void Enter()
    {
        playerController.isLanding = false;
    }
    public override void Update()
    {
        CheckTransitions();
    }
    public override void Exit() { playerController.ResetVelocity(); }
}

public class SupperLandingState : PlayerState
{
    private static SupperLandingState instance = new SupperLandingState();
    private SupperLandingState() { }
    public static SupperLandingState GetInstance() { return instance; }

    public override void Enter()
    {
        playerController.SupperLanding();
    }
    public override void Update()
    {
        CheckTransitions();
    }
    public override void Exit() { playerController.ResetVelocity(); }
}

public class ClimbingState : PlayerState
{
    private static ClimbingState instance = new ClimbingState();
    private ClimbingState() { }
    public static ClimbingState GetInstance() { return instance; }

    public override void Update()
    {
        CheckTransitions();
    }

    public override void FixedUpdate()
    {

    }
    public override void Exit() { playerController.ResetVelocity(); }
}

