using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    public float templateHeight = 50f;
    private int[] scores; // = {0,0,0,0,0};
    private string[] names; // = {"AAA", "AAA", "AAA", "AAA", "AAA"};
    private void Awake()
    {
        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        LoadScores();
        for (int i = 0; i < 5; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);

            int place = i + 1;
            string placeText = place + ".";

            entryTransform.Find("PlaceText").GetComponent<TextMeshProUGUI>().text = placeText;
            int score = scores[i];
            entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();
            entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = names[i];

        }

        
    }

    private void LoadScores()
    {
        if (File.Exists(Application.persistentDataPath + "/highscoreList.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + 
                "/highscoreList.dat", FileMode.Open);

            HighscoreList highscoreList = (HighscoreList)bf.Deserialize(file);
            file.Close();
            scores = highscoreList.scores;
            names = highscoreList.names;
        }
    }

}

[Serializable]
public class HighscoreList
{
    public int[] scores;
    public string[] names;
}
