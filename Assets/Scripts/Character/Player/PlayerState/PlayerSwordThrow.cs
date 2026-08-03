using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordThrow : PlayerState
{

    private Camera mainCamera;

    public PlayerSwordThrow(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillManager.swordThrow.EnableDots(true);

        if (mainCamera != Camera.main)
        {
            mainCamera = Camera.main;
        }
    }

    public override void Update()
    {
        base.Update();

        Vector2 dirToMouse = DirectionToMouse();

        player.SetVelocity(0, rb.velocity.y);
        player.HandleFlip(dirToMouse.x);
        skillManager.swordThrow.PredictTrajectory(dirToMouse);

        if (input.Player.Attack.WasPressedThisFrame())
        {
            anim.SetBool("swordThrowPerformed", true);

            skillManager.swordThrow.EnableDots(false);
            skillManager.swordThrow.ConfirmTrajectory(dirToMouse);
        }

        if (input.Player.RangeAttack.WasReleasedThisFrame() || attackOver) 
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("swordThrowPerformed", false);
        skillManager.swordThrow.EnableDots(false);
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(player.mousePosition);

        Vector2 direction = worldMousePos - playerPos;

        return direction.normalized;
    }
}
