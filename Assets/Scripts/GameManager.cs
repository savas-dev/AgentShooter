using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Settings")]
    public GameObject tapToStartGO;
    public GameObject menuPanelGO;
    public GameObject gameOverPanelGO;
    public GameObject restartGameGO;
    public GameObject winPanelGO;
    public GameObject winStartTextGO;
    public TextMeshProUGUI levelText;

    [Header("Game Settings")]
    public bool isGameStarted;
    public int levelCount;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (PlayerPrefs.HasKey("level"))
        {
            levelCount = PlayerPrefs.GetInt("level");
        }
        else
        {
            levelCount = 1;
        }
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        levelText.text = "Level " + levelCount.ToString();

        tapToStartGO.SetActive(true);
        menuPanelGO.SetActive(true);
    }

    public void StartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            tapToStartGO.SetActive(false);
            menuPanelGO.SetActive(false);
        }
    }

    public void GameOver()
    {
        AdsController.instance.ShowAd();
        gameOverPanelGO.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Win()
    {
        AdsController.instance.ShowAd();
        winPanelGO.gameObject.SetActive(true);
        levelCount++;

        PlayerPrefs.SetInt("level", levelCount);
    }


    public void NextLevel()
    {
        int randLevel = Random.Range(0, SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene(randLevel);
    }

}
