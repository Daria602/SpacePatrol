using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;




public class GameOverScreen : MonoBehaviour
{

    public AudioMixer audioMixer;
    public TextMeshProUGUI scoreText;

    private int[] scores; //= { 0, 0, 0, 0, 0 };
    private string[] names; //= { "AAA", "AAA", "AAA", "AAA", "AAA" };
    private int score;

    public TextMeshProUGUI playerName;

    private void Awake()
    {
        score = PlayerPrefs.GetInt("score");
        SetPlayerMusicPrefs();
        scoreText.text = "Your score is " + score.ToString();
    }

    private void SetPlayerMusicPrefs()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        audioMixer.SetFloat("MasterVolume", masterVolume);
        audioMixer.SetFloat("MusicVolume", musicVolume);
        audioMixer.SetFloat("SFXVolume", sfxVolume);
    }

    public void GoBackToMenu()
    {
        SaveScore();
        SceneManager.LoadScene(1);
    }

    public void SaveScore()
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath +
            "/highscoreList.dat", FileMode.Open);

        HighscoreList highscoreList = (HighscoreList)bf.Deserialize(file);
        file.Close();
        scores = highscoreList.scores;
        names = highscoreList.names;


        // check here if any of the scores was beaten
        int indexBeatenScore = CheckIfScoreWasBeaten();

        if (indexBeatenScore != -1)
        {
            UpdateScoreList(indexBeatenScore);
        }










        // Code here to create a file if it doesnt exist
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Open(Application.persistentDataPath +
        //   "/highscoreList.dat", FileMode.Create);
        //HighscoreList highscoreList = new HighscoreList();
        //highscoreList.scores = scores;
        //highscoreList.names = names;
        //bf.Serialize(file, highscoreList);
        //file.Close();

    }

    private int CheckIfScoreWasBeaten()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (score > scores[i])
            {
                return i;
            }
        }

        return -1;
    }

    private void UpdateScoreList(int indexBeatenScore)
    {
        int scoreToCarry;
        string nameToCarry;

        string playerNameText = playerName.text.Length > 0 ?
            playerName.text : "NO NAME";

        int scoreToPut = score;
        string nameToPut = playerNameText;

        for (int i = indexBeatenScore; i < scores.Length; i++)
        {
            scoreToCarry = scores[i];
            nameToCarry = names[i];

            scores[i] = scoreToPut;
            names[i] = nameToPut;

            scoreToPut = scoreToCarry;
            nameToPut = nameToCarry;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath +
            "/highscoreList.dat", FileMode.Create);
        HighscoreList hl = new HighscoreList();
        hl.scores = scores;
        hl.names = names;
        formatter.Serialize(file, hl);
        file.Close();

    }
}
