using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
        if (Time.timeSinceLevelLoad > 0.5f && Input.anyKey)
        {
            SceneManager.LoadScene("Level");
        }
    }
}
