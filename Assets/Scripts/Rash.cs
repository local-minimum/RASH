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

    private void OnEnable()
    {
        Rashes.Add(this);
    }

    public void OnDisable()
    {
        Rashes.Remove(this);
    }

    private void OnDestroy()
    {
        Rashes.Remove(this);
    }

    public Vector2 CalculateForce(Vector3 otherPosition)
    {
        var direction = transform.position - otherPosition;
        var distance = Vector2.SqrMagnitude(direction);
        return direction.normalized * attractionForce * intensity / distance;
    }

}
