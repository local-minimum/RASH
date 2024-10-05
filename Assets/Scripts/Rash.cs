using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ScratchRashEvent(Rash rash);

public class Rash : MonoBehaviour
{
    public static event ScratchRashEvent OnScratch;

    public static HashSet<Rash> Rashes = new HashSet<Rash>();

    [SerializeField, Range(0, 1)]
    float intensity;


    float size = 1f;

    [SerializeField]
    float attractionForce = 20f;
    float AttractionForce => attractionForce * size;

    [SerializeField]
    Color noItchColor;
    [SerializeField]
    Color fullItchColor;
    [SerializeField]
    Color inactiveItchColor;

    [SerializeField]
    float itchInDuration = 1;
    [SerializeField]
    float itchingDuration = 2;
    [SerializeField]
    float itchOutDuration = 1;

    private enum ItchPhase { Inactive, ItchIn, Itching, ItchOut  };
    private ItchPhase _phase;
    private ItchPhase Phase
    {
        get => _phase;
        set
        {
            _phase = value;
            phaseStart = Time.timeSinceLevelLoad;
        }
    }

    private float phaseStart;

    int scratches = 0;

    private void OnEnable()
    {
        StartItch();
    }
    
    public void StartItch(float magnitude = 1f)
    {
        size = magnitude;
        transform.localScale = Vector3.one * size;
        scratches++;
        Rashes.Add(this);
        Phase = ItchPhase.ItchIn;
        SetItch(0f);
    }

    public void OnDisable()
    {
        Rashes.Remove(this);
    }

    private void OnDestroy()
    {
        Rashes.Remove(this);
    }

    [SerializeField]
    float minDistance = 0.5f;

    public Vector2 CalculateForce(Vector3 otherPosition)
    {
        var direction = transform.position - otherPosition;
        var distance = Mathf.Max(minDistance, Vector2.SqrMagnitude(direction));
        return direction.normalized * AttractionForce * intensity / distance;
    }

    float PhaseDuration => Time.timeSinceLevelLoad - phaseStart;

    void SetItch(float magnitude)
    {
        intensity = Mathf.Clamp01(magnitude);
        GetComponent<SpriteRenderer>().color = Color.Lerp(noItchColor, fullItchColor, intensity);
    }

    [SerializeField]
    int maxScratches = 3;

    public void Scratch()
    {
        if (Phase == ItchPhase.Itching || Phase == ItchPhase.ItchOut)
        {
            scratches++;
            if (scratches < maxScratches) {
                Phase = ItchPhase.Itching;
                intensity = 1f;
            }
        
            OnScratch?.Invoke(this);
        }
    }

    private void Update()
    {
        var duration = PhaseDuration;

        if (Phase == ItchPhase.Inactive)
        {
            var rend = GetComponent<SpriteRenderer>();
            rend.color = Color.Lerp(
                    rend.color,
                    inactiveItchColor,
                    0.25f); 
            if (duration > 1.0f) gameObject.SetActive(false);
        }
        else if (Phase == ItchPhase.ItchIn)
        {
            SetItch(duration / itchInDuration);
            if (intensity == 1f)
            {
                Phase = ItchPhase.Itching;
            }
        }
        else if (Phase == ItchPhase.Itching)
        {
            var rend = GetComponent<SpriteRenderer>();
            rend.color = Color.Lerp(
                    rend.color,
                    fullItchColor,
                    0.5f); 
            if (duration > itchingDuration)
            {
                Phase = ItchPhase.ItchOut;
            }
        }
        else if (Phase == ItchPhase.ItchOut)
        {
            SetItch(1 - duration / itchOutDuration);
            if (intensity == 0f)
            {
                Phase = ItchPhase.Inactive;
            } 
        }
    }
}
