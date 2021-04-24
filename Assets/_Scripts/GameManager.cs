using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public TMP_Text scoreText;

    public bool isPaused;
    public float timer = 0.0f;

    public bool unlocked;

    [Header("SFX")]
    public AudioSource menuClick;
    public AudioSource smallPop;
    public AudioSource largePop;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
            timer += Time.deltaTime;
    }

    public void PauseGame()
    {
        if(!isPaused)
        {
            isPaused = true;

            Cursor.lockState = CursorLockMode.None;

            pauseMenu.SetActive(true);
        }
        else
        {
            isPaused = false;

            Cursor.lockState = CursorLockMode.Locked;

            pauseMenu.SetActive(false);
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;

        gameOverMenu.SetActive(true);

        scoreText.text = "Final Time: " + timer.ToString("00.0");
    }

    public void MenuClick()
    {
        menuClick.Play();
    }

    public void SmallPop()
    {
        smallPop.Play();
    }

    public void LargePop()
    {
        largePop.Play();
    }
}
