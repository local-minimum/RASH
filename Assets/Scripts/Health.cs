using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void DeathEvent(int score);

public class Health : MonoBehaviour
{
    public static event DeathEvent OnDeath;

    [SerializeField]
    Image HealthImg;

    [SerializeField]
    int maxScratches = 20;

    [SerializeField]
    GameObject GameOver;

    int scratches;
    bool alive;
    float Fill => (float) scratches / maxScratches;

    void Start()
    {
        alive = true;
        HealthImg.fillAmount = Fill; 
        GameOver.SetActive(false);
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
        if (!alive) return;

        scratches++;

        HealthImg.fillAmount = Fill;
        
        if (scratches >= maxScratches)
        {
            alive = false;
            GameOver.SetActive(true);

            var score = Mathf.FloorToInt(Time.timeSinceLevelLoad);
            PlayerPrefs.SetInt("Highscore", Mathf.Max(score, PlayerPrefs.GetInt("Highscore", 0)));
            OnDeath?.Invoke(score);
            StartCoroutine(LoadMenu());
        }
    }

    [SerializeField]
    float delayMenuWith = 2f;

    IEnumerator<WaitForSeconds> LoadMenu()
    {
        yield return new WaitForSeconds(delayMenuWith);
        SceneManager.LoadScene("Menu");
    }
}
