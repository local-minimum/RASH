using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    AudioClip ScratchSound;
    [SerializeField]
    AudioClip[] SighSounds;

    [SerializeField]
    Transform ScratchCenter;

    [SerializeField]
    float dragOverRash = 2f;

    [SerializeField]
    float dragNoRash = 0.8f;

    private void OnEnable()
    {
        alive = true;

        var rb = GetComponent<Rigidbody2D>();
        rb.drag = dragNoRash;

        Rash.OnItchPhase += Rash_OnItchPhase;
        Health.OnDeath += Health_OnDeath;
    }

    private void OnDisable()
    {
        Rash.OnItchPhase -= Rash_OnItchPhase;
        Health.OnDeath -= Health_OnDeath;
    }

    bool alive;

    private void Health_OnDeath(int score)
    {
        alive = false;
    }

    private void Rash_OnItchPhase(Rash rash, Rash.ItchPhase phase)
    {
        if (phase == Rash.ItchPhase.ItchIn)
        {
            var collider = rash.GetComponentInChildren<CircleCollider2D>();
            var results = new List<Collider2D>() { GetComponentInChildren<Collider2D>() };
            var n = collider.OverlapCollider(new ContactFilter2D()
            {
                useTriggers = true,
                useLayerMask = true,
                layerMask = LayerMask.NameToLayer("Hand"),
                useDepth = false,

            },
                results);
            if (n > 0)
            {
                Debug.Log(results[0] == GetComponentInChildren<Collider2D>());
                OverlappingRashes.Add(rash);
            } else
            {
                Debug.LogWarning("Not over spawn");
            }
        }
    }

    [SerializeField]
    float minMouseForce = 1.0f;

    [SerializeField]
    float maxMouseForce = 100f;

    [SerializeField]
    float maxAtDistance = 5f;


    bool scratching;

    [SerializeField]
    float scratchWaitTime = 1.5f;
    [SerializeField]
    float scratchingTime = 1.0f;

    [SerializeField, Range(0, 1)]
    float allowScratchOnProgress = 0.9f;

    float waitForScratchStart;

    float scratchProgress =>
        Mathf.Clamp01((Time.timeSinceLevelLoad - waitForScratchStart) / (scratching ? scratchingTime : scratchWaitTime));

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
                delta.magnitude / maxAtDistance);
        }
    }

    [SerializeField]
    AnimationCurve panic;

    private void Update()
    {
        var rb = GetComponent<Rigidbody2D>();
        var rashGravity = Vector2.zero;
        foreach (var rash in Rash.Rashes)
        {
            rashGravity += rash.CalculateForce(ScratchCenter.position);
        }

        var scratch = scratchProgress;
        var mouse = MouseForce;

        rb.AddForce(Vector2.Lerp(mouse, rashGravity + mouse, panic.Evaluate(scratching || !alive ? 1f : scratch)));

        if (!scratching)
        {
            if (!alive || scratch == 1.0f || Input.GetMouseButtonDown(0) && scratch > allowScratchOnProgress)
            {
                Scratch();
            }
        } else if (scratch == 1.0f)
        {
            StopScratch();
        }
    }

    HashSet<Rash> OverlappingRashes = new HashSet<Rash>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var rash = collision.GetComponent<Rash>();
        if (rash != null)
        {
            if (scratching) rash.Scratch();

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        var rash = collision.GetComponent<Rash>();
        if (rash != null)
        {
            OverlappingRashes.Add(rash);
        }
    }

    void Scratch()
    {
        Debug.Log("Scratch");
        scratching = true;
        waitForScratchStart = Time.timeSinceLevelLoad;
        
        foreach (var rash in OverlappingRashes) rash.Scratch();

        GetComponent<Animator>().SetTrigger("Scratch");
        GetComponent<AudioSource>().PlayOneShot(ScratchSound);
    }

    void StopScratch()
    {
        Debug.Log("Stop Scratch");
        scratching = false;
        waitForScratchStart = Time.timeSinceLevelLoad;

        GetComponent<Animator>().SetTrigger("No-Scratch");
        if (alive)
        {
            GetComponent<AudioSource>().PlayOneShot(SighSounds[Random.Range(0, SighSounds.Length)]);
        }
    }
}
