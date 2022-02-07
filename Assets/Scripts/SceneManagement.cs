using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;

    [Header("Fading")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;

    [Header("Objects to Turn On or Off")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameplayUI;

    [Header("LEVELS")]
    [SerializeField] private List<string> currentPlaythroughLevels;
    [SerializeField] private List<string> campaignOneLevels;
    [SerializeField] private List<string> campaignTwoLevels;
    [SerializeField] private int currentSceneIndex;

    public delegate void SceneUnload();
    public static event SceneUnload SceneUnloading;
    public delegate void SceneLoad();
    public static event SceneLoad SceneLoaded;

    public List<string> CurrentPlaythroughLevels
    {
        get { return currentPlaythroughLevels; }
        set { currentPlaythroughLevels = value; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        currentSceneIndex = 0;
    }

    public void ReloadCurrent()
    {
        StartCoroutine(SceneAdditiveUnload(currentPlaythroughLevels[currentSceneIndex]));
        StartCoroutine(SceneAdditiveLoad(currentPlaythroughLevels[currentSceneIndex]));
    }

    public void GoToNextScene()
    {
        if (currentSceneIndex + 1 == currentPlaythroughLevels.Count)
        {
            StartCoroutine(SceneAdditiveUnload(currentPlaythroughLevels[currentSceneIndex]));
            currentSceneIndex = 0;
            GameManager.instance.GoBackToMainMenu();
            GoBackToMainMenu();
            return;
        }

        if (currentSceneIndex != 0)
            StartCoroutine(SceneAdditiveUnload(currentPlaythroughLevels[currentSceneIndex]));

        ++currentSceneIndex;
        StartCoroutine(SceneAdditiveLoad(currentPlaythroughLevels[currentSceneIndex]));
    }

    public void GoBackToMainMenu()
    {
        StartCoroutine(SceneAdditiveUnload(currentPlaythroughLevels[currentSceneIndex]));
    }

    public void ReceiveLoad(string sceneName)
    {
        StartCoroutine(SceneAdditiveLoad(sceneName));
    }

    public void ReceiveUnload(string sceneName)
    {
        StartCoroutine(SceneAdditiveUnload(sceneName));
    }

    public void UseCampaignOneLevels()
    {
        currentPlaythroughLevels = campaignOneLevels;
    }

    public void UseCampaignTwoLevels()
    {
        currentPlaythroughLevels = campaignTwoLevels;
    }

    private IEnumerator SceneAdditiveLoad(string sceneName)
    {
        yield return StartCoroutine(PreOp());
        yield return StartCoroutine(Load(sceneName));
        SceneLoaded(); // event
        yield return StartCoroutine(PostOp());
    }

    private IEnumerator SceneAdditiveUnload(string sceneName)
    {
        yield return StartCoroutine(PreOp());
        yield return StartCoroutine(Unload(sceneName));
        yield return StartCoroutine(PostOp());
    }

    // fade to black
    private IEnumerator PreOp()
    {
        yield return StartCoroutine(Utilities.Fade(canvasGroup, 0.0f, 1.0f, 1.0f));
    }

    // fade from black
    private IEnumerator PostOp()
    {
        yield return StartCoroutine(Utilities.Fade(canvasGroup, 1.0f, 0.0f, 1.0f));
    }

    private IEnumerator Load(string sceneName)
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);

        AsyncOperation sceneOP = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!sceneOP.isDone)
        {
            // timer for animation if we have one
            yield return null;
        }
    }

    private IEnumerator Unload(string sceneName)
    {
        mainMenuUI.SetActive(true);
        gameplayUI.SetActive(false);

        SceneUnloading();

        AsyncOperation sceneOP = SceneManager.UnloadSceneAsync(sceneName);

        while (!sceneOP.isDone)
        {
            // timer for animation if we have one
            yield return null;
        }
    }
}
