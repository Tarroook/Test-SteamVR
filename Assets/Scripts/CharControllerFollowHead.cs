using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharControllerFollowHead : MonoBehaviour
{
    private CharacterController charController;
    public Transform centerEye;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        Vector3 newCenter = transform.InverseTransformVector(centerEye.position - transform.position);
        charController.center = new Vector3(newCenter.x, charController.center.y, newCenter.z);
    }
}
