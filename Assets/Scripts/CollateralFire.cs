using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollateralFire : MonoBehaviour
{
    [SerializeField] private List<TileObject> collateralChildren;
    [SerializeField] private GameObject firePrefab;

    public void SpreadFire(TileObject childOrigin)
    {
        childOrigin.AreWeOnFire = true;
        GameObject go = Instantiate(firePrefab, childOrigin.gameObject.transform.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneAt(1));

        StartCoroutine(StartSpreading(childOrigin));
    }

    private IEnumerator StartSpreading(TileObject fireOrigin)
    {
        yield return new WaitForSeconds(1.0f);
        int spotsLeftToPutAFire = collateralChildren.Count - 1; // origin already started

        while (spotsLeftToPutAFire > 0)
        {
            // "reset"
            float shortestDistance = Mathf.Infinity;
            TileObject winner = new TileObject(), winner2, winner3;

            foreach (TileObject t in collateralChildren)
            {
                if (t.AreWeOnFire) // dont consider this tile
                    continue;

                //Instantiate(firePrefab, t);
                float distanceToTest = Vector3.Distance(fireOrigin.transform.position, t.transform.position);
                if (distanceToTest < shortestDistance)
                {
                    shortestDistance = distanceToTest;
                    winner = t;
                }
            }
            winner.AreWeOnFire = true;
            GameObject go = Instantiate(firePrefab, winner.transform.position, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneAt(1));

            --spotsLeftToPutAFire;
            yield return new WaitForSeconds(1.0f);
        }

    }
}
