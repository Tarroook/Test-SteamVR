using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawn;
    public float bulletSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            fire();
    }

    private void fire()
    {
        Debug.Log("Fired gun");
        Rigidbody bulletRB = Instantiate(bullet, spawn.position, spawn.rotation).GetComponent<Rigidbody>();
        bulletRB.velocity = spawn.forward * bulletSpeed;
    }
}
