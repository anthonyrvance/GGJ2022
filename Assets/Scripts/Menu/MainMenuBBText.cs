using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuBBText : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float timeBetweenInstances;
    [SerializeField] private float timeInstanceStaysOpen;
    [SerializeField] private float timer;

    [SerializeField] private List<string> messageOptions;
    [SerializeField] private TextMeshProUGUI messageText;

    private void Awake()
    {
        timer = timeBetweenInstances / 2.0f;
        canvasGroup.alpha = 0.0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenInstances)
        {
            timer = 0.0f;
            StartCoroutine(FadeShowUnfade());
        }
    }

    private IEnumerator FadeShowUnfade()
    {
        RectTransform orgRT = this.gameObject.GetComponent<RectTransform>();
        Vector3 orgPos = orgRT.localPosition;
        int x = Random.Range(100, 130), y = Random.Range(-50, -80);
        orgRT.localPosition = new Vector3(x, y);
        
        messageText.text = messageOptions[Random.Range(0, messageOptions.Count)];

        yield return StartCoroutine(Fade(0.0f, 1.0f));
        yield return new WaitForSeconds(timeInstanceStaysOpen);
        yield return StartCoroutine(Fade(1.0f, 0.0f));
    }

    private IEnumerator Fade(float startValue, float endValue)
    {
        float elapsedTime = 0.0f;
        canvasGroup.alpha = startValue;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, endValue, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endValue;
    }
}
