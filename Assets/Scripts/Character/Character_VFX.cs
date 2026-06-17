using System.Collections;
using UnityEngine;

public class Character_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Character character;

    [Header("受伤特效")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;

    [Header("打击敌方特效")]
    public GameObject hitVfx;
    public GameObject onCritHitVfx;
    public Transform vfxCreatedPos;//随机生成中心
    public Vector2 leftBottom;//左下限制
    public Vector2 rightUp;//右上限制

    [Header("受到元素攻击特效")]
    [SerializeField] private Color chillVfx = Color.cyan;
    [SerializeField] private Color burnVfx = Color.red;
    [SerializeField] private Color electrifyVfx = Color.yellow;
    private Color originalColor;
    private Color hitVfxColor;
    
    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        character = GetComponent<Character>();
        originalMaterial = sr.material;
    }

    //元素伤害特效
    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayStatusVfxCo(duration, chillVfx));
        if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVfxCo(duration, burnVfx));
        if (element == ElementType.Lightning)
            StartCoroutine(PlayStatusVfxCo(duration, electrifyVfx));
    }

    public void StopAllVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
    {
        float tickInterval = .25f;
        float timer = 0;

        Color lightColor = effectColor * 1.1f;
        Color darkColor = effectColor * .9f;
        bool toggle = false;

        while(timer < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);

            timer = timer + tickInterval;
        }

        sr.color = Color.white;
    }

    //生成打击特效
    public void CreateHitVfx(bool isCrit)
    {
        if (hitVfx == null || onCritHitVfx == null)
            return;

        GameObject hitPrefab = isCrit ? onCritHitVfx : hitVfx;

        Vector3 offsetPos = new Vector3(Random.Range(leftBottom.x, rightUp.x), Random.Range(leftBottom.y, rightUp.y), 0);
        GameObject vfx = Instantiate(hitPrefab, vfxCreatedPos.position + offsetPos, Quaternion.identity);

        originalColor = vfx.GetComponent<SpriteRenderer>().color;
        vfx.GetComponent<SpriteRenderer>().color = hitVfxColor != default ? hitVfxColor : originalColor;

        if (character.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }

    //元素特效
    public void UpdateHitColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.None:
                hitVfxColor = default;
                break;
            case ElementType.Fire:
                hitVfxColor = burnVfx;           
                break;
            case ElementType.Ice:
                hitVfxColor = chillVfx;
                break;
        }
    }

    //受伤特效
    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);

        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
}
