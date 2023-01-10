using UnityEngine;

public class GameManager : MonoBehaviour
{

    private bool _playerIsDead = false;

    public int playerOneHp;
    public int playerTwoHp;

    public int score;

    public GameObject gameOverPanel;


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
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (_playerIsDead)
        {
            EndGame();
        }
    }
    public void EndGame()
    {
        gameOverPanel.SetActive(true);
    }

    public void AddToScore(int amount)
    {
        score += amount;
        PlayerPrefs.SetInt("score", score);
    }

}
