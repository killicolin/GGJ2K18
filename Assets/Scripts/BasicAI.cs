using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : AIController
{
    void Start()
    {
        base.Start();
        anim.runtimeAnimatorController = Resources.Load("PrisonnerMovement") as RuntimeAnimatorController;
    }
}
