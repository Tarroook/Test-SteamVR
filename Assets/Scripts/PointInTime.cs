using UnityEngine;

public struct PointInTime
{
    public bool isNotNull;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public PointInTime(Vector3 pos, Quaternion rot, Vector3 sca)
    {
        position = pos;
        rotation = rot;
        scale = sca;

        isNotNull = true;
    }
}
