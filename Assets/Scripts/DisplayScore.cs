using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    public GameManager gameManager;
    private string scoreDisplayed;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        scoreDisplayed = gameManager.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplayed = gameManager.score.ToString();
        gameObject.GetComponent<TextMeshProUGUI>().text = scoreDisplayed;
    }
}
