using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGrenade : MonoBehaviour
{
    public GameObject explosionEffect;
    public float delay = 3f;
    public float blastRadius = 5f;
    public float blastForce = 700f;

    [SerializeField] private float countdown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
            explode();
    }

    private void explode()
    {
        Debug.Log("Boom");
        hasExploded = true;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(blastForce, transform.position, blastRadius);
            }
            // add damage if damageable
        }

        Destroy(gameObject);
    }
}
