﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float countdownSeconds = 5;
    [SerializeField]
    private float rumblingSeconds = 2;
    [SerializeField]
    private float lavaAndCameraSpeed;
    [SerializeField]
    private float musicVolume = 0.25f;
    [SerializeField]
    private float lavaVolume = 1.0f;
    [SerializeField]
    private float rumblingVolume = 0.5f;
    [SerializeField]
    private float gemVolume = 0.5f;
    [SerializeField]
    private float deathVolume = 0.5f;
    [SerializeField]
    private float deathPitch = 2.5f;

    [SerializeField]
    private AudioClip musicClip;
    private AudioSource musicAudioSource;

    [SerializeField]
    private AudioClip lavaClip;
    private AudioSource lavaAudioSource;

    [SerializeField]
    private AudioClip rumblingClip;
    private AudioSource rumblingAudioSource;

    [SerializeField]
    private AudioClip gemClip;
    private AudioSource gemAudioSource;

    [SerializeField]
    private AudioClip deathClip;
    private AudioSource deathAudioSource;

    private GameObject backdrop;
    private GameObject gameOverMenu;
    private TilemapCollider2D levelCollider;
    private LavaController lavaController;
    private CameraController cameraController;
    private List<PlayerController> playerControllers;

    private bool movingCamera;

    private void Awake()
    {
        SetupAudio();
    }

    private void Start()
    {
        backdrop = GameObject.Find("Canvas/Backdrop");
        gameOverMenu = GameObject.Find("Canvas/GameOverMenuPanel");
        EnableGameOverMenu(false);

        levelCollider = GameObject.Find("Grid/Level").GetComponent<TilemapCollider2D>();
        lavaController = GameObject.Find("Grid/Lava").GetComponent<LavaController>();
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();



        var players = GameObject.FindGameObjectsWithTag("Player");
        playerControllers = new List<PlayerController>();
        foreach (var player in players)
        {
            playerControllers.Add(player.GetComponent<PlayerController>());
        }

        StartCoroutine(moveLavaInSeconds(countdownSeconds));
        musicAudioSource.Play();
    }

    private void Update()
    {
        if (!movingCamera && lavaController.IsErupted())
        {
            cameraController.SetSpeed(lavaAndCameraSpeed);
            movingCamera = true;
        }
    }

    private IEnumerator moveLavaInSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        rumblingAudioSource.Play();
        cameraController.Shake(rumblingSeconds);

        yield return new WaitForSecondsRealtime(rumblingSeconds);

        lavaController.SetSpeed(lavaAndCameraSpeed);
        lavaAudioSource.Play();
    }

    public void GameOver()
    {
        foreach (var player in playerControllers)
        {
            player.Kill();
        }
        EnableGameOverMenu(true);
        lavaController.SetSpeed(0);
        cameraController.SetSpeed(0);
        deathAudioSource.Play();
        // TODO:
        // disable user input
        // animate player jump
    }

    private void EnableGameOverMenu(bool show)
    {
        backdrop.SetActive(show);
        gameOverMenu.SetActive(show);
    }

    private void SetupAudio()
    {
        musicAudioSource = AddAudioSourceComponent(musicClip, true, musicVolume);
        lavaAudioSource = AddAudioSourceComponent(lavaClip, true, lavaVolume);
        rumblingAudioSource = AddAudioSourceComponent(rumblingClip, false, rumblingVolume);
        gemAudioSource = AddAudioSourceComponent(gemClip, false, gemVolume);
        deathAudioSource = AddAudioSourceComponent(deathClip, false, deathVolume, deathPitch);
    }

    private AudioSource AddAudioSourceComponent(AudioClip clip, bool loop, float volume, float pitch = 1)
    {
        AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        return audioSource;
    }
}
