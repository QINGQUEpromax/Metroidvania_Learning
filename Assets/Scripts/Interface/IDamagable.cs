using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public void TakeDamage(float damage,float elementDamage,ElementType element,float duration, Transform damageSource);
}
