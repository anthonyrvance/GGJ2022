using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Movement stuff")]
    [SerializeField] private TextMeshProUGUI movesLeftBeforeFireText;
    [SerializeField] private string movesLeftBeforeFireString;
    [SerializeField] private int numMovesOnReset;
    [SerializeField] private int currentMovesLeft;

    private void Awake()
    {
        currentMovesLeft = numMovesOnReset;
        UpdateText();
    }

    #region Subscriptions
    private void OnEnable()
    {
        Player.OnMove += PlayerMoved;
    }

    private void OnDisable()
    {
        Player.OnMove -= PlayerMoved;
    }
    #endregion

    private void PlayerMoved()
    {
        // check moves count
        --currentMovesLeft;
        if (currentMovesLeft <= 0)
        {
            // do fire thing and check nearby tiles (somehow...)((just use vectors?)
            // maybe do a coroutine here to do some fire projectiles and after that update text
            currentMovesLeft = numMovesOnReset;
        }
        else
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        movesLeftBeforeFireText.text = movesLeftBeforeFireString + " " + currentMovesLeft;
    }
}
