using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.LogError("hello");
            SceneManagement.instance.GoToNextScene();
        }
    }
}
