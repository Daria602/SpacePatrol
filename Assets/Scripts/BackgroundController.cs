using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BackgroundController : MonoBehaviour
{
    public float yStart;
    public float yDifference;
    public float toSubstract; // used to move background 

    [SerializeField] private TextMeshProUGUI continueText;

    private bool hasContinueTextTransitionStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        yStart = transform.localPosition.y;
        yDifference = 222;
        toSubstract = 0.2f;

        hideContinueText();
    }

    // Update is called once per frame
    void Update()
    {
        VerticalMovement();
    }

    private void VerticalMovement()
    {
        float yCurrent = transform.localPosition.y;
        
        if (yCurrent - toSubstract > yStart - yDifference)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, yCurrent - toSubstract);
        } else if (!hasContinueTextTransitionStarted) {
            hasContinueTextTransitionStarted = true;
            showContinueText();
        }
    }

    private IEnumerator FadeIn () {
        float duration = 2f;
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            continueText.color = new Color(continueText.color.r, continueText.color.g, continueText.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private void hideContinueText () {
        continueText.color = new Color(continueText.color.r, continueText.color.g, continueText.color.b, 0f);
    }

    private void showContinueText () {
        StartCoroutine(FadeIn());
    }
}
