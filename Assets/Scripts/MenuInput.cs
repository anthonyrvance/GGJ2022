using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    [SerializeField] private string sceneName;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManagement.instance.ReceiveLoad(sceneName);
        }
    }
}
