using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField]
    private Transform elevatorContainer;
    [SerializeField]
    private float speed = 2.0f;

    private Transform elevator;
    private Transform destination;
    private Utilities.ColorEnum crystalColor;
    private bool active = false;
    private bool goingFromOrigin = true;
    private Vector3 origin;
    private Rigidbody2D elevatorBody;

    // Use this for initialization
    private void Start()
    {
        crystalColor = GetComponent<CrystalController>().GetCrystalColor();

        elevator = elevatorContainer.GetChild(0);
        destination = elevatorContainer.GetChild(1);

        elevatorBody = elevator.GetComponent<Rigidbody2D>();
        origin = elevator.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (active)
        {
            Vector2 dir;
            float move = Time.fixedDeltaTime * speed; ;

            if (goingFromOrigin)
            {
                dir = destination.position - elevator.position;

                if (dir.magnitude < move)
                {
                    move = dir.magnitude;
                    goingFromOrigin = false;
                }
            }
            else
            {
                dir = origin - elevator.position;

                if (dir.magnitude < move)
                {
                    move = dir.magnitude;
                    goingFromOrigin = true;
                }
            }

            elevatorBody.MovePosition(elevatorBody.position + (dir.normalized * move));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            active = false;
        }
    }
}
