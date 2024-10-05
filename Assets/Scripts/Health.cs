using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    Image HealthImg;

    [SerializeField]
    int maxScratches = 20;

    int scratches;

    float Fill => (float) scratches / maxScratches;

    void Start()
    {
        HealthImg.fillAmount = Fill; 
    }

    private void OnEnable()
    {
        Rash.OnScratch += Rash_OnScratch;
    }

    private void OnDisable()
    {
        Rash.OnScratch -= Rash_OnScratch;
    }

    private void Rash_OnScratch(Rash rash)
    {
        scratches++;

        HealthImg.fillAmount = Fill; 
    }
}
