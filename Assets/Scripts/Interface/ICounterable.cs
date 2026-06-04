using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICounterable 
{
    public bool canBeCountered { get;  }
    public void HandleCounter();
}
