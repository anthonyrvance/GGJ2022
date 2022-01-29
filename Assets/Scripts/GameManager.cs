using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Movement Stuff")]
    [SerializeField] private TextMeshProUGUI movesLeftBeforeFireText;
    [SerializeField] private string movesLeftBeforeFireString;
    [SerializeField] private int numMovesOnReset;
    [SerializeField] private int currentMovesLeft;

    [Header("Tile Objects")]
    [SerializeField] private List<GameObject> destructibleObjects;
    [SerializeField] private List<GameObject> failerObjects;
    [SerializeField] private List<GameObject> passerObjects;
    [SerializeField] private GameObject firePrefab;

    #region "setters"
    public void AddToDestructibleObjects(GameObject incGO)
    {
        destructibleObjects.Add(incGO);
    }
    public void AddToFailerObjects(GameObject incGO)
    {
        failerObjects.Add(incGO);
    }
    public void AddToPasserObjects(GameObject incGO)
    {
        passerObjects.Add(incGO);
    }
    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        destructibleObjects = new List<GameObject>();
        failerObjects = new List<GameObject>();
        passerObjects = new List<GameObject>();

        Init();
    }

    private void Init()
    {
        currentMovesLeft = numMovesOnReset;
        UpdateMoveText();

        destructibleObjects.Clear();
        failerObjects.Clear();
        passerObjects.Clear();
    }

    #region Subscriptions
    private void OnEnable()
    {
        Player.OnMove += PlayerMoved;
        SceneManagement.SceneUnloading += Init;
    }

    private void OnDisable()
    {
        Player.OnMove -= PlayerMoved;
        SceneManagement.SceneUnloading -= Init;
    }
    #endregion

    #region Pass/Fail/Return
    public void FailLevel()
    {
        SceneManagement.instance.ReloadCurrent();
    }

    public void PassLevel()
    {
        SceneManagement.instance.GoToNextScene();
    }

    public void GoBackToMainMenu()
    {
        SceneManagement.instance.GoBackToMainMenu();
    }
    #endregion

    #region Movement
    private void PlayerMoved(Vector3 playersNewPos)
    {
        // check moves count
        --currentMovesLeft;
        if (currentMovesLeft <= 0)
        {
            // do fire thing and check nearby tiles (somehow...)((just use vectors?)
            CheckForNearbyObjects(playersNewPos);
            // maybe do a coroutine here to do some fire projectiles and after that update text
            currentMovesLeft = numMovesOnReset;
            UpdateMoveText(); // TEMPORARY PLEASEEE
        }
        else
        {
            UpdateMoveText();
        }
    }

    private void UpdateMoveText()
    {
        movesLeftBeforeFireText.text = movesLeftBeforeFireString + " " + currentMovesLeft;
    }
    #endregion

    #region Tile objects
    private void CheckForNearbyObjects(Vector3 origin)
    {
        // cardinal directions for now?
        Vector3 northTile = origin + new Vector3(0.0f, 1.0f, 0.0f);
        CompareObjectsInListToPosition(destructibleObjects, northTile);
        CompareObjectsInListToPosition(failerObjects, northTile);
        CompareObjectsInListToPosition(passerObjects, northTile);

        Vector3 eastTile = origin + new Vector3(1.0f, 0.0f, 0.0f);
        CompareObjectsInListToPosition(destructibleObjects, eastTile);
        CompareObjectsInListToPosition(failerObjects, eastTile);
        CompareObjectsInListToPosition(passerObjects, eastTile);

        Vector3 southTile = origin + new Vector3(0.0f, -1.0f, 0.0f);
        CompareObjectsInListToPosition(destructibleObjects, southTile);
        CompareObjectsInListToPosition(failerObjects, southTile);
        CompareObjectsInListToPosition(passerObjects, southTile);

        Vector3 westTile = origin + new Vector3(-1.0f, 0.0f, 0.0f);
        CompareObjectsInListToPosition(destructibleObjects, westTile);
        CompareObjectsInListToPosition(failerObjects, westTile);
        CompareObjectsInListToPosition(passerObjects, westTile);

        ShootOutFire(origin, northTile, eastTile, southTile, westTile);
    }

    // an ugly copy of member list
    private void CompareObjectsInListToPosition(List<GameObject> list, Vector3 tileToCheck)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(list[i].transform.position, tileToCheck) <= 0.1f)
            {
                Debug.Log("the fire is going to hit something on an adjacent tile...");
                Debug.Log(Vector3.Distance(list[i].transform.position, tileToCheck));
                if (list[i].GetComponent<TileObject>() != null)
                {
                    list[i].GetComponent<TileObject>().ReceiveFire();
                }
                else
                {
                    Debug.LogError("the fire hit a tile without a tileobject script... weird");
                }
            }
        }
    }

    // giving up on clean-ish code starting now
    private void ShootOutFire(Vector3 center, Vector3 north, Vector3 east, Vector3 south, Vector3 west)
    {
        GameObject northFire = Instantiate(firePrefab, center, Quaternion.identity);
        GameObject eastFire = Instantiate(firePrefab, center, Quaternion.identity);
        GameObject southFire = Instantiate(firePrefab, center, Quaternion.identity);
        GameObject westFire = Instantiate(firePrefab, center, Quaternion.identity);

        StartCoroutine(ScaleUp(northFire, north));
        StartCoroutine(ScaleUp(eastFire, east));
        StartCoroutine(ScaleUp(southFire, south));
        StartCoroutine(ScaleUp(westFire, west));
    }

    private IEnumerator ScaleUp(GameObject fire, Vector3 tile)
    {
        float elapsedTime = 0.0f;
        fire.transform.localScale = new Vector3(0, 0);

        while (elapsedTime < 1.0f)
        {
            float val = Mathf.Lerp(0, 1, elapsedTime / 1);
            fire.transform.localScale = new Vector3(val, val);
            fire.transform.position = Vector3.MoveTowards(fire.transform.position, tile, 3.5f * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(fire.gameObject);
    }
    #endregion
}
