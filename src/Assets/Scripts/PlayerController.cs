using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int playerID = 0;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float jumpForce = 1.0f;

    private Vector2 direction;
    private bool jump = false;

    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rewired.Player player;

    // Use this for initialization
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        direction = Vector2.zero;
        direction.x = player.GetAxis("Move Horizontal");

        if (!jump)
        {
            jump = player.GetButtonDown("Action");
        }
    }

    private void Move()
    {
        if (direction.x != 0.0f)
        {
            // animator.SetLayerWeight(1, 1.0f);
            spriteRenderer.flipX = direction.x < 0.0f;
            body.velocity = new Vector2(direction.x * speed * Time.fixedDeltaTime, body.velocity.y);
        }
        else
        {
            // animator.SetLayerWeight(1, 0.0f);
            body.velocity = new Vector2(0.0f, body.velocity.y);
        }

        if (jump)
        {
            jump = false;
            body.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }
}
