using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BackgroundController : MonoBehaviour
{
    private float yStart;
    private float yDifference;
    private bool isUp = false;
    private bool movingWasSpeedUp = false;

    [SerializeField] public string sceneToChangeTo;
    [SerializeField] public float movingSpeed;
    [SerializeField] private TextMeshProUGUI continueText;

    private bool hasContinueTextTransitionStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        yStart = transform.localPosition.y;
        yDifference = 222;

        hideContinueText();
    }

    // Update is called once per frame
    void Update()
    {
        VerticalMovement();
        if (!movingWasSpeedUp && !isUp && Input.anyKeyDown)
        {
            movingSpeed += 0.5f;
            movingWasSpeedUp = true;
        }
        if(isUp == true && Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneToChangeTo);
        }
    }

    private void VerticalMovement()
    {
        float yCurrent = transform.localPosition.y;
        
        if (yCurrent - movingSpeed > yStart - yDifference)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, yCurrent - movingSpeed);
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
        isUp = true;
        yield break;
    }

    private void hideContinueText () {
        continueText.color = new Color(continueText.color.r, continueText.color.g, continueText.color.b, 0f);
    }

    private void showContinueText () {
        StartCoroutine(FadeIn());
    }
}
