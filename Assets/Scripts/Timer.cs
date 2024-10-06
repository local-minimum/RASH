using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Text;

    void Start()
    {
        Text.text = "";
    }

    private void OnEnable()
    {
        Health.OnDeath += Health_OnDeath;
    }

    private void OnDisable()
    {
        Health.OnDeath -= Health_OnDeath; 
    }

    private void Health_OnDeath(int score)
    {
        Text.text = score.ToString();
        enabled = false;
    }

    void Update()
    {
        Text.text = Mathf.FloorToInt(Time.timeSinceLevelLoad).ToString();
    }
}
