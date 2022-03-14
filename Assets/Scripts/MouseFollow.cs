using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MouseFollow : MonoBehaviour
{
    public Transform floor = null;
    public LayerMask terrainToCheck;
    public Camera cam;
    public Material valid;
    public Material invalid;

    private Vector3 minPoint = new Vector3 (-26.5f, 0, -20);
    private Vector3 maxPoint = new Vector3(26.5f, 0, 20);
    private Collider coll;
    private bool everyOther;
    private Material[] originalMats;
    private Renderer[] towerMats;
    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        coll = GetComponent<Collider>();
        everyOther = true;
        towerMats = gameObject.GetComponentsInChildren<Renderer>();
        originalMats = new Material[towerMats.Length];
        for (int i = 0; i < towerMats.Length; i++) {
            originalMats[i] = towerMats[i].material;
        }
        minPoint.x += coll.bounds.extents.x;
        maxPoint.x -= coll.bounds.extents.x;
        minPoint.z += coll.bounds.extents.z;
        maxPoint.z -= coll.bounds.extents.z;
    }
    private void Update()
    {
        Vector3 newPos = cam.ScreenToWorldPoint(Input.mousePosition);
        newPos.x = Mathf.Clamp(newPos.x, minPoint.x, maxPoint.x);
        newPos.z = Mathf.Clamp(newPos.z, minPoint.z, maxPoint.z);
        newPos.y = floor.position.y;
        transform.position = newPos;
        everyOther = !everyOther;
        if (everyOther) {
            foreach (Renderer r in towerMats) {
                r.material = ValidPlacement() ? valid : invalid;
            }
        }
    }
    public bool ValidPlacement() {
        Collider[] hits = Physics.OverlapBox(coll.bounds.center, coll.bounds.extents, Quaternion.identity, terrainToCheck);
        Debug.Log(hits.Length);
        bool returnVal = hits.Length < 2; //if you collide with anything aside from yourself
        return returnVal;
    }
    public void RestoreMaterials() {
        for (int i = 0; i < towerMats.Length; i++) {
            towerMats[i].material = originalMats[i];
        }
    }
}
