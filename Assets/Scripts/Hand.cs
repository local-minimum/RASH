using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    Transform ScratchCenter;

    [SerializeField]
    float dragOverRash = 2f;

    [SerializeField]
    float dragNoRash = 0.8f;

    private void OnEnable()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.drag = dragNoRash;
    }


    [SerializeField]
    float minMouseForce = 1.0f;

    [SerializeField]
    float maxMouseForce = 100f;

    [SerializeField]
    float maxAtSqDistance = 20f;

    Vector2 MouseForce
    {
        get
        {
            var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = transform.position.z;

            var delta = mouse - transform.position;

            return delta.normalized * Mathf.Lerp(
                minMouseForce,
                maxMouseForce,
                delta.sqrMagnitude / maxAtSqDistance);
        }
    }

    private void Update()
    {
        var rb = GetComponent<Rigidbody2D>();
        var rashGravity = Vector2.zero;
        foreach (var rash in Rash.Rashes)
        {
            //if (OverlappingRashes.Contains(rash)) continue;

            rashGravity += rash.CalculateForce(ScratchCenter.position);
        }


        rb.AddForce(rashGravity + MouseForce);
    }

    HashSet<Rash> OverlappingRashes = new HashSet<Rash>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var rash = collision.GetComponent<Rash>();
        if (rash != null)
        {
            OverlappingRashes.Add(rash);

            var rb = GetComponent<Rigidbody2D>();
            rb.drag = dragOverRash;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var rash = collision.GetComponent<Rash>();
        if (rash != null)
        {
            OverlappingRashes.Remove(rash);
            if (OverlappingRashes.Count == 0)
            {
                var rb = GetComponent<Rigidbody2D>();
                rb.drag = dragNoRash;
            } 
        }
    }
}
