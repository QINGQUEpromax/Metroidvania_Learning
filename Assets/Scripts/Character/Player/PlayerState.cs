public abstract class PlayerState : EntityState
{
    protected Player player;
    protected Player_SkillManager skillManager;

    public PlayerState(Player player, StateMachine stateMachine, string stateName) : base(stateMachine, stateName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        stats = player.stats;
        input = player.inputActions;
        skillManager = player.skillManager;
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skillManager.dash.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }

        if (input.Player.UltimateSpell.WasPressedThisFrame() && skillManager.domainExpansion.CanUseSkill())
        {
            if (skillManager.domainExpansion.InstantDomain())
                skillManager.domainExpansion.CreateDomain();
            else
                stateMachine.ChangeState(player.domainExpansionState);

            skillManager.domainExpansion.SetSkillOnCooldown();
        }
    }

    private bool CanDash()
    {
        if (skillManager.dash.CanUseSkill() == false)
            return false;

        if (player.isOnWall || stateMachine.currentState == player.dashState)
            return false;

        if (stateMachine.currentState == player.dashState || stateMachine.currentState == player.domainExpansionState)
            return false;

        return true;
    }

    protected override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.velocity.y);
    }
}
