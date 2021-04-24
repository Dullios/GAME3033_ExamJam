using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;

    public bool isPaused;
    public float timer = 0.0f;

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

            canvas.SetActive(true);
        }
        else
        {
            isPaused = false;

            Cursor.lockState = CursorLockMode.Locked;

            canvas.SetActive(false);
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
