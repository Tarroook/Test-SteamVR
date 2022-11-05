using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(TimeBody))]
public class TimeRigidBody : MonoBehaviour
{
    private Rigidbody rb;
    private TimeBody tb;
    private bool originalKinematicValue = true;

    private Vector3[] velocities;

    private void OnEnable()
    {
        tb = GetComponent<TimeBody>();

        tb.onRecordPoint += record;
        tb.onStopRewind += rewindStopped;
        tb.onReleaseTime += release;
    }

    private void OnDisable()
    {
        tb.onRecordPoint -= record;
        tb.onStopRewind -= rewindStopped;
        tb.onReleaseTime -= release;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocities = new Vector3[(int)Mathf.Round(tb.recordSeconds / Time.fixedDeltaTime)];

        originalKinematicValue = rb.isKinematic;
    }

    private void Update()
    {
        if (tb.isRewinding || tb.isHolding)
            rb.isKinematic = true;
    }

    private void record()
    {
        velocities[tb.sizeOfActivePoints - 1] = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
    }

    private void release()
    {
        rb.isKinematic = originalKinematicValue;
        if (!rb.isKinematic)
            rb.AddForce(velocities[tb.sizeOfActivePoints - 1], ForceMode.Impulse);
        Debug.Log(velocities[tb.sizeOfActivePoints - 1]);
    }

    private void rewindStopped()
    {
        rb.isKinematic = originalKinematicValue;
        if(!rb.isKinematic)
            rb.AddForce(velocities[tb.sizeOfActivePoints - 1], ForceMode.Impulse);
    }
}
