using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    private GameObject controlsDisplay;
    private TMP_Dropdown difficultyDropdown;
    private bool inMainMenu;

    private void Start()
    {
        inMainMenu = SceneManager.GetActiveScene().buildIndex == 0;

        if (inMainMenu)
        {
            controlsDisplay = GameObject.FindGameObjectWithTag("ControlsDisplay");
            controlsDisplay.SetActive(false);

            difficultyDropdown = GameObject.Find("Canvas/Difficulty/Dropdown").GetComponent<TMP_Dropdown>();
        }
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        if (inMainMenu)
        {
            Utilities.difficulty = difficultyDropdown.value;
        }

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
