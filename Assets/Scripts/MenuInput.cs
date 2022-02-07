using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    [SerializeField] private GameObject part1;
    [SerializeField] private GameObject part2;
    [SerializeField] private CanvasGroup partOneCanvasGroup;
    [SerializeField] private CanvasGroup partTwoCanvasGroup;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (part1.activeInHierarchy)
            {
                StartCoroutine(FadeParts());
            }
        }
    }

    public void StartCampaignOne()
    {
        SceneManagement.instance.UseCampaignOneLevels();
        CallSceneBegin();
    }

    public void StartCampaignTwo()
    {
        SceneManagement.instance.UseCampaignTwoLevels();
        CallSceneBegin();
    }

    private void CallSceneBegin()
    {
        SceneManagement.instance.GoToNextScene();
        partTwoCanvasGroup.alpha = 0.0f; // for when we go back to menu and turn it on
    }

    private IEnumerator FadeParts()
    {
        yield return StartCoroutine(Utilities.Fade(partOneCanvasGroup, 1.0f, 0.0f, 0.3f));
        part1.SetActive(false);
        partOneCanvasGroup.alpha = 1.0f; // for when we go back to menu and turn it on

        yield return null;

        partTwoCanvasGroup.alpha = 0.0f; // just for extra safety
        part2.SetActive(true);
        yield return StartCoroutine(Utilities.Fade(partTwoCanvasGroup, 0.0f, 1.0f, 0.3f));
    }
}
