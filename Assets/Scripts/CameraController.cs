using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public GameObject target;
    public Vector3 offset;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void LateUpdate()
    {
        if(target != null)
        {
            transform.position = target.transform.position + offset;
        }
    }

}
