using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public bool isRewinding = false;
    public bool isHolding = false;
    private bool wasHolding = false;

    public float recordSeconds = 5f;
    [Range(0f, 1f)]
    public float time = 0.0f;

    PointInTime[] pointsInTime;
    public int sizeOfActivePoints = 0; // used as ".Lenght" because pointsInTime's actual length never changes

    public int maxPoints;
    public Slider3D cursor;

    public delegate void rewindPointAction();
    public event rewindPointAction onRewindPoint;

    public delegate void recordPointAction();
    public event recordPointAction onRecordPoint;

    public delegate void stopRewindAction();
    public event stopRewindAction onStopRewind;

    public delegate void holdTimeAction(int index);
    public event holdTimeAction onHoldTime;

    public delegate void releaseTimeAction();
    public event releaseTimeAction onReleaseTime;

    private bool doOnce = false;
    private PointInTime[] storageArray; // Reused to not make garbage


    private void OnEnable()
    {
        if (cursor == null)
            cursor = GameObject.FindGameObjectWithTag("PlayerTimeSlider").GetComponent<Slider3D>();
        if (cursor == null)
            Debug.Log("No time cursor found");
        
        cursor.onGrab += startHolding;
        cursor.onRelease += stopHolding;
    }

    private void OnDisable()
    {
        cursor.onGrab -= startHolding;
        cursor.onRelease -= stopHolding;
    }


    // 1f / Time.fixedDeltaTime is the amounts of fixedUpdate called in a second, so multiply it by x to record x seconds
    // And also round it because Count is int
    private void Start()
    {
        maxPoints = (int)Mathf.Round(recordSeconds / Time.fixedDeltaTime);
        pointsInTime = new PointInTime[maxPoints];
    }

    // Update is called once per frame
    private void Update()
    {
        if (!doOnce)
        {
            maxPoints = (int)Mathf.Round(recordSeconds / Time.fixedDeltaTime);
            pointsInTime = new PointInTime[maxPoints];
            storageArray = new PointInTime[maxPoints];
            doOnce = true;
        }

        time = (float)sizeOfActivePoints / maxPoints;

        if (!isHolding)
            cursor.value = time;
        cursor.value = Mathf.Clamp(cursor.value, 0f, time);         
        wasHolding = isHolding;
    }

    private void FixedUpdate()
    {
        if (isRewinding)
            rewind();
        else if (isHolding)
            hold(cursor.value);
        else if (!isHolding && wasHolding)
            release(hold(cursor.value));
        else
            record();
    }

    
    private void record()// since sizeOfActivePoints = 0 by default it'll then be incremented
    {
        //insertPointIntoArray(0, new PointInTime(transform.position, transform.rotation, transform.localScale));
        addPointAtEnd(new PointInTime(transform.position, transform.rotation, transform.localScale));
        if (onRecordPoint != null)
            onRecordPoint();
    }

    // timeToHold should be a value between 0 and 1
    //index needs to start at 0 and go up to sizeOfActivePoints - 1
    private int hold(float timeToHold)
    {
        if (sizeOfActivePoints > 0 && pointsInTime[0].isNotNull)
        {
            int index = (int)Mathf.Round(timeToHold * maxPoints);
            index = (int)Mathf.Clamp(index, 0f, sizeOfActivePoints - 1);
            PointInTime pointInTime = pointsInTime[index];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            transform.localScale = pointInTime.scale;
            //Debug.Log("index = " + index + " math = " + timeToHold * maxPoints);
            if (onHoldTime != null)
                onHoldTime(index);
            return index;
        }
        return -1;
    }

    private void release(int index)
    {
        cleanAfterIndex(index);
        if (onReleaseTime != null)
            onReleaseTime();
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
            if (onRewindPoint != null)
                onRewindPoint();
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
        if (onStopRewind != null)
            onStopRewind();
    }

    public void startHolding()
    {
        isHolding = true;
    }

    public void stopHolding()
    {
        isHolding = false;
    }


    private void addPointAtEnd(PointInTime newPoint)
    {
        bool added = false;
        for(int i = 0; i < pointsInTime.Length; i++)
        {
            if (!pointsInTime[i].isNotNull && !added)
            {
                pointsInTime[i] = newPoint;
                added = true;
                break;
            }
        }

        if (!added)// full so we add it at the end and move everything
        {
            for (int i = 0; i + 1 < pointsInTime.Length; i++)
            {
                storageArray[i] = pointsInTime[i + 1];
            }
            storageArray[pointsInTime.Length - 1] = newPoint;
            pointsInTime = storageArray;
        }

        if (sizeOfActivePoints < pointsInTime.Length)
            sizeOfActivePoints++;
    }

    private void cleanAfterIndex(int index)
    {
        for(int i = index + 1; i < pointsInTime.Length; i++)
        {
            pointsInTime[i].isNotNull = false;
        }
        sizeOfActivePoints = index + 1;
    }


    // inserts newPoint at pos and "removes" last value
    private void insertPointIntoArray(int pos, PointInTime newPoint)
    {
        if (pos > pointsInTime.Length)// can't be fucked with throwing errors/warnings rn lmao
        {
            Debug.Log("insert point bad position");
            return;
        }

        for(int i = 0; i < pointsInTime.Length; i++)
        {
            if (i < pos)
                storageArray[i] = pointsInTime[i];
            else if (i == pos)
                storageArray[i] = newPoint;
            else
                storageArray[i] = pointsInTime[i - 1];

        }
        pointsInTime = storageArray;

        sizeOfActivePoints++;
        if (sizeOfActivePoints > pointsInTime.Length)
            sizeOfActivePoints = pointsInTime.Length;
    }

    private void removePointFromArray(int pos)
    {
        if(pos > pointsInTime.Length)
        {
            Debug.Log("remove point bad position");
            return;
        }

        for (int i = pos; i + 1 < pointsInTime.Length; i++)
        {
            storageArray[i] = pointsInTime[i + 1];
        }
        pointsInTime = storageArray; // don't even have to set "last" value to null cuz structs can't be null
        sizeOfActivePoints--;
    }
}
