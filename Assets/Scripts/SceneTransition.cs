using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }  // Singleton instance
    public CanvasGroup fadeCanvasGroup;  // Reference to the CanvasGroup of the fade image
    public float fadeDuration = 0.5f;      // Time for the fade in/out effect

    private void Awake()
    {
        // Ensure only one instance of SceneTransition exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist this object across scene changes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }

        // Ensure the fade is initially off
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }
    }

    // Call this method to start a fade transition
    public void Transition()
    {
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        // Fade out to black
        yield return StartCoroutine(Fade(1f));

        // Fade in (reveal the new scene)
        yield return StartCoroutine(Fade(0f));
    }

    // Call this method to start the scene transition
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Fade out to black
        yield return StartCoroutine(Fade(1f));

        // Load the new scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;  // Prevent the scene from activating immediately

        // Wait until the new scene is fully loaded
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // When the scene is ready, allow activation
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // Fade in (reveal the new scene)
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
