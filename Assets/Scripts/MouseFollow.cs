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
    }
    private void Update()
    {
        Vector3 newPos = cam.ScreenToWorldPoint(Input.mousePosition);
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
        Collider[] hits = Physics.OverlapBox(transform.position, coll.bounds.extents, Quaternion.identity, terrainToCheck);
        bool returnVal = hits.Length < 2; //if you collide with anything aside from yourself
        return returnVal;
    }
    public void RestoreMaterials() {
        for (int i = 0; i < towerMats.Length; i++) {
            towerMats[i].material = originalMats[i];
        }
    }
}
