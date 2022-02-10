using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBack : PowerUps, IDecorator
{
    public void Execute()
    {
        MementoManager.instance.RewindTime();
        Destroy(this.gameObject);
    }
}
