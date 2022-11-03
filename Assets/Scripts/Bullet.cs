using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        Debug.Log("Bullet collided with : " + collision.gameObject.name);
        GameObject.Destroy(gameObject);
    }


    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Destroy(gameObject);
    }
}
