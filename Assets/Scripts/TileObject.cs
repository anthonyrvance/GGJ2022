using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    DESTRUCTIBLE,
    FAILER,
    PASSER,
    COLLATERAL
}

public class TileObject : MonoBehaviour
{
    [SerializeField] private ObjectType objectType;
    private CollateralFire parentCollateral;
    private bool areWeOnFire;

    public bool AreWeOnFire
    {
        get { return areWeOnFire; }
        set { areWeOnFire = value; }
    }

    private void Start()
    {
        areWeOnFire = false;
        parentCollateral = GetComponentInParent<CollateralFire>();
        // in initial testing this has to be in start to game manager can setup first
        AddOurselfToGameManager();
    }

    public void ReceiveFire()
    {
        switch (objectType)
        {
            case ObjectType.DESTRUCTIBLE:
                StartCoroutine(DieAfterTime());
                break;
            case ObjectType.FAILER:
                GameManager.instance.FailLevel();
                break;
            case ObjectType.PASSER:
                GameManager.instance.PassLevel();
                break;
            case ObjectType.COLLATERAL:
                if (!areWeOnFire)
                    parentCollateral.SpreadFire(this);
                break;
            default:
                Debug.LogWarning("Unknown objecttype");
                break;
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Die", true);
        }
    }

    private IEnumerator DieAfterTime()
    {
        yield return new WaitForSeconds(2.0f);
        //Destroy(this.gameObject); // cant do this because gamemananger loses track
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
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
            case ObjectType.COLLATERAL:
                GameManager.instance.AddToCollateralObjects(this.gameObject);
                break;
            default:
                Debug.LogWarning("Unknown objecttype");
                break;
        }
    }
}
