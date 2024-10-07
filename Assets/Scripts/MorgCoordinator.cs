using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void CoordinateEvent(float nextDelay);
public delegate void ReleaseEvent();

public class MorgCoordinator : MonoBehaviour
{
    public static event CoordinateEvent OnCoordinate;
    public static event ReleaseEvent OnRelease;

    bool coordinating;
    float nextTransition;

    [SerializeField]
    float coordinateMinTime = 1f;
    [SerializeField]
    float coordinateMaxTime = 2f;

    [SerializeField]
    float releaseMinTime = 2f;
    [SerializeField]
    float releaseMaxTime = 4f;

    void Start()
    {
        nextTransition = Random.Range(releaseMinTime, releaseMaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > nextTransition)
        {
            coordinating = !coordinating;
            var delay = coordinating ?
                    Random.Range(coordinateMinTime, coordinateMaxTime) :
                    Random.Range(releaseMinTime, releaseMaxTime);

            nextTransition = Time.timeSinceLevelLoad + delay;

            if (coordinating)
            {
                OnCoordinate?.Invoke(delay);
            }
            else
            {
                OnRelease?.Invoke();
            }
        }
        if (Time.timeSinceLevelLoad > 15f || Time.timeSinceLevelLoad > 1f && Input.anyKey)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
