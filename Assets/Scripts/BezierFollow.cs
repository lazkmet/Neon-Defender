using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    public float speedModifier = 0.5f;

    public BezierPath startRoute;

    public BezierPath currentRoute { get; private set; } = null;

    public float tParam { get; private set; }

    protected Vector3 objectPosition;

    protected bool coroutineAllowed;

    void Start()
    {
        currentRoute = startRoute;
        tParam = 0f;
        coroutineAllowed = true;
    }

    void Update()
    {
        if (coroutineAllowed && currentRoute != null)
        {
            StartCoroutine(GoByTheRoute(currentRoute));
        }
    }

    protected virtual IEnumerator GoByTheRoute(BezierPath routeToGo)
    {
        coroutineAllowed = false;

        Vector3 p0;
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;

        while (tParam < 1)
        {
            p0 = routeToGo.controlPoints[0].position;
            p1 = routeToGo.controlPoints[1].position;
            p2 = routeToGo.controlPoints[2].position;
            p3 = routeToGo.controlPoints[3].position;

            tParam += Time.deltaTime * speedModifier * routeToGo.localMultiplier;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        currentRoute = routeToGo.nextPath;

        coroutineAllowed = true;

    }
}
