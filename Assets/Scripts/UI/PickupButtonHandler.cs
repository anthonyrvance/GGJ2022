using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupButtonHandler : MonoBehaviour
{
    public delegate void PickupButton();
    public static event PickupButton PickupButtonPressed;

    public void PickupButtonPress()
    {
        PickupButtonPressed();
    }
}
