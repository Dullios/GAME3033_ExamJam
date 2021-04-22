using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneScript : MonoBehaviour
{
    public GameObject titleCanvas;
    public GameObject instructionCanvas;
    public GameObject creditCanvas;

    private void Start()
    {
        titleCanvas.SetActive(true);
        instructionCanvas.SetActive(false);
        creditCanvas.SetActive(false);
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnInstructionButton()
    {
        titleCanvas.SetActive(false);
        instructionCanvas.SetActive(true);
    }

    public void OnCreditButton()
    {
        titleCanvas.SetActive(false);
        creditCanvas.SetActive(true);
    }

    public void OnBackButton()
    {
        titleCanvas.SetActive(true);
        instructionCanvas.SetActive(false);
        creditCanvas.SetActive(false);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
