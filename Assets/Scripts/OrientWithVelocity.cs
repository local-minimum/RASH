using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientWithVelocity : MonoBehaviour
{
    [SerializeField]
    float noSpeedThreshold = 0.1f;

    [SerializeField]
    float attack = 0.5f;

    void Update()
    {
        var rb = GetComponent<Rigidbody2D>();
        var forward = rb.velocity;
        if (forward.magnitude < noSpeedThreshold) return;

        var a = Mathf.Atan2(forward.y, forward.x);

        transform.rotation = Quaternion.Lerp(
            transform.rotation, 
            Quaternion.Euler(0,0,a),
            attack);
    }
}
