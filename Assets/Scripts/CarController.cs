using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public static CarController instance;

    public float carSpeed;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        int randSayi = Random.Range(1, 4);
        if(randSayi < 3)
        {
            carSpeed = -7.5f;
        }

        if (randSayi == 3)
        {
            carSpeed = -15f;
        }
    }

    private void Update()
    {
        if (GameManager.instance.isGameStarted)
        {
            transform.Translate(0, 0, -carSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CarDestroyer"))
        {
            Destroy(this.gameObject);
        }
    }
}
