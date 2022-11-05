using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(TimeBody))]
public class TimeRigidBody : MonoBehaviour
{
    private Rigidbody rb;
    private TimeBody tb;
    private bool originalKinematicValue = true;
    private bool doOnce = false;

    private Vector3[] velocities;
    private Vector3[] storageArray;

    private void OnEnable()
    {
        tb = GetComponent<TimeBody>();

        tb.onRecordPoint += record;
        tb.onStopRewind += rewindStopped;
        tb.onReleaseTime += release;
        tb.onHoldTime += hold;
    }

    private void OnDisable()
    {
        tb.onRecordPoint -= record;
        tb.onStopRewind -= rewindStopped;
        tb.onReleaseTime -= release;
        tb.onHoldTime -= hold;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocities = new Vector3[(int)Mathf.Round(tb.recordSeconds / Time.fixedDeltaTime)];

        originalKinematicValue = rb.isKinematic;
    }

    private void Update()
    {
        if (!doOnce)
        {
            velocities = new Vector3[(int)Mathf.Round(tb.recordSeconds / Time.fixedDeltaTime)];
            storageArray = new Vector3[(int)Mathf.Round(tb.recordSeconds / Time.fixedDeltaTime)];
            doOnce = true;
        }
        if (tb.isRewinding)
            rb.isKinematic = true;
    }

    private void record()
    {
        if(tb.sizeOfActivePoints < tb.maxPoints)
            velocities[tb.sizeOfActivePoints - 1] = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        else
        {
            for (int i = 0; i + 1 < velocities.Length; i++)
            {
                storageArray[i] = velocities[i + 1];
            }
            storageArray[velocities.Length - 1] = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            velocities = storageArray;
        }
    }

    private void hold(int index)
    {
        rb.isKinematic = true;
    }

    private void release()
    {
        rb.isKinematic = originalKinematicValue;
        if (!rb.isKinematic)
            rb.AddForce(velocities[tb.sizeOfActivePoints - 1], ForceMode.Impulse);
    }

    private void rewindStopped()
    {
        rb.isKinematic = originalKinematicValue;
        if(!rb.isKinematic)
            rb.AddForce(velocities[tb.sizeOfActivePoints - 1], ForceMode.Impulse);
    }
}
