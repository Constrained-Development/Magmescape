using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int playerID = 0;

    [SerializeField]
    private Utilities.ColorEnum playerColor;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float jumpForce = 1.0f;

    [SerializeField]
    private GameObject lavaDeathSplashParticle;

    private Vector2 direction;
    private bool jump = false;
    private bool dead = false;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource jumpAudioSource;
    private Rewired.Player player;
    private int levelCollisions = 0;
    private GameManager gameManager;
    private Transform prevParent;

    // Use this for initialization
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpAudioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();

        player = ReInput.players.GetPlayer(playerID);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!dead)
        {
            GetInput();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Lava")
        {
            Instantiate(lavaDeathSplashParticle, transform.position, lavaDeathSplashParticle.transform.rotation);
            gameManager.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Level" || other.collider.tag == "MovingPlatform")
        {
            levelCollisions++;
        }

        if (other.collider.tag == "MovingPlatform")
        {
            prevParent = transform.parent;
            transform.parent = other.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Level" || other.collider.tag == "MovingPlatform")
        {
            levelCollisions--;
        }

        if (other.collider.tag == "MovingPlatform")
        {
            transform.parent = prevParent;
        }
    }

    private void GetInput()
    {
        direction = Vector2.zero;
        direction.x = player.GetAxis("Move Horizontal");

        if (!jump && levelCollisions != 0)
        {
            jump = player.GetButtonDown("Action");
        }
    }

    private void Move()
    {
        if (direction.x != 0.0f)
        {
            if (levelCollisions != 0)
            {
                animator.SetLayerWeight(1, 1.0f);
            }

            spriteRenderer.flipX = direction.x < 0.0f;
            body.velocity = new Vector2(direction.x * speed * Time.fixedDeltaTime, body.velocity.y);
        }
        else
        {
            if (levelCollisions != 0)
            {
                animator.SetLayerWeight(1, 0.0f);
            }

            body.velocity = new Vector2(0.0f, body.velocity.y);
        }

        if (jump)
        {
            jump = false;

            jumpAudioSource.Play();
            body.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }

        if (levelCollisions == 0)
        {
            animator.SetLayerWeight(2, 1.0f);
        }
        else
        {
            animator.SetLayerWeight(2, 0.0f);
        }
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    public Utilities.ColorEnum GetPlayerColor()
    {
        return playerColor;
    }

    public void Kill()
    {
        Destroy(boxCollider);
        dead = true;
    }
}
