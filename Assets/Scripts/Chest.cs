using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour , IDamage
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Animator anim => GetComponent<Animator>();
    private Character_VFX vfx => GetComponent<Character_VFX>();

    [Header("´ò¿ªÏ¸½Ú")]
    [SerializeField] private Vector2 openPower;

    public void TakeDamage(float damage, float duration, Transform damageSource)
    {
        vfx.PlayOnDamageVfx();
        anim.SetBool("open",true);
        rb.velocity = openPower;
        rb.angularVelocity = Random.Range(-200f, 200f);

    }

}
