using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Heal on doing damage", fileName = "Item effect data - Heal on doing phys damage")]
public class ItemEffect_HealOnDoingDamage : ItemEffect_DataSO
{
    [SerializeField] private float percentHealedonAttack = .2f;

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.playerCombat.OnDoingPhysicalDamage += HealonDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.playerCombat.OnDoingPhysicalDamage -= HealonDoingDamage;
        player = null;
    }

    private void HealonDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage * percentHealedonAttack);
    }
}
