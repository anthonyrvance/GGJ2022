using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRotator : MonoBehaviour
{
    [SerializeField] private float speed;

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, speed * Time.deltaTime));
    }
}
