using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [SerializeField] Animator sceneTransitionAnimator;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySceneOpen()
    {
        sceneTransitionAnimator.Play("Open");
    }

    public void PlaySceneClose()
    {
        sceneTransitionAnimator.Play("Close");
    }
}
