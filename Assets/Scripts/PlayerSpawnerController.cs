using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerController : MonoBehaviour
{
    public static PlayerSpawnerController instance;

    public GameObject playerGO;
    public List<GameObject> playerList = new List<GameObject>();
    public float speed = 5f;
    private float xSpeed;
    public float limitX = 1.68f;
    public int playerMaxCount;
    private bool isPlayerMoving;

    [Header("Animation Settings")]
    public Animator playerAnim;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        playerList.Add(transform.GetChild(0).gameObject);

        //playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        if (GameManager.instance.isGameStarted)
        {
            foreach (var item in playerList)
            {
                item.GetComponent<Animator>().Play("Run");
            }

            //playerAnim.Play("Run");

            if (isPlayerMoving)
            {
                return;
            }

            float touchX = 0f;
            float newXValue;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                xSpeed = 500f;
                touchX = Input.GetTouch(0).deltaPosition.x / Screen.width;
            }
            else if (Input.GetMouseButton(0))
            {
                xSpeed = 300f;
                touchX = Input.GetAxis("Mouse X");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnPlayer();
            }

            newXValue = transform.position.x + xSpeed * touchX * Time.deltaTime;
            newXValue = Mathf.Clamp(newXValue, -limitX, limitX);
            Vector3 playerNewPosition = new Vector3(newXValue, transform.position.y, transform.position.z + speed * Time.deltaTime);
            transform.position = playerNewPosition;

        }
    }

    public void SpawnPlayer()
    {
        if(playerList.Count <= playerMaxCount)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject newPlayerGO = Instantiate(playerGO, GetPlayerPosition(), Quaternion.identity, transform);
                playerList.Add(newPlayerGO);
            }
        }
        
    }

    public Vector3 GetPlayerPosition()
    {
        Vector3 pos = Random.insideUnitSphere * 0.3f;
        Vector3 newPos = transform.position + pos;
        return newPos;
    }
}
