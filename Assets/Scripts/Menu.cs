using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Highscore;

    private void Start()
    {
        var highscore = PlayerPrefs.GetInt("Highscore", 0);
        if (highscore == 0)
        {
            Highscore.gameObject.SetActive(false);
        } else
        {
            Highscore.text = $"{highscore} seconds";
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Time.timeSinceLevelLoad > 0.5f && Input.anyKey)
        {
            SceneManager.LoadScene("Level");
        }
    }
}
