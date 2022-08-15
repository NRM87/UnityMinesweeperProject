using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float scrollSens = 5.0f;
    public float minZoom = 4.0f;
    public float maxMenuWidth = 11.0f;
    private float maxZoom = 16.0f;
    private float moveSpeed;
    private bool dragOriginSet;
    private Vector3 dragOrigin;
    private Vector3 dragDifference;
    void Start()
    {
        
        Camera.main.orthographicSize = maxZoom;
    }
    void Update()
    {
        //process scroll wheel and zoom
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSens;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        moveSpeed = Camera.main.orthographicSize * 0.005f;

        //process inputs and movements
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            transform.Translate(-moveSpeed, 0.0f, 0.0f);
        }
        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            transform.Translate(0.0f, moveSpeed, 0.0f);
        }
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            transform.Translate(moveSpeed, 0.0f, 0.0f);
        }
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            transform.Translate(0.0f, -moveSpeed, 0.0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragOriginSet = true;
        }
        if (Input.GetMouseButton(0) && dragOriginSet)
        {
            dragDifference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += dragDifference;
        }
        else
        {
            dragOriginSet = false;
        }
    }
}
