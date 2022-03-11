using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPrep : MonoBehaviour
{
    public LineRenderer path;
    public LineRenderer glow;
    private BezierPath[] routes = { };
    private void Awake()
    {
        routes = GetComponentsInChildren<BezierPath>();
        //Each path in renderer has 10 positions for the line to render to, plus the final position which is ignored by the loop
        path.positionCount = (routes.Length * 10) + 1;
        glow.positionCount = (routes.Length * 10) + 1;
        int currentIndex = 0;
        Vector3 currentPosition = Vector3.zero;

        foreach (BezierPath r in routes) {
            //set first line position closer to the next, to fix sharp turns
            float t = 0.05f;
            currentPosition = Mathf.Pow(1 - t, 3) * r.controlPoints[0].position + 3 * Mathf.Pow(1 - t, 2) * t * r.controlPoints[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * r.controlPoints[2].position + Mathf.Pow(t, 3) * r.controlPoints[3].position;
            path.SetPosition(currentIndex, currentPosition);
            currentPosition.y -= 0.1f;
            glow.SetPosition(currentIndex, currentPosition);
            currentIndex++;

            for (t = 0.1f; t <= 1; t += 0.1f)
            {
                currentPosition = Mathf.Pow(1 - t, 3) * r.controlPoints[0].position + 3 * Mathf.Pow(1 - t, 2) * t * r.controlPoints[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * r.controlPoints[2].position + Mathf.Pow(t, 3) * r.controlPoints[3].position;

                path.SetPosition(currentIndex, currentPosition);
                currentPosition.y -= 0.1f;
                glow.SetPosition(currentIndex, currentPosition);

                currentIndex++;
            }
        }

        if (routes.Length > 0)
        { //Set final position
            currentPosition = routes[routes.Length-1].controlPoints[3].position;
            path.SetPosition(currentIndex, currentPosition);
            currentPosition.y -= 0.1f;
            glow.SetPosition(currentIndex, currentPosition);
            currentIndex++;
        }

        path.gameObject.SetActive(true);
        glow.gameObject.SetActive(true);

        BakeCollider();
    }

    private void BakeCollider()
    { 
        //code from https://forum.unity.com/threads/how-can-i-add-collider-to-my-line-renderer.947151/
        Mesh mesh = new Mesh();
        mesh.name = "Path mesh";
        glow.BakeMesh(mesh);
        mesh.Optimize();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        glow.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
