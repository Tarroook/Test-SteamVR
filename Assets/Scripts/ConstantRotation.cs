using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ConstantRotation : MonoBehaviour
{
    bool x = false;
    float xSpeed = 0f;

    bool y = true;
    float ySpeed = 0f;

    bool z = false;
    float zSpeed = 0f;

    private void Update()
    {
        if (x)
        {
            transform.Rotate(new Vector3(xSpeed, 0f, 0f));
        }
        if (y)
        {
            transform.Rotate(new Vector3(0f, ySpeed, 0f));
        }
        if (z)
        {
            transform.Rotate(new Vector3(0f, 0f, zSpeed));
        }
    }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(ConstantRotation))]
    public class ConstantRotationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ConstantRotation cr = (ConstantRotation)target;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("X", GUILayout.MaxWidth(20));
            cr.x = EditorGUILayout.Toggle(cr.x);
            EditorGUILayout.EndHorizontal();
            if (cr.x)
            {
                cr.xSpeed = EditorGUILayout.Slider(cr.xSpeed, 0f, 100f);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(20));
            cr.y = EditorGUILayout.Toggle(cr.y);
            EditorGUILayout.EndHorizontal();
            if (cr.y)
            {
                cr.ySpeed = EditorGUILayout.Slider(cr.ySpeed, 0f, 100f);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(20));
            cr.z = EditorGUILayout.Toggle(cr.z);
            EditorGUILayout.EndHorizontal();
            if (cr.z)
            {
                cr.zSpeed = EditorGUILayout.Slider(cr.zSpeed, 0f, 100f);
            }
        }
    }
    #endif
    #endregion
}
