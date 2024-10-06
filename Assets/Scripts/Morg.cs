using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Morg : MonoBehaviour
{
    bool traveling;

    [SerializeField]
    Color TravelColor;

    [SerializeField]
    Color SurfaceColor;

    Vector3 RandomTravelTarget
    {
        get
        {

            var coords = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 11);

            coords.x = Mathf.Clamp(coords.x, 0, Screen.width);
            coords.y = Mathf.Clamp(coords.y, 0, Screen.height);

            return Camera.main.ScreenToWorldPoint(coords);
        }
    }

    [SerializeField]
    float travelDuration = 0.5f;

    float currentTravelDuration;
    float nextRandomTravel;
    float travelStartTime;
    Vector3 travelStart;
    Vector3 travelEnd;

    private void Start()
    {
        traveling = true;

        travelStart = RandomTravelTarget;
        travelEnd = travelStart;
        transform.position = travelStart;

        currentTravelDuration = travelDuration;
    }

    private void OnEnable()
    {
        Rash.OnItchPhase += Rash_OnItchPhase;
    }

    private void OnDisable()
    {
        Rash.OnItchPhase -= Rash_OnItchPhase;
    }

    [SerializeField]
    float abortForRashProb = 0.3f;

    private void Rash_OnItchPhase(Rash rash, Rash.ItchPhase phase)
    {
        if (phase == Rash.ItchPhase.ItchIn && (!targetIsRash || Random.value < abortForRashProb))
        {
            var target = rash.transform.position;
            target.z = transform.position.z;
            var offset = Random.value * rash.Size;
            var a = Random.value * 2 * Mathf.PI;
            target += new Vector3(offset * Mathf.Cos(a), offset * Mathf.Sin(a));
            Debug.Log($"Travel to rash {target}");

            SetupTravel(target, Random.Range(0.2f, 2f));
            targetIsRash = true;
        }
    }

    bool targetIsRash;

    void SetupTravel(Vector3 end, float nextDelay)
    {
            travelStart = transform.position;
            travelEnd = end;
            traveling = true;
            travelStartTime = Time.timeSinceLevelLoad;
            var direction = travelEnd - travelStart;
            currentTravelDuration = travelDuration * Mathf.Clamp(direction.magnitude / 5, 0.5f, 2f);
            nextRandomTravel = travelStartTime + currentTravelDuration + nextDelay;
            transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);   
    }

    [SerializeField]
    float orthoMagnitude = 0.4f;

    [SerializeField]
    float orthoFrequency = 3;

    private void Update()
    {
        var rend = GetComponentInChildren<SpriteRenderer>();
        rend.color = Color.Lerp(rend.color, traveling ? TravelColor : SurfaceColor, 0.5f);

        if (Time.timeSinceLevelLoad > nextRandomTravel)
        {
            SetupTravel(RandomTravelTarget, Random.Range(0, 2f));
            targetIsRash = false;
        }

        var travelProgress = Mathf.Clamp01((Time.timeSinceLevelLoad - travelStartTime) / currentTravelDuration);
        transform.position = Vector3.Lerp(travelStart, travelEnd, travelProgress);

        var delta = travelEnd - travelStart;
        var distance = delta.magnitude;
        var travelled = distance * travelProgress;
        var ortho = new Vector3(delta.y, -delta.x, 0).normalized * Mathf.Sin(travelled / orthoFrequency) * orthoMagnitude;
        transform.position += ortho;

        if (travelProgress == 1)
        {
            if (targetIsRash)
            {
                traveling = false;
            }
    }
}
}
