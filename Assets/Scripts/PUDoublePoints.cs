using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUDoublePoints : PowerUp
{

    public override void Use()
    {
        throw new System.NotImplementedException();
    }

    protected override void AfterDisable()
    {
        throw new System.NotImplementedException();
    }

    public override void StartRun()
    {
        throw new System.NotImplementedException();
    }

    public override void Collect()
    {
        if (!inUse)
        {
            Use();
        }
    }
}
