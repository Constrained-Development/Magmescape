using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public const float GROUND_VERTICAL_POSITION = 3.275f;

    [SerializeField]
    private float lavaAndCameraSpeed;

    private CanvasGroup gameOverMenu;
    private TilemapCollider2D levelCollider;
    private LavaController lava;
    private CameraController cameraController;

    private bool movingCamera;

    private void Start()
    {
        gameOverMenu = GameObject.Find("Canvas/GameOverMenuPanel").GetComponent<CanvasGroup>();
        levelCollider = GameObject.Find("Grid/Level").GetComponent<TilemapCollider2D>();
        lava = GameObject.Find("Grid/Lava").GetComponent<LavaController>();
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        StartCoroutine("moveLavaInSeconds", 5);
    }

    private void Update()
    {
        if (!movingCamera && lava.Erupted)
        {
            cameraController.Speed = lavaAndCameraSpeed;
            movingCamera = true;
        }
    }

    private IEnumerator moveLavaInSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        lava.Speed = lavaAndCameraSpeed;
    }

    public void GameOver()
    {
        levelCollider.enabled = false;
        ShowGameOverMenu();
        lava.Speed = 0;
        cameraController.Speed = 0;
        // TODO:
        // disable user input
        // animate player jump
    }

    private void ShowGameOverMenu()
    {
        gameOverMenu.alpha = 1;
        gameOverMenu.interactable = true;
    }
}
