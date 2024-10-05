using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScratchCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Text;

    int counter = 0;

    void Start()
    {
        Text.text = $"{counter}";
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
        counter++;
        Text.text = $"{counter}";
    }
}
