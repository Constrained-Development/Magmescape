using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private GameObject controlsDisplay;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            controlsDisplay = GameObject.FindGameObjectWithTag("ControlsDisplay");
            controlsDisplay.SetActive(false);
        }
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void ToggleControlsDisplay()
    {
        controlsDisplay.SetActive(!controlsDisplay.activeSelf);
    }

}
