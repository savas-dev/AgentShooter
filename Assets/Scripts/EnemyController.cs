using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    public Animator enemyAnim;
    public GameObject enemyBullet;
    public GameObject startPoint;
    public bool hasLive;
    public TextMeshPro enemyHealthText;
    public int enemyHealth;
    public Material normalMaterial;
    public Material hitMaterial;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        normalMaterial = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
        
        hasLive = true;
        enemyAnim = GetComponent<Animator>();

        int randSayi = Random.Range(1, 4);
        Debug.Log(randSayi);
        if(randSayi >= 1)
        {
            enemyHealth = Random.Range(10, 101);
        }

        if (randSayi == 2)
        {
            enemyHealth = Random.Range(10, 50);
        }

        if (randSayi > 3)
        {
            enemyHealth = Random.Range(80, 101);
        }
        enemyHealthText.text = enemyHealth.ToString();

        if (hasLive)
        {
            EnemyFire();
        }
    }

    private void Update()
    {
        
    }

    public void EnemyFire()
    {
        StartCoroutine(StartEnemyFire(0.8f));
    }

    public void StopFire()
    {
        StopCoroutine(StartEnemyFire(.1f));
    }

    private IEnumerator StartEnemyFire(float delay)
    {
        while (true && hasLive)
        {
            yield return new WaitForSeconds(delay);
            if (GameManager.instance.isGameStarted)
            {
                GameObject ebullet = Instantiate(enemyBullet, startPoint.transform.position, Quaternion.identity);
            }
        }
        
    }
}
