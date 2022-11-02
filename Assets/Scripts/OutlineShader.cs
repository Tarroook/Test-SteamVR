using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineShader : MonoBehaviour
{
    public Material shader;
    
    private void Start()
    {
        checkFirstOne();
        loopCheckThroughChildren(transform);
    }

    private void createOutline(GameObject parent, Mesh mesh)
    {
        GameObject outlineObject = new GameObject(parent.name + "-Outline");
        outlineObject.tag = "Outline";
        outlineObject.transform.parent = parent.transform;
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localRotation = Quaternion.identity;
        outlineObject.transform.localScale = new Vector3(-1, 1, 1);

        outlineObject.AddComponent<MeshFilter>().mesh = mesh;
        MeshRenderer mr = outlineObject.AddComponent<MeshRenderer>();
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.material = shader;
    }

    
    private Mesh findMesh(GameObject currGameObject)
    {
        if (currGameObject.GetComponent<MeshFilter>() != null)
        {
            return currGameObject.GetComponent<MeshFilter>().mesh;
        }
        else if (currGameObject.GetComponent<SkinnedMeshRenderer>() != null)
        {
            return currGameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }
        else
            return null;
    }

    private void checkFirstOne()
    {
        Mesh m = findMesh(gameObject);
        if (m != null)
        {
            createOutline(gameObject, m);
        }
    }

    private void loopCheckThroughChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag != "Outline")
            {
                Mesh mesh = findMesh(child.gameObject);
                if (mesh != null)
                {
                    createOutline(child.gameObject, mesh);
                }
                loopCheckThroughChildren(child);
            }
        }
    }
}
