using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
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
}
