using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Snowcone : Pickup
{
    bool hasBeenPickedUp = false;

    protected override void OnPickup()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public override void UsePickup(Player player)
    {
        // yikes, dont look
        player.UpdateAnimation(1);
        GameManager.instance.ReceiveNewTimeToFire(3);
    }
}
