using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    private bool isRewinding = false;
    public float recordSeconds = 5f;

    PointInTime[] pointsInTime;
    private int sizeOfActivePoints = 0; // used as ".Lenght" because pointsInTime's actual length never changes

    // Start is called before the first frame update
    void Start()
    {
        pointsInTime = new PointInTime[(int)Mathf.Round(recordSeconds / Time.fixedDeltaTime)];
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
        insertPointIntoArray(0, new PointInTime(transform.position, transform.rotation, transform.localScale));
    }

    private void rewind()
    {
        if (sizeOfActivePoints > 0 && pointsInTime[0].isNotNull)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            transform.localScale = pointInTime.scale;
            removePointFromArray(0);
        }
        else
            stopRewind();
    }

    public void startRewind()
    {
        isRewinding = true;
    }

    public void stopRewind()
    {
        isRewinding = false;
    }

    // inserts newPoint at pos and "removes" last value
    private void insertPointIntoArray(int pos, PointInTime newPoint)
    {
        if (pos > pointsInTime.Length)// can't be fucked with throwing errors/warnings rn lmao
        {
            Debug.Log("insert point bad position");
            return;
        }

        PointInTime[] newArray = new PointInTime[pointsInTime.Length];
        for(int i = 0; i < pointsInTime.Length; i++)
        {
            if (i < pos)
                newArray[i] = pointsInTime[i];
            else if (i == pos)
                newArray[i] = newPoint;
            else
                newArray[i] = pointsInTime[i - 1];

        }
        pointsInTime = newArray;

        sizeOfActivePoints++;
        if (sizeOfActivePoints < pointsInTime.Length)
            sizeOfActivePoints = pointsInTime.Length;
    }

    private void removePointFromArray(int pos)
    {
        if(pos > pointsInTime.Length)
        {
            Debug.Log("remove point bad position");
            return;
        }

        PointInTime[] newArray = new PointInTime[pointsInTime.Length];
        for (int i = pos; i + 1 < pointsInTime.Length; i++)
        {
            newArray[i] = pointsInTime[i + 1];
        }
        pointsInTime = newArray; // don't even have to set "last" value to null cuz structs can't be null
        sizeOfActivePoints--;
    }
}
