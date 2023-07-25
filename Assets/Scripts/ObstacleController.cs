using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleController : MonoBehaviour
{
    public static ObstacleController instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.DOMoveZ(other.transform.position.z - 10f, 0.5f);
        }
    }
}
