using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rash : MonoBehaviour
{
    public static List<Rash> Rashes = new List<Rash>();

    [SerializeField, Range(0, 1)]
    float intensity;

    [SerializeField]
    float attractionForce = 20f;

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

    private void OnEnable()
    {
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
        return direction.normalized * attractionForce * intensity / distance;
    }

    float PhaseDuration => Time.timeSinceLevelLoad - phaseStart;

    void SetItch(float magnitude)
    {
        intensity = Mathf.Clamp01(magnitude);
        GetComponent<SpriteRenderer>().color = Color.Lerp(noItchColor, fullItchColor, intensity);
    }

    public void Scratch()
    {
        if (Phase == ItchPhase.Itching || Phase == ItchPhase.ItchOut) {
            Phase = ItchPhase.Itching;
            intensity = 1f;
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
