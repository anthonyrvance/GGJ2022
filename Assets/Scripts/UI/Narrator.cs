using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrator : MonoBehaviour
{
    [SerializeField] private string dialogue;

    private void Start()
    {
        GetComponent<DialogueScrollBox>().OnTriggerKeyPressed(dialogue);
    }
}
