using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUMagnet : PowerUp
{
    [SerializeField] private GameObject magnetCollider;
    public override void Use()
    {
        inUse = true;
        magnetCollider.SetActive(true);
        StartCoroutine(nameof(Disable));
    }

    public override void StartRun()
    {
        magnetCollider.SetActive(false);
    }

    public override void AfterDisable()
    {
        base.AfterDisable();
        magnetCollider.SetActive(false);
    }

    public override void Collect()
    {
        if (!inUse)
        {
            Use();
        }
    }
}
