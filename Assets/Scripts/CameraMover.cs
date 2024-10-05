using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        initialZoom = GetComponent<Camera>().orthographicSize;

        nextMove = startMovingAfter;
        nextZoom = zoomAfter;
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
            cam.orthographicSize = initialZoom - Mathf.Sin(zoomProgress * Mathf.PI) * maxZoom;
            zooming = zoomProgress < 1f;
        }
    }
}
