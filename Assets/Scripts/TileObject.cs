using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    DESTRUCTIBLE,
    FAILER,
    PASSER
}

public class TileObject : MonoBehaviour
{
    [SerializeField] private ObjectType objectType;

    private void Start()
    {
        // in initial testing this has to be in start to game manager can setup first
        AddOurselfToGameManager();
    }

    public void ReceiveFire()
    {
        switch (objectType)
        {
            case ObjectType.DESTRUCTIBLE:
                Debug.Log("destructible was hit");

                break;
            case ObjectType.FAILER:
                Debug.Log("failer was hit");

                break;
            case ObjectType.PASSER:
                Debug.Log("passer was hit");

                break;
            default:
                Debug.LogWarning("Unknown objecttype");
                break;
        }
    }

    private void AddOurselfToGameManager()
    {
        switch (objectType)
        {
            case ObjectType.DESTRUCTIBLE:
                GameManager.instance.AddToDestructibleObjects(this.gameObject);
                break;
            case ObjectType.FAILER:
                GameManager.instance.AddToFailerObjects(this.gameObject);
                break;
            case ObjectType.PASSER:
                GameManager.instance.AddToPasserObjects(this.gameObject);
                break;
            default:
                Debug.LogWarning("Unknown objecttype");
                break;
        }
    }
}
