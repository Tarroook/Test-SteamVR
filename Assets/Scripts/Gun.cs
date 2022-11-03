using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawn;
    public float bulletSpeed = 1f;

    public SteamVR_Action_Boolean fireAction;
    private Interactable interactable;


    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;
            if (fireAction[source].stateDown) // equivalent of getButtonDown(fireAction)
            {
                fire();
            }
        }
    }

    private void fire()
    {
        Debug.Log("Fired gun");
        Rigidbody bulletRB = Instantiate(bullet, spawn.position, spawn.rotation).GetComponent<Rigidbody>();
        bulletRB.gameObject.layer = 7;
        bulletRB.velocity = spawn.forward * bulletSpeed;
    }
}
