using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class StartXR : MonoBehaviour
{
    public bool startInVr = true;
    public GameObject vrPrefab;
    public GameObject flatScreenPrefab;
    private void Awake()
    {
        if (startInVr)
        {
            XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Instantiate(vrPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Instantiate(flatScreenPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    private void OnApplicationQuit()
    {
        List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();

        SubsystemManager.GetInstances<XRDisplaySubsystem>(displaySubsystems);
        foreach (XRDisplaySubsystem subsystem in displaySubsystems)
        {
            if (subsystem.running)
            {
                stopXR();
                break;
            }
        }
    }

    private void stopXR()
    {
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
    }
}
