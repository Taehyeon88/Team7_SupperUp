using System.Collections;
using System.Collections.Generic;
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
                else if (pC.isTrusted) sM.TransitionToState(FallingState.GetInstance());                      //장애물로 일해 밀쳐진 상태
                break;
            case JumpingState:
                if (pC.isLanding) sM.TransitionToState(LandingState.GetInstance());                           //착지상태
                else if (mC.CheckClimbing()) sM.TransitionToState(ClimbingState.GetInstance());               //파쿠르상태
                else if (pC.CheckDistance() > 25f && !pC.isFalling && !pC.IsGrounded() && pC.CheckFalling(6)) //낙하상태
                {
                    sM.TransitionToState(FallingState.GetInstance());
                }
                else if (pC.isTrusted) sM.TransitionToState(FallingState.GetInstance());                      //장애물로 일해 밀쳐진 상태
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
        pC.isHightLanding = false;
        sM.StartCoroutine(WaitLanding());
    }
    public override void Update()
    {
        pC.Rotate(true);
        pC.CheckLanding();
        CheckTransitions();
        if (pC.CheckDistance() > 2.2f && !pC.IsGrounded())
        {
            SoundManager.instance.FadeSound("Walk(stone)", 0f);
            SoundManager.instance.FadeSound("Walk(wood)", 0f);
        }
    }

    public override void FixedUpdate()
    {
        if(!pC.isSuperLanding) pC.Move(true);
    }

    public override void Exit()
    {
        SoundManager.instance.FadeSound("Walk(stone)", 0f);
        SoundManager.instance.FadeSound("Walk(wood)", 0f);
        pC.isOneTime = false;
    }

    IEnumerator WaitLanding()
    {
        yield return new WaitForSeconds(0.7f);
        pC.isSuperLanding = false;
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
        pC.Move(false);
    }
}

public class FallingState : PlayerState
{
    private static FallingState instance = new FallingState();
    private FallingState() { }
    public static FallingState GetInstance() { return instance; }

    public override void Enter()
    {
        if (!pC.isTrusted)
        {
            pC.isFalling = true;
        }
        else sM.StartCoroutine(WaitSuperLanding());
        sM.cameraController.StartCameraShake(0.05f);

        SoundManager.instance.FadeSound("Falling", 1f);
    }
    public override void Update()
    {
        pC.Rotate(false);
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        pC.Move(false);
        if (pC.CheckDistance() < 10f)
        {
            sM.cameraController.ReadyToStopCameraShake();
        }
    }

    public override void Exit()
    {
        sM.cameraController.StopCameraShake();
        pC.isTrusted = false;

        SoundManager.instance.FadeSound("Falling", 0f, true);
    }

    IEnumerator WaitSuperLanding()
    {
        yield return new WaitForSeconds(0.3f);
        pC.isFalling = true;
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
        if (pC.GetGroundTypeString() == "Walk(wood)") SoundManager.instance.PlaySound("Landing(wood)");
        else if (pC.GetGroundTypeString() == "Walk(stone)") SoundManager.instance.PlaySound("Landing(stone)");
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
        pC.isSuperLanding = true;
        if(pC.GetGroundTypeString() == "Walk(wood)") SoundManager.instance.PlaySound("SuperLanding(wood)");
        else if(pC.GetGroundTypeString() == "Walk(stone)") SoundManager.instance.PlaySound("SuperLanding(stone)");
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

