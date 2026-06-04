using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVfx : MonoBehaviour
{
    //销毁特效事件
    public void DestroyVfx()
    {
        Destroy(gameObject);
    }
}
