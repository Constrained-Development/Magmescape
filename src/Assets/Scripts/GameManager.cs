using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int countdownSeconds = 5;
    [SerializeField]
    private float rumblingSeconds = 2;
    [SerializeField]
    private float lavaAndCameraSpeed = 1;

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

    private SkullsManager skullsManager;
    private MenuController menuController;
    private TextMeshProUGUI countdownText;
    private TextMeshProUGUI gemsCounter;
    private GameObject backdrop;
    private GameObject gameOverMenu;
    private LavaController lavaController;
    private CameraController cameraController;
    private List<PlayerController> playerControllers;
    private Rewired.Player sharedInput;

    private bool movingCamera;
    private int redGems = 0;
    private int blueGems = 0;
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
        countdownText = GameObject.Find("Canvas/CountdownText").GetComponent<TextMeshProUGUI>();
        skullsManager = GameObject.FindGameObjectWithTag("SkullManager").GetComponent<SkullsManager>();

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

        skullsManager.SpawnSkulls();
        SetDifficulty();

        UpdateUI();

        StartCoroutine(StartCountdown(countdownSeconds));
        StartCoroutine(RumbleInSeconds(countdownSeconds));
        StartCoroutine(MoveLavaInSeconds(countdownSeconds + rumblingSeconds));
        StartCoroutine(RandomRumbler(15, 30));

        musicAudioSource.Play();

        Cursor.visible = false;
    }

    private void Update()
    {
        if (!gameOver && !gameWon && !movingCamera && lavaController.IsErupted())
        {
            cameraController.SetSpeed(lavaAndCameraSpeed);
            movingCamera = true;
        }

        if (gameOver || gameWon)
        {
            if (sharedInput.GetButtonDown("Quit"))
            {
                menuController.QuitGame();
            }

            if (sharedInput.GetButtonDown("Restart"))
            {
                menuController.LoadSceneByIndex(1);
            }
        }
    }

    private void SetDifficulty()
    {
        switch (Utilities.difficulty)
        {
            case 0:
                lavaAndCameraSpeed = Utilities.easySpeed;
                break;

            case 1:
                lavaAndCameraSpeed = Utilities.mediumSpeed;
                break;

            case 2:
            default:
                lavaAndCameraSpeed = Utilities.hardSpeed;
                break;
        }
    }

    private IEnumerator RandomRumbler(float minSecondsBetweenRumbles, float maxSecondsBetweenRumbles)
    {
        while (!gameOver && !gameWon)
        {
            float secondsBetweenRumbles = Random.Range(minSecondsBetweenRumbles, maxSecondsBetweenRumbles);
            yield return new WaitForSecondsRealtime(secondsBetweenRumbles);
            if (!gameOver && !gameWon)
            {
                Rumble();
            }
        }
    }

    private IEnumerator MoveLavaInSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        lavaController.SetSpeed(lavaAndCameraSpeed);
        lavaAudioSource.Play();
    }

    private void Rumble()
    {
        cameraController.Shake(rumblingSeconds);
        rumblingAudioSource.Play();
    }

    private IEnumerator RumbleInSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Rumble();
    }

    private IEnumerator StartCountdown(int seconds)
    {
        countdownText.gameObject.SetActive(true);
        for (var i = 0; i < seconds; i++)
        {
            yield return new WaitForSeconds(1);
            countdownText.text = (seconds - i - 1).ToString();
        }
        countdownText.gameObject.SetActive(false);
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
        Cursor.visible = true;
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
        if (gameWon || gameOver)
        {
            return;
        }

        gameOver = true;

        foreach (var player in playerControllers)
        {
            skullsManager.AddLocation(player.transform.position);
            player.Kill();
        }
        EnableGameOverMenu(true);
        lavaController.SetSpeed(0);
        cameraController.SetSpeed(0);
        deathAudioSource.Play();
    }

    public void WinGame()
    {
        if (gameOver || gameWon)
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
