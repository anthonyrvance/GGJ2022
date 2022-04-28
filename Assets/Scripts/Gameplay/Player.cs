using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private GameObject spriteGO;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private GameObject futureMovementPosition;
    [SerializeField] private LayerMask collidableLM;
    [SerializeField] private LayerMask slipperyLM;
    [SerializeField] private float physicsOverlapTolerance;

    public delegate void PlayerMove(Vector3 pos);
    public static event PlayerMove OnMove;

    private int cachedDirectionForSliding;
    private bool isSliding;

    private void Awake()
    {
        // just as a cleaner look the movePos will start under the player but we'll unchild it so when we move it doesn't move with us
        futureMovementPosition.transform.parent = null;

        isSliding = false;
    }

    #region Subscriptions
    private void OnEnable()
    {
        MovementButtonHandler.MovementButtonPressed += MoveFutureMovementPosition;
    }

    private void OnDisable()
    {
        MovementButtonHandler.MovementButtonPressed -= MoveFutureMovementPosition;
    }
    #endregion

    private void Update()
    {
        // animation?
    }

    public void MoveFutureMovementPosition(int direction)
    {
        // 0 is up, 1 is right, 2 is down, 3 is left
        Vector3 pos = Vector3.zero;
        if (direction == 0)
        {
            pos = new Vector3(0.0f, 1.0f, 0.0f);
        }
        else if (direction == 1)
        {
            pos = new Vector3(1.0f, 0.0f, 0.0f);
        }
        else if (direction == 2)
        {
            pos = new Vector3(0.0f, -1.0f, 0.0f);
        }
        else if (direction == 3)
        {
            pos = new Vector3(-1.0f, 0.0f, 0.0f);
        }

        cachedDirectionForSliding = direction;
        CheckIfFuturePositionIsBlocked(pos);
    }

    private void CheckIfFuturePositionIsBlocked(Vector3 newPos)
    {
        if (Physics2D.OverlapCircle(futureMovementPosition.transform.position += newPos, physicsOverlapTolerance,  collidableLM))
        {
            UnsuccessfullyMove();
        }
        else
        {
            SuccessfullyMove();
        }
    }

    private void SuccessfullyMove()
    {
        // the future position succesfully moved, lets move player to it
        // could use something like movetowards or lerp or something
        this.transform.position = futureMovementPosition.transform.position;

        CheckIfPositionIsSlippery();
    }

    private void UnsuccessfullyMove()
    {
        // we hit something, go back to the previous spot
        // could play a little bounce back animation or something
        if (isSliding)
        {
            isSliding = false;
            FinishMove();
        }
        futureMovementPosition.transform.position = this.transform.position;
    }

    private void FinishMove()
    {
        OnMove(this.transform.position); // send out event
        animator.SetTrigger("OnMove");
    }

    private void CheckIfPositionIsSlippery()
    {
        if (Physics2D.OverlapCircle(this.transform.position, physicsOverlapTolerance, slipperyLM))
        {
            Debug.Log("yay we successfully moved to a position and its slippery - move again");
            isSliding = true;
            StartCoroutine(WaitThenCheckForSlipAgain());
        }
        else
        {
            FinishMove();
        }
    }

    IEnumerator WaitThenCheckForSlipAgain()
    {
        yield return new WaitForSeconds(0.1f);
        MoveFutureMovementPosition(cachedDirectionForSliding);
    }
}
