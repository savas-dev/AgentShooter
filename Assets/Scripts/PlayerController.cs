using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float speed = 5f;
    private float xSpeed;
    public float limitX = 1.68f;
    public bool isPlayerMoving;
    public Animator playerAnim;
    public TextMeshPro playerHealthText;
    public int playerHealth;
    public Material normalMaterial;
    public Material hitMaterial;

    [Header("Gun Settings")]
    public bool hasWeapon;
    public bool hasRifle;
    public bool hasPistol;
    public bool hasLive;
    public bool touchedCar;
    public bool isFinished;
    public GameObject pistolOnHand;
    public GameObject rifleOnHand;
    public GameObject pistolStartPoint;
    public GameObject rifleStartPoint;
    public GameObject pistolBullet;
    public GameObject rifleBullet;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        Time.timeScale = 1;

        normalMaterial = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
        hasLive = true;
        playerHealth = 100;
        playerHealthText.text = playerHealth.ToString();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        if (GameManager.instance.isGameStarted)
        {
            

            if (isPlayerMoving)
            {
                return;
            }

            if (!hasWeapon && hasLive && !touchedCar && !isFinished)
            {
                playerAnim.Play("Run");
            }

            if (hasLive)
            {
                float touchX = 0f;
                float newXValue;
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    xSpeed = 1000f;
                    touchX = Input.GetTouch(0).deltaPosition.x / Screen.width;
                }
                else if (Input.GetMouseButton(0))
                {
                    xSpeed = 300f;
                    touchX = Input.GetAxis("Mouse X");
                }

                newXValue = transform.position.x + xSpeed * touchX * Time.deltaTime;
                newXValue = Mathf.Clamp(newXValue, -limitX, limitX);
                Vector3 playerNewPosition = new Vector3(newXValue, transform.position.y, transform.position.z + speed * Time.deltaTime);
                transform.position = playerNewPosition;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pistol") && !touchedCar)
        {
            Destroy(other.gameObject);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 20, transform.rotation.z);
            playerHealthText.transform.localRotation = Quaternion.Euler(0, -20, 0);
            hasWeapon = true;
            Pistol();
        }

        if (other.gameObject.CompareTag("Rifle") && !touchedCar)
        {
            Destroy(other.gameObject);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 20, transform.rotation.z);
            playerHealthText.transform.localRotation = Quaternion.Euler(0, -20, 0);
            hasWeapon = true;
            Rifle();
        }

        if (other.gameObject.CompareTag("Car"))
        {
            playerHealth = 0;
            playerHealthText.text = playerHealth.ToString();
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = hitMaterial;
            hasLive = false;
            hasRifle = false;
            hasPistol = false;
            pistolOnHand.SetActive(false);
            rifleOnHand.SetActive(false);
            touchedCar = true;
            other.GetComponent<CarController>().carSpeed = 0;
            other.transform.DOShakeRotation(1f, 50);
            speed = 0;
            transform.DOMoveZ(transform.position.z - 3f, 1f);
            playerAnim.Play("Die");
            StartCoroutine(StopTime(3.5f));
            
        }

        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Obstacle"))
        {
            playerHealth = 0;
            playerHealthText.text = playerHealth.ToString();
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = hitMaterial;
            hasLive = false;
            hasRifle = false;
            hasPistol = false;
            pistolOnHand.SetActive(false);
            rifleOnHand.SetActive(false);
            speed = 0;
            transform.DOMoveZ(transform.position.z - 3f, 1f);
            playerAnim.Play("Die");
            StartCoroutine(StopTime(3.5f));
        }

        if (other.gameObject.CompareTag("FinishLine"))
        {
            Debug.Log("Finale GEldi");
            hasRifle = false;
            hasPistol = false;
            pistolOnHand.SetActive(false);
            rifleOnHand.SetActive(false);
            isFinished = true;
            isPlayerMoving = false;
            speed = 0;
            playerHealthText.gameObject.SetActive(false);

            transform.DORotateQuaternion(Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z), .5f);

            string dance1 = "dance1";
            string dance2 = "dance2";
            string dance3 = "dance3";

            int randDance = Random.Range(1, 4);
            Debug.Log(randDance);
            if(randDance == 1)
            {
                playerAnim.Play(dance1);
            }
            if (randDance == 2)
            {
                playerAnim.Play(dance2);
            }
            if (randDance == 3)
            {
                playerAnim.Play(dance3);
            }
            else
            {
                playerAnim.Play(dance1);
            }

            StartCoroutine(WinTime(3.5f));


        }
    }

    public void Pistol()
    {
        hasPistol = true;
        hasRifle = false;
        pistolOnHand.SetActive(true);
        rifleOnHand.SetActive(false);
        playerAnim.Play("PistolRun");

        StartCoroutine(PistolFire(0.5f));
    }

    public void Rifle()
    {
        hasRifle = true;
        hasPistol = false;
        rifleOnHand.SetActive(true);
        pistolOnHand.SetActive(false);
        playerAnim.Play("RifleRun");

        StartCoroutine(RifleFire(0.2f));
    }

    private IEnumerator PistolFire(float delay)
    {
        while (true && hasPistol)
        {
            yield return new WaitForSeconds(delay);
            GameObject bullet = Instantiate(pistolBullet, pistolStartPoint.transform.position, Quaternion.identity);
        }
    }

    private IEnumerator RifleFire(float delay)
    {
        while (true && hasRifle)
        {
            yield return new WaitForSeconds(delay);
            GameObject bullet = Instantiate(pistolBullet, pistolStartPoint.transform.position, Quaternion.identity);
        }
    }

    IEnumerator StopTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Time.timeScale = 0;

        GameManager.instance.GameOver();
    }

    IEnumerator WinTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Time.timeScale = 0;
        
        GameManager.instance.Win();
    }

}
