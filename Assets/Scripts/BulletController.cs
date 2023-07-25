using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class BulletController : MonoBehaviour
{
    public static BulletController instance;
    public enum WeaponType{ pistol, rifle, enemyBullet};

    public WeaponType weaponType;
    public Rigidbody rb;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        switch (weaponType)
        {
            case WeaponType.pistol:
                transform.DOMoveZ(transform.position.z + 30f, 2f);
                Destroy(this.gameObject, 1f);
                break;

            case WeaponType.rifle:
                transform.DOMoveZ(transform.position.z + 100f, 3f);
                Destroy(this.gameObject, 1f);
                break;

            case WeaponType.enemyBullet:
                transform.DOMoveZ(transform.position.z - 100f, 15f);
                Destroy(this.gameObject, 7.5f);
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && weaponType == WeaponType.pistol)
        {
            if (other.GetComponent<EnemyController>().enemyHealth <= 0)
            {
                other.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = other.GetComponent<EnemyController>().hitMaterial;
                other.GetComponent<EnemyController>().enemyHealth = 0;
                other.GetComponent<EnemyController>().enemyHealthText.text = other.GetComponent<EnemyController>().enemyHealth.ToString();
                other.GetComponent<Animator>().Play("Die");
                other.GetComponent<EnemyController>().hasLive = false;
                other.GetComponent<CapsuleCollider>().enabled = false;
                Destroy(other.gameObject, 5f);

            }
            else if (EnemyController.instance.enemyHealth > 0)
            {
                other.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = other.GetComponent<EnemyController>().hitMaterial;
                other.GetComponent<EnemyController>().enemyHealth -= 5;
                if (other.GetComponent<EnemyController>().enemyHealth <= 0)
                {
                    other.GetComponent<EnemyController>().enemyHealth = 0;
                    other.GetComponent<EnemyController>().enemyHealthText.text = "0";
                }
                other.GetComponent<EnemyController>().enemyHealthText.text = other.GetComponent<EnemyController>().enemyHealth.ToString();
            }

        }

        if (other.gameObject.CompareTag("Enemy") && weaponType == WeaponType.rifle)
        {
            if (other.GetComponent<EnemyController>().enemyHealth <= 0)
            {
                other.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = other.GetComponent<EnemyController>().hitMaterial;
                other.GetComponent<EnemyController>().enemyHealth = 0;
                other.GetComponent<EnemyController>().enemyHealthText.text = other.GetComponent<EnemyController>().enemyHealth.ToString();
                other.GetComponent<Animator>().Play("Die");
                other.GetComponent<EnemyController>().hasLive = false;
                other.GetComponent<CapsuleCollider>().enabled = false;
                Destroy(other.gameObject, 5f);

            }
            else if (EnemyController.instance.enemyHealth > 0)
            {
                other.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = other.GetComponent<EnemyController>().hitMaterial;
                other.GetComponent<EnemyController>().enemyHealth -= 10;
                if(other.GetComponent<EnemyController>().enemyHealth <= 0)
                {
                    other.GetComponent<EnemyController>().enemyHealth = 0;
                    other.GetComponent<EnemyController>().enemyHealthText.text = "0";
                }
                other.GetComponent<EnemyController>().enemyHealthText.text = other.GetComponent<EnemyController>().enemyHealth.ToString();
            }

        }

        if (other.gameObject.CompareTag("Player") && weaponType == WeaponType.enemyBullet)
        {
            if (PlayerController.instance.playerHealth <= 0)
            {
                PlayerController.instance.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = PlayerController.instance.hitMaterial;
                PlayerController.instance.playerHealth = 0;
                PlayerController.instance.hasLive = false;
                PlayerController.instance.hasRifle = false;
                PlayerController.instance.hasPistol = false;
                PlayerController.instance.pistolOnHand.SetActive(false);
                PlayerController.instance.rifleOnHand.SetActive(false);
                PlayerController.instance.playerHealthText.text = PlayerController.instance.playerHealth.ToString();
                other.GetComponent<Animator>().Play("Die");
                other.GetComponent<CapsuleCollider>().enabled = false;
                PlayerController.instance.speed = 0;
                Destroy(PlayerController.instance.playerHealthText.gameObject, .5f);
                StartCoroutine(StopTime(3.5f));

            }
            else if(PlayerController.instance.playerHealth > 0)
            {
                PlayerController.instance.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = PlayerController.instance.hitMaterial;
                PlayerController.instance.playerHealthText.GetComponent<Animator>().Play("HealtAnim");
                PlayerController.instance.playerHealth -= 5;
                PlayerController.instance.playerHealthText.text = PlayerController.instance.playerHealth.ToString();
            }
        }
    }

    private void OnTriggerExit(Collider Other)
    {
        if (Other.gameObject.CompareTag("Enemy") && weaponType == WeaponType.pistol)
        {
            Other.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = EnemyController.instance.normalMaterial;
        }

        if (Other.gameObject.CompareTag("Player") && weaponType == WeaponType.enemyBullet)
        {
            PlayerController.instance.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = PlayerController.instance.normalMaterial;
            PlayerController.instance.playerHealthText.GetComponent<Animator>().Play("ExitFire");
            Invoke(nameof(HealthTextNormalAnim), .25f);
        }
    }

    IEnumerator StopTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Time.timeScale = 0;

        GameManager.instance.GameOver();
    }

    public void HealthTextNormalAnim()
    {
        PlayerController.instance.playerHealthText.GetComponent<Animator>().Play("Null");
    }
}
