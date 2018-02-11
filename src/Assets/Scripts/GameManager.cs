using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float countdownSeconds = 5;
    [SerializeField]
    private float rumblingSeconds = 2;
    [SerializeField]
    private float lavaAndCameraSpeed;

    [SerializeField]
    private Transform chestTransform;
    [SerializeField]
    private float gemSprayInterval = 0.1f;
    [SerializeField]
    private float gemSprayMinForce = 2f;
    [SerializeField]
    private float gemSprayMaxForce = 5f;
    [SerializeField]
    private GameObject standardBlueGem;
    [SerializeField]
    private GameObject standardRedGem;

    [SerializeField]
    private float musicVolume = 0.25f;
    [SerializeField]
    private float lavaVolume = 1.0f;
    [SerializeField]
    private float victoryVolume = 0.5f;
    [SerializeField]
    private float rumblingVolume = 0.5f;
    [SerializeField]
    private float gemVolume = 0.5f;
    [SerializeField]
    private float doorVolume = 0.5f;
    [SerializeField]
    private float crystalVolume = 0.5f;
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
    private AudioClip victoryClip;
    private AudioSource victoryAudioSource;

    [SerializeField]
    private AudioClip rumblingClip;
    private AudioSource rumblingAudioSource;

    [SerializeField]
    private AudioClip gemClip;
    private AudioSource gemAudioSource;

    [SerializeField]
    private AudioClip doorClip;
    private AudioSource doorAudioSource;

    [SerializeField]
    private AudioClip crystalClip;
    private AudioSource crystalAudioSource;

    [SerializeField]
    private AudioClip deathClip;
    private AudioSource deathAudioSource;

    private MenuController menuController;
    private TextMeshProUGUI gemsCounter;
    private GameObject backdrop;
    private GameObject gameOverMenu;
    private LavaController lavaController;
    private CameraController cameraController;
    private List<PlayerController> playerControllers;
    private Rewired.Player sharedInput;

    private bool movingCamera;
    private int redGems = 50;
    private int blueGems = 30;
    private bool gameWon = false;
    private bool gameOver = false;

    private void Awake()
    {
        SetupAudio();
    }

    private void Start()
    {
        sharedInput = ReInput.players.GetPlayer("Shared");
        menuController = GetComponent<MenuController>();

        gemsCounter = GameObject.Find("Canvas/GemsCounterText").GetComponent<TextMeshProUGUI>();

        backdrop = GameObject.Find("Canvas/Backdrop");
        gameOverMenu = GameObject.Find("Canvas/GameOverMenuPanel");
        EnableGameOverMenu(false);

        lavaController = GameObject.Find("Grid/Lava").GetComponent<LavaController>();
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();

        var players = GameObject.FindGameObjectsWithTag("Player");
        playerControllers = new List<PlayerController>();
        foreach (var player in players)
        {
            playerControllers.Add(player.GetComponent<PlayerController>());
        }

        UpdateUI();

        StartCoroutine(MoveLavaInSeconds(countdownSeconds));
        musicAudioSource.Play();
    }

    private void Update()
    {
        if (!movingCamera && lavaController.IsErupted())
        {
            cameraController.SetSpeed(lavaAndCameraSpeed);
            movingCamera = true;
        }

        if (sharedInput.GetButtonDown("Quit"))
        {
            menuController.QuitGame();
        }

        if (sharedInput.GetButtonDown("Restart"))
        {
            menuController.LoadSceneByIndex(1);
        }
    }

    private IEnumerator MoveLavaInSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        rumblingAudioSource.Play();
        cameraController.Shake(rumblingSeconds);

        yield return new WaitForSecondsRealtime(rumblingSeconds);

        lavaController.SetSpeed(lavaAndCameraSpeed);
        lavaAudioSource.Play();
    }

    private IEnumerator SprayGems()
    {
        while (redGems != 0 || blueGems != 0)
        {
            if (redGems != 0)
            {
                SprayGem(standardRedGem);
                redGems--;
                yield return new WaitForSecondsRealtime(gemSprayInterval);
            }

            if (blueGems != 0)
            {
                SprayGem(standardBlueGem);
                blueGems--;
                yield return new WaitForSecondsRealtime(gemSprayInterval);
            }
        }
    }

    private void SprayGem(GameObject gem)
    {
        var dir = new Vector2(Random.Range(-2.0f, 2.0f), 1).normalized;
        var rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        var gemInstance = Instantiate(gem, chestTransform.position, rot);
        gemInstance.GetComponent<Rigidbody2D>().AddForce(dir * Random.Range(gemSprayMinForce, gemSprayMaxForce));
        gemAudioSource.Play();
    }

    private void EnableGameOverMenu(bool show)
    {
        backdrop.SetActive(show);
        gameOverMenu.SetActive(show);
    }

    private void UpdateUI()
    {
        gemsCounter.text = (blueGems + redGems).ToString();
    }

    private void SetupAudio()
    {
        musicAudioSource = AddAudioSourceComponent(musicClip, true, musicVolume);
        lavaAudioSource = AddAudioSourceComponent(lavaClip, true, lavaVolume);
        rumblingAudioSource = AddAudioSourceComponent(rumblingClip, false, rumblingVolume);
        gemAudioSource = AddAudioSourceComponent(gemClip, false, gemVolume);
        doorAudioSource = AddAudioSourceComponent(doorClip, false, doorVolume);
        crystalAudioSource = AddAudioSourceComponent(crystalClip, false, crystalVolume);
        deathAudioSource = AddAudioSourceComponent(deathClip, false, deathVolume, deathPitch);
        victoryAudioSource = AddAudioSourceComponent(victoryClip, false, victoryVolume);
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

    public void GameOver()
    {
        if (gameWon)
        {
            return;
        }

        gameOver = true;

        foreach (var player in playerControllers)
        {
            player.Kill();
        }
        EnableGameOverMenu(true);
        lavaController.SetSpeed(0);
        cameraController.SetSpeed(0);
        deathAudioSource.Play();
    }

    public void WinGame()
    {
        if (gameOver)
        {
            return;
        }

        gameWon = true;

        StartCoroutine(SprayGems());
        lavaController.SetSpeed(0);
        cameraController.SetSpeed(0);
        victoryAudioSource.Play();
    }

    public void IncrementGems(Utilities.ColorEnum color)
    {
        if (color == Utilities.ColorEnum.Red)
        {
            redGems++;
        }
        else
        {
            blueGems++;
        }


        gemAudioSource.Play();
        UpdateUI();
    }

    public void PlayOpenDoorSound()
    {
        doorAudioSource.pitch = 1.0f;
        doorAudioSource.time = 0.0f;
        doorAudioSource.Play();
    }

    public void PlayCloseDoorSound()
    {
        doorAudioSource.pitch = -1.0f;
        doorAudioSource.timeSamples = doorAudioSource.clip.samples - 1;
        doorAudioSource.Play();
    }

    public void PlayActivateCrystalSound()
    {
        crystalAudioSource.pitch = 1.0f;
        crystalAudioSource.time = 0.0f;
        crystalAudioSource.Play();
    }

    public void PlayDeactivateCrystalSound()
    {
        crystalAudioSource.pitch = -1.0f;
        crystalAudioSource.timeSamples = crystalAudioSource.clip.samples - 1;
        crystalAudioSource.Play();
    }
}
