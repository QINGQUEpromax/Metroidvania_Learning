using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Character character;
    private Character_VFX vfx;
    private ElementType currentEffect = ElementType.None;

    private void Awake()
    {
        character = GetComponent<Character>();
        vfx = GetComponent<Character_VFX>();
    }
    public void ApplyChilledEffect(float duration,float slowMultiplier)
    {
        character.SlowDownEntity(duration, slowMultiplier);

        StartCoroutine(ChilledEffectCo(duration));
    }

    private IEnumerator ChilledEffectCo(float duration)
    {
        currentEffect = ElementType.Ice;
        vfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }
    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }
}
