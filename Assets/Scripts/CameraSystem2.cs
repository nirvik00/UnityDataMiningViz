using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem2 : MonoBehaviour
{
    [SerializeField]
    float speed = 0.5f;

    [SerializeField]
    float sensitivity = 1.0f;

    Vector3 anchorPoint;
    Quaternion anchorRot;

    bool orbitmouseModeActive = false;

    private Vector2 lastMousePosition;

    private void Awake()
    {
    }


    void Update()
    {
        // right mouse button drag to orbit
        if (Input.GetMouseButtonDown(1))
        {
            orbitmouseModeActive = true;
            lastMousePosition = Input.mousePosition;
            anchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            anchorRot = transform.rotation;
        }
        if (Input.GetMouseButtonUp(1))
        {
            orbitmouseModeActive = false;
        }

        MoveCamera_MouseSCroll();
    }

    void FixedUpdate()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            move += Vector3.forward * speed;
        if (Input.GetKey(KeyCode.S))
            move -= Vector3.forward * speed;
        if (Input.GetKey(KeyCode.D))
            move += Vector3.right * speed;
        if (Input.GetKey(KeyCode.A))
            move -= Vector3.right * speed;
        if (Input.GetKey(KeyCode.E))
            move += Vector3.up * speed;
        if (Input.GetKey(KeyCode.Q))
            move -= Vector3.up * speed;
        transform.Translate(move);


        // right mouse button drag to orbit
        if (orbitmouseModeActive == true)
        {
            Quaternion rot = anchorRot;
            Vector3 dif = anchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            rot.eulerAngles += dif * sensitivity;
            transform.rotation = rot;
            lastMousePosition = Input.mousePosition;
        }
    }

    private void MoveCamera_MouseSCroll()
    {
        Vector3 move = Vector3.zero;
        float speed = 10f;
        if (Input.mouseScrollDelta.y > 0)
        {
            move += Vector3.forward * speed;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            move -= Vector3.forward * speed;
        }
        transform.Translate(move);
    }


}