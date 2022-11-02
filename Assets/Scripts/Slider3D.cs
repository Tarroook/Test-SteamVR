using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider3D : MonoBehaviour
{
    private Transform startPoint;
    private Transform endPoint;
    private Transform slider;
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
        value = (slider.localPosition.z / endPoint.localPosition.z);
        slider.localPosition = new Vector3(0f, 0f, Mathf.Clamp(slider.localPosition.z, startPoint.localPosition.z, endPoint.localPosition.z));
    }
}
