using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    protected abstract void OnPickup();

    public abstract void UsePickup(Player player);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.CurrentPickup = this;
            OnPickup();
        }
    }
}
