using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerMovement : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    private CharacterController cc;

    public float speed = 1;

    public float gravMultiplier = 1f;
    private readonly float gravity = -9.81f;
    [SerializeField] Vector3 velocity;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        cc.detectCollisions = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.axis.magnitude > .1f)
        {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0f, input.axis.y));
            cc.Move(Vector3.ProjectOnPlane(direction, Vector3.up) * speed * Time.deltaTime);
        }

        doGravity();
    }

    private void doGravity() // need to multiply by time squared because physics
    {
        if (!cc.isGrounded)
        {
            velocity.y += gravity * gravMultiplier * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity.y = -2f;
        }
    }
}
