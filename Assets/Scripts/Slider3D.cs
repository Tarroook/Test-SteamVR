using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider3D : MonoBehaviour
{
    private Transform startPoint;
    private Transform endPoint;
    private Transform slider;
    [SerializeField] private bool isGrabbed = false;

    public delegate void grabAction();
    public event grabAction onGrab;

    public delegate void releaseAction();
    public event releaseAction onRelease;

    [Range(0.0f, 1.0f)]
    public float value;

    private void Start()
    {
        startPoint = gameObject.transform.Find("StartPoint");
        endPoint = gameObject.transform.Find("EndPoint");
        slider = gameObject.transform.Find("Slider");
    }

    private void Update()
    {
        slider.localPosition = new Vector3(0f, 0f, Mathf.Clamp(slider.localPosition.z, startPoint.localPosition.z, endPoint.localPosition.z));
    }

    private void FixedUpdate()
    {
        
        if (!isGrabbed)
        {
            slider.localPosition = new Vector3(0f, 0f, value * endPoint.localPosition.z);
        }
        if (isGrabbed)
        {
            value = (slider.localPosition.z / endPoint.localPosition.z);
        }
    }

    public void grabEvent()
    {
        isGrabbed = true;
        if (onGrab != null)
            onGrab();
    }

    public void releaseEvent()
    {
        isGrabbed = false;
        if (onRelease != null)
            onRelease();
    }
}
