using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private RectTransform rt;
    private float scrollSpeed;
    [SerializeField] private float leftWall;
    [SerializeField] private float rightWall;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        scrollSpeed = Random.Range(5.0f, 50.0f);
    }

    // Update is called once per frame
    void Update()
    {
        rt.position += new Vector3(-scrollSpeed * Time.deltaTime, 0.0f);

        if (rt.localPosition.x <= leftWall)
        {
            rt.position += new Vector3(rightWall * 2.0f, 0.0f);
        }
    }
}
