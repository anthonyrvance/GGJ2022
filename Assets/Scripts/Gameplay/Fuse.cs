using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BurnType
{
    SLOW,
    FAST
}

public class Fuse : MonoBehaviour
{
    [SerializeField] private List<TileObject> fuseChildren;
    [SerializeField] private GameObject firePrefab;

    [SerializeField] private BurnType burnType;
    [SerializeField] private int playerMovesBeforeSlowBurnMoves;
    [SerializeField] private int slowBurnCountdown = -1; // starts at -1 because first fuse call triggers when it shouldnt - serialized for debugging

    [SerializeField] private bool hasFuseStarted = false; // serialized for debugging
    [SerializeField] private int fuseIgnitionIndex = -1; // serialized for debugging
    private GameObject currentBurningFire;

    #region Subscriptions
    private void OnEnable()
    {
        Player.OnMove += MoveFuseIgnition;
    }

    private void OnDisable()
    {
        Player.OnMove -= MoveFuseIgnition;
    }
    #endregion

    // function from base to start the fuse
    public void PrepFuse(TileObject childOrigin)
    {
        for (int i = 0; i < fuseChildren.Count; i++)
        {
            if (childOrigin == fuseChildren[i])
            {
                // scenario: the fuse already started and youre lighting a spot that used to have a fuse
                if (!fuseChildren[i].gameObject.activeInHierarchy)
                {
                    Debug.Log("youve attempted to light a spot that has already been lit - returning");
                    return;
                }

                fuseIgnitionIndex = i;
            }    
        }

        hasFuseStarted = true;
        currentBurningFire = Instantiate(firePrefab, fuseChildren[fuseIgnitionIndex].gameObject.transform.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(currentBurningFire, SceneManager.GetSceneAt(1)); // spawn in level scene - not main menu
        fuseChildren[fuseIgnitionIndex].gameObject.SetActive(false);
        //--fuseIgnitionIndex; // bandaid to start at where the fire was released from player
        //MoveFuseIgnition(Vector3.zero);
    }

    // listener to move event to continue fuse
    private void MoveFuseIgnition(Vector3 playerPos)
    {
        if (!hasFuseStarted)
        {
            Debug.Log("the player sent out fire somewhere but we haven't even started yet (or the fuse ended) - returning");
            return;
        }

        if (burnType == BurnType.SLOW)
        {
            // so we've received another player move - check if we should move the ignition based on current vs set count to move
            ++slowBurnCountdown;
            if (slowBurnCountdown < playerMovesBeforeSlowBurnMoves)
            {
                // its still not time to move it, return but if it is, set countdown back to 0
                Debug.Log("the player moved but its not time to move slow fuse - returning");
                return;
            }
            slowBurnCountdown = 0;
        }

        ++fuseIgnitionIndex;

        // we've reached the last fuse so do something
        if (fuseIgnitionIndex == fuseChildren.Count - 1)
        {
            Debug.Log("end of fuse reached");
            hasFuseStarted = false;
            Destroy(currentBurningFire);
            fuseChildren[fuseIgnitionIndex].gameObject.SetActive(false);
        }
        else // move fire and turn off fuse sprite
        {
            currentBurningFire.transform.position = fuseChildren[fuseIgnitionIndex].gameObject.transform.position;

            // or maybe switch to a burnt fuse if we get sprites
            fuseChildren[fuseIgnitionIndex].gameObject.SetActive(false);
        }
    }
}
