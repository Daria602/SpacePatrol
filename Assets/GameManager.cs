using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _playerIsDead = false;


    public int score;

    public bool FinishedGame = false;


    public bool PlayerIsDead
    {
        get { return _playerIsDead; }
        set { _playerIsDead = value; }
    }


    private void Awake()
    {
        PlayerPrefs.GetInt("score");
    }
    private void Start()
    {
    }

    private void Update()
    {
        if (_playerIsDead && !FinishedGame)
        {
            EndGame(false);
        }
        else if (FinishedGame)
        {
            EndGame(true);
        }
    }
    public void EndGame(bool finishedSuccess)
    {
        if (finishedSuccess)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } 
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }

    public void AddToScore(int amount)
    {
        score += amount;
        PlayerPrefs.SetInt("score", score);
    }

}
