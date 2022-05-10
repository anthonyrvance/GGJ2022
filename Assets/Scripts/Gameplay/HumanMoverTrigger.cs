using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    FIRE,
    PRESSURE_PLATE
}

public class HumanMoverTrigger : TileObject
{
    [SerializeField] private TriggerType triggerType;

    [SerializeField] private List<GameObject> humansToMove;
    [SerializeField] private List<List<Vector3>> pointsToMoveTo;

    public override void ReceiveFire()
    {
        if (triggerType == TriggerType.FIRE)
        {
            MoveUnits();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerType == TriggerType.PRESSURE_PLATE)
        {
            MoveUnits();
        }
    }

    private void MoveUnits()
    {
        for (int i = 0; i < humansToMove.Count; i++)
        {
            StartCoroutine(LoopOverMovement(humansToMove[i]));
        }
    }

    IEnumerator LoopOverMovement(GameObject unit)
    {
        Debug.Log("Entering");
        yield return new WaitForSeconds(3.0f);
    }
}
