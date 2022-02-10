using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoPowerUp : MonoBehaviour, IDecorator
{
    public void Execute()
    {
        MementoManager.instance.RewindTime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            Destroy(gameObject);
        }
    }
}
