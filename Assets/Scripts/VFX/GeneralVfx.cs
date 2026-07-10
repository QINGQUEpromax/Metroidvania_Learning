using System.Collections;
using UnityEngine;

public class GeneralVfx : MonoBehaviour
{
    public SpriteRenderer sr { get; private set; }


    [Header("冲刺视觉效果")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1f;

    private Color targetColor;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (canFade)
            StartCoroutine(FadeCo());
    }

    private void FixedUpdate()
    {
        if (targetColor.a < 0)
            Destroy(gameObject);
    }
    private IEnumerator FadeCo()
    {
        targetColor = Color.white;
        while (targetColor.a > 0)
        {
            targetColor.a -= fadeSpeed * Time.deltaTime;
            sr.color = targetColor;
            yield return null;
        }

        sr.color = targetColor;
    }

    //特效销毁动画事件
    public void DestroyVFX()
    {
        Destroy(gameObject);
    }

}
