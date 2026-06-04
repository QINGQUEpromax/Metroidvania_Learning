using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMiniHealthBar : MonoBehaviour
{
    private Character character;

    private void Awake()
    {
        character = GetComponentInParent<Character>();  
    }
    private void OnEnable()
    {
        character.OnFlipped += HandleFlip;
    }

    private void OnDisable()
    {
        character.OnFlipped -= HandleFlip;
    }

    private void HandleFlip() => transform.rotation = Quaternion.identity;
}
