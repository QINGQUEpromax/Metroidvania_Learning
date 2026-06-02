using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_VFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header(" ‹…ÀÃÿ–ß")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();    
        originalMaterial = sr.material;
    }

    public void PlayerOnDamageVfx()
    {
        if(onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);

       onDamageVfxCoroutine =  StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
}
