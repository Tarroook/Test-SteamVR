using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerMovement : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public float speed = 1;
    private CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.axis.magnitude > .1f)
        {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0f, input.axis.y));
            cc.Move(Vector3.ProjectOnPlane(direction, Vector3.up) * speed * Time.deltaTime - new Vector3(0f, 9.81f, 0f) * Time.deltaTime);
        }
    }
}
