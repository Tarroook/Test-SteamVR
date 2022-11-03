using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    private bool isRewinding = false;
    public float recordSeconds = 5f;

    private List<PointInTime> pointsInTime;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            startRewind();
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            stopRewind();
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
            rewind();
        else
            record();
    }

    // 1f / Time.fixedDeltaTime is the amounts of fixedUpdate called in a second, so multiply it by x to record x seconds
    // And also round it because Count is int
    private void record() 
    {
        if(pointsInTime.Count > Mathf.Round(recordSeconds / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }
        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, transform.localScale));
    }

    private void rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            transform.localScale = pointInTime.scale;
            pointsInTime.RemoveAt(0);
        }
        else
            stopRewind();
    }

    public void startRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }

    public void stopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
    }
}
