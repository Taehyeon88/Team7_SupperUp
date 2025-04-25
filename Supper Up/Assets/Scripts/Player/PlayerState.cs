using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine sM;
    protected PlayerController pC;
    protected MovementController mC;
    protected PlayerAnimationController aniC;

    //초기화메서드
    public void Init(PlayerStateMachine stateMachine)
    {
        this.sM = stateMachine;
        this.pC = stateMachine.playerController;
        this.mC = stateMachine.movementController;
        this.aniC = stateMachine.GetComponent<PlayerAnimationController>();
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    protected void CheckTransitions()
    {
        if (sM.currentState == null) return;

        switch (sM.currentState)
        {
            case MoveState:
                if (Input.GetKeyDown(KeyCode.Space) && pC.IsGrounded()) sM.TransitionToState(JumpingState.GetInstance());     //점프상태
                else if (mC.CheckClimbing()) sM.TransitionToState(ClimbingState.GetInstance());                               //파쿠르상태
                else if (pC.CheckDistance() > 25f && !pC.isFalling && !pC.IsGrounded() && pC.CheckFalling(6))                 //낙하상태
                {
                    sM.TransitionToState(FallingState.GetInstance());
                }
                else if (pC.isHightLanding) sM.TransitionToState(LandingState.GetInstance());                 //착지상태
                break;
            case JumpingState:
                if (pC.isLanding) sM.TransitionToState(LandingState.GetInstance());                           //착지상태
                else if (mC.CheckClimbing()) sM.TransitionToState(ClimbingState.GetInstance());               //파쿠르상태
                else if (pC.CheckDistance() > 25f && !pC.isFalling && !pC.IsGrounded() && pC.CheckFalling(6)) //낙하상태
                {
                    sM.TransitionToState(FallingState.GetInstance());
                }
                break;
            case FallingState:
                if (pC.CheckDistance() < 2.2f && pC.isFalling)                           //슈퍼낙하상태
                {
                    sM.TransitionToState(SupperLandingState.GetInstance());
                }
                break;
            case LandingState:
                if (pC.isJumping && !pC.isLanding) sM.TransitionToState(MoveState.GetInstance());                 //점프낙하 -> 이동상태
                else if(!pC.isJumping && !pC.isHightLanding) sM.TransitionToState(MoveState.GetInstance());       //일반낙하 -> 이동상태
                break;
            case SupperLandingState:
                sM.TransitionToState(MoveState.GetInstance());                                             //이동상태
                break;
            case ClimbingState:                                                                                      
                if (!mC.isClimbing) sM.TransitionToState(MoveState.GetInstance());                         //이동상태
                break;
        }
    }
}

public class MoveState : PlayerState
{
    private static MoveState instance = new MoveState();
    private MoveState() { }
    public static MoveState GetInstance() { return instance; }

    public override void Enter()
    {
        pC.isJumping = false;     //점프초기화용
    }
    public override void Update()
    {
        pC.Rotate(true);
        pC.CheckLanding();
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        pC.Move();
    }
}

public class JumpingState : PlayerState
{
    private static JumpingState instance = new JumpingState();
    private JumpingState() { }
    public static JumpingState GetInstance() { return instance; }

    public override void Enter()
    {
        pC.Jumping();
        pC.isJumping = true;
    }
    public override void Update()
    {
        pC.Rotate(false);
        pC.CheckLanding();
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        pC.Move();
    }
}

public class FallingState : PlayerState
{
    private static FallingState instance = new FallingState();
    private FallingState() { }
    public static FallingState GetInstance() { return instance; }

    public override void Enter()
    {
        pC.isFalling = true; 
    }
    public override void Update()
    {
        pC.Rotate(false);
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        pC.Move();
    }
}

public class LandingState : PlayerState
{
    private static LandingState instance = new LandingState();
    private LandingState() { }
    public static LandingState GetInstance() { return instance; }

    public override void Enter()
    {
        pC.Landing(false);
    }
    public override void Update()
    {
        pC.CheckLanding();
        CheckTransitions();
    }
    public override void Exit() 
    { 
        pC.ResetVelocity();
        pC.isJumping = false;
    }
}

public class SupperLandingState : PlayerState
{
    private static SupperLandingState instance = new SupperLandingState();
    private SupperLandingState() { }
    public static SupperLandingState GetInstance() { return instance; }

    public override void Enter()
    {
        pC.Landing(true);
    }
    public override void Update()
    {
        CheckTransitions();
    }
    public override void Exit() 
    { 
        pC.ResetVelocity();
    }
}

public class ClimbingState : PlayerState
{
    private static ClimbingState instance = new ClimbingState();
    private ClimbingState() { }
    public static ClimbingState GetInstance() { return instance; }

    public override void Enter()
    {
        mC.isClimbing = true;
        mC.StartClimbing();
    }
    public override void Update()
    {
        mC.EndClimbing();
        CheckTransitions();
    }
    public override void Exit() 
    { 
        pC.ResetVelocity();
    }
}

