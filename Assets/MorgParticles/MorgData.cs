using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MorgInput : MonoBehaviour
{
    [SerializeField]
    float CurrentVelocity;
    [SerializeField]
    ParticleSystem MorgBody;
    [SerializeField]
    ParticleSystem MorgTail;
    ShapeModule tailShape;
    [SerializeField]
    ParticleSystem MorgHead;
    [SerializeField]
    ParticleSystem MorgTendrilU1;
    ShapeModule tendrilShapeU1;
    [SerializeField]
    ParticleSystem MorgTendrilU2;
    ShapeModule tendrilShapeU2;
    [SerializeField]
    ParticleSystem MorgTendrilU3;
    ShapeModule tendrilShapeU3;
    [SerializeField]
    ParticleSystem MorgTendrilL1;
    ShapeModule tendrilShapeL1;
    [SerializeField]
    ParticleSystem MorgTendrilL2;
    ShapeModule tendrilShapeL2;
    [SerializeField]
    ParticleSystem MorgTendrilL3;
    ShapeModule tendrilShapeL3;
    [SerializeField]
    ParticleSystem MorgTendrilH1;
    ShapeModule tendrilShapeH1;
    [SerializeField]
    ParticleSystem MorgTendrilH2;
    ShapeModule tendrilShapeH2;
    [SerializeField]
    ParticleSystem MorgTendrilH3;
    ShapeModule tendrilShapeH3;

    Vector2 CurrentPos;
    Vector2 LastPos;

    List<ParticleSystem> ParticleSystems;

    void Start()
    {
        ParticleSystems = new List<ParticleSystem>()
        {
            MorgBody,
            MorgHead,
            MorgTail,
            MorgTendrilH1,
            MorgTendrilH2,
            MorgTendrilH3,
            MorgTendrilU1,
            MorgTendrilU2,
            MorgTendrilU3,
            MorgTendrilL1,
            MorgTendrilL2,
            MorgTendrilL3,

        };

        CurrentPos = transform.position;
        tailShape = MorgTail.shape;
        tendrilShapeH1 = MorgTendrilH1.shape;
        tendrilShapeH2 = MorgTendrilH2.shape;
        tendrilShapeH3 = MorgTendrilH3.shape;
        tendrilShapeL1 = MorgTendrilL1.shape;
        tendrilShapeL2 = MorgTendrilL2.shape;
        tendrilShapeL3 = MorgTendrilL3.shape;
        tendrilShapeU1 = MorgTendrilU1.shape;
        tendrilShapeU2 = MorgTendrilU2.shape;
        tendrilShapeU3 = MorgTendrilU3.shape;

        var seed = (uint) Mathf.FloorToInt(transform.position.magnitude);
        foreach (ParticleSystem ps in ParticleSystems)
        {
            ps.Stop();
            ps.randomSeed = seed;
            ps.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        LastPos = CurrentPos;
        CurrentPos = transform.position;
        CurrentVelocity = Vector2.Distance(LastPos, CurrentPos);

        if (CurrentVelocity > 0)
        {
            //tailShape.radius = 0.28f + (CurrentVelocity);
            tailShape.randomPositionAmount = 0.1f + CurrentVelocity;
            tailShape.radiusSpeed = 0.01f + (CurrentVelocity * 10f);

            tendrilShapeH1.randomPositionAmount = 0.002f + (CurrentVelocity * 0.1f);
            tendrilShapeH2.randomPositionAmount = 0.002f + (CurrentVelocity * 0.1f);
            tendrilShapeH3.randomPositionAmount = 0.002f + (CurrentVelocity * 0.1f);
            tendrilShapeU1.randomPositionAmount = 0.002f + (CurrentVelocity * 0.1f);
            tendrilShapeU2.randomPositionAmount = 0.002f + (CurrentVelocity * 0.1f);
            tendrilShapeU3.randomPositionAmount = 0.002f + (CurrentVelocity * 0.1f);
            tendrilShapeL1.randomPositionAmount = 0.002f + (CurrentVelocity * 0.1f);
            tendrilShapeL2.randomPositionAmount = 0.002f + (CurrentVelocity * 0.1f);
            tendrilShapeL3.randomPositionAmount = 0.002f + (CurrentVelocity * 0.1f);

            tendrilShapeH1.radiusSpeed = 0.2f + CurrentVelocity;
            tendrilShapeH2.radiusSpeed = 0.2f + CurrentVelocity;
            tendrilShapeH3.radiusSpeed = 0.2f + CurrentVelocity;
            tendrilShapeU1.radiusSpeed = 0.2f + CurrentVelocity;
            tendrilShapeU2.radiusSpeed = 0.2f + CurrentVelocity;
            tendrilShapeU3.radiusSpeed = 0.2f + CurrentVelocity;
            tendrilShapeL1.radiusSpeed = 0.2f + CurrentVelocity;
            tendrilShapeL2.radiusSpeed = 0.2f + CurrentVelocity;
            tendrilShapeL3.radiusSpeed = 0.2f + CurrentVelocity;

            tendrilShapeH1.arc = 20f + (CurrentVelocity * 20f);
            tendrilShapeH2.arc = 20f + (CurrentVelocity * 20f);
            tendrilShapeH3.arc = 20f + (CurrentVelocity * 20f);
            tendrilShapeU1.arc = 20f + (CurrentVelocity * 20f);
            tendrilShapeU2.arc = 20f + (CurrentVelocity * 20f);
            tendrilShapeU3.arc = 20f + (CurrentVelocity * 20f);
            tendrilShapeL1.arc = 20f + (CurrentVelocity * 20f);
            tendrilShapeL2.arc = 20f + (CurrentVelocity * 20f);
            tendrilShapeL3.arc = 20f + (CurrentVelocity * 20f);
        }
    }
}
