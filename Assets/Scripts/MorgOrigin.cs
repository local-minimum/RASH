using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorgOrigin : MonoBehaviour
{
    Vector3 SpawnPosition;
    Morg morg;

    private void Awake()
    {
        SpawnPosition = transform.position;
        morg = GetComponent<Morg>();
    }

    private void OnEnable()
    {
        MorgCoordinator.OnCoordinate += MorgCoordinator_OnCoordinate;
        MorgCoordinator.OnRelease += MorgCoordinator_OnRelease;
    }

    private void MorgCoordinator_OnRelease()
    {
        morg.SetupTravel();
    }

    private void MorgCoordinator_OnCoordinate(float nextDelay)
    {
        morg.SetupTravel(SpawnPosition, nextDelay);
    }
}
