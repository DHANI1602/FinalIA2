using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePack : PowerUps, IDecorator
{
    public void Execute()
    {
        EventManager.Trigger("GainLife", 1);
        Destroy(this.gameObject);
    }
}
