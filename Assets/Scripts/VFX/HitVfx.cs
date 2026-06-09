using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVfx : MonoBehaviour
{
    public SpriteRenderer sr {  get; private set; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    //销毁特效事件
    public void DestroyVfx()
    {
        Destroy(gameObject);
    }

 
}
