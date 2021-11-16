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
        base.StartRun();
        magnetCollider.SetActive(false);
    }

    protected override void AfterDisable()
    {
        base.AfterDisable();
        magnetCollider.SetActive(false);
    }
}
