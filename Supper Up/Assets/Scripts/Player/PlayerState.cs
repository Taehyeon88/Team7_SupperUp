using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerController playerController;
    protected PlayerAnimationController animationController;

    //�ʱ�ȭ�޼���
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
        if (stateMachine.currentState == null) return;

        switch (stateMachine.currentState)
        {
            case MoveState:
                if (Input.GetKeyDown(KeyCode.Space)) stateMachine.TransitionToState(JumpingState.GetInstance());     //��������
                else if (playerController.CheckClimbing()) stateMachine.TransitionToState(ClimbingState.GetInstance());   //��������
                else if (playerController.CheckDistance() > 30f && !playerController.isFalling)                      //���ϻ���
                {
                    stateMachine.TransitionToState(FallingState.GetInstance());
                }
                break;
            case JumpingState:
                if (playerController.isLanding) stateMachine.TransitionToState(LandingState.GetInstance());          //��������
                else if (playerController.CheckClimbing()) stateMachine.TransitionToState(ClimbingState.GetInstance());   //��������
                break;
            case FallingState:
                if (playerController.CheckDistance() < 2.2f && playerController.isFalling)                           //���۳��ϻ���
                {
                    stateMachine.TransitionToState(SupperLandingState.GetInstance());
                }
                break;
            case LandingState:
                if (!playerController.isLanding) stateMachine.TransitionToState(MoveState.GetInstance());            //�̵�����
                break;
            case SupperLandingState:
                if (!playerController.isLanding) stateMachine.TransitionToState(MoveState.GetInstance());            //�̵�����
                break;
            case ClimbingState:                                                                                      
                if (!playerController.isClimbing) stateMachine.TransitionToState(MoveState.GetInstance());           //�̵�����
                break;
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

    public override void Enter()
    {
        playerController.isFalling = true; 
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

public class LandingState : PlayerState
{
    private static LandingState instance = new LandingState();
    private LandingState() { }
    public static LandingState GetInstance() { return instance; }

    public override void Enter()
    {

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

    public override void Enter()
    {
        playerController.isClimbing = true;
        playerController.StartClimbing();
    }
    public override void Update()
    {
        playerController.EndClimbing();
        CheckTransitions();
    }
    public override void Exit() { playerController.ResetVelocity(); }
}

