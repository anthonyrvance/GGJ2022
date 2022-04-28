using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementButtonHandler : MonoBehaviour
{
    public delegate void MovementButton(int direction);
    public static event MovementButton MovementButtonPressed;
    
    public void MovementButtonPress(int incomingDirection)
    {
        MovementButtonPressed(incomingDirection);
    }
}
