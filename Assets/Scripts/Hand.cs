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

    private void Update()
    {
        var rb = GetComponent<Rigidbody2D>();
        var force = Vector2.zero;
        foreach (var rash in Rash.Rashes)
        {
            if (OverlappingRashes.Contains(rash)) continue;

            force += rash.CalculateForce(ScratchCenter.position);
        }

        rb.AddForce(force);
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
