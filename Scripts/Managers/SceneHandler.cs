using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;

    [SerializeField] float minimumTransitionTime;
    Timer transitionTimer = new Timer();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {   
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        UiManager.instance.PlaySceneClose();
        transitionTimer.StartTimer(minimumTransitionTime);
        while (transitionTimer.IsGreaterThatZero())
        {
            transitionTimer.Tick(Time.deltaTime);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

        UiManager.instance.PlaySceneOpen();
        transitionTimer.StartTimer(minimumTransitionTime);
        while (transitionTimer.IsGreaterThatZero())
        {
            transitionTimer.Tick(Time.deltaTime);
            yield return null;
        }
    }
}
