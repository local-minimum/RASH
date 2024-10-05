using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    [SerializeField]
    float startMovingAfter = 10f;

    [SerializeField]
    float minMoveTime = 1f;
    [SerializeField]
    float maxMoveTime = 4f;
    float nextMove;

    [SerializeField]
    float minMoveSpeed = 1f;
    [SerializeField]
    float maxMoveSpeed = 3f;

    Vector2 targetVelocity;

    [SerializeField]
    float zoomAfter = 20f;

    [SerializeField]
    float maxZoom = 1.5f;

    [SerializeField]
    float minZoomDuration = 4f;
    [SerializeField]
    float maxZoomDuration = 7f;
    [SerializeField]
    float minZoomPause = 1f;
    [SerializeField]
    float maxZoomPause = 4f;
    float nextZoom;
    float zoomStart;
    float zoomDuration;
    bool zooming;

    float initialZoom;

    [SerializeField]
    List<Transform> Edges = new List<Transform>();
    List<Vector2> EdgeOffsets = new List<Vector2>();

    private void Start()
    {
        initialZoom = GetComponent<Camera>().orthographicSize;

        nextMove = startMovingAfter;
        nextZoom = zoomAfter;

        EdgeOffsets.AddRange(Edges.Select(e => new Vector2(e.transform.localPosition.x, e.localPosition.y)));
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > nextMove)
        {
            nextMove = Time.timeSinceLevelLoad + Random.Range(minMoveTime, maxMoveTime);
            var a = Mathf.PI * 2 * Random.value;
            targetVelocity = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * Random.Range(minMoveSpeed,maxMoveSpeed);
        }

        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, 0.5f);

        if (Time.timeSinceLevelLoad > nextZoom)
        {
            zoomStart = Time.timeSinceLevelLoad;
            zoomDuration = Random.Range(minZoomDuration, maxZoomDuration);
            nextZoom = zoomStart + zoomDuration + Random.Range(minZoomPause, maxZoomPause);
            zooming = true;
        }

        if (zooming)
        {
            float zoomProgress = Mathf.Clamp01((Time.timeSinceLevelLoad - zoomStart) / zoomDuration);

            var cam = GetComponent<Camera>();
            var zoom = Mathf.Sin(zoomProgress * Mathf.PI) * maxZoom;
            cam.orthographicSize = initialZoom - zoom;
            var factor = (initialZoom - zoom) / initialZoom;
            for (int i = 0, l = Edges.Count; i < l; i++)
            {
                var edge = Edges[i];
                var offset = EdgeOffsets[i];
                edge.transform.localPosition = new Vector3(offset.x * factor, offset.y * factor, edge.transform.localPosition.z);
            }
            zooming = zoomProgress < 1f;
        }
    }
}
