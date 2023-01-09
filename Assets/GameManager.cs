using UnityEngine;

public class GameManager : MonoBehaviour
{

    private bool _playerOneIsDead = false;
    private bool _playerTwoIsDead = false;
    public int playerOneHp;
    public int playerTwoHp;

    public GameObject gameOverPanel;


    public bool PlayerOneIsDead
    {
        get { return _playerOneIsDead; }
        set { _playerOneIsDead = value; }
    }

    public bool PlayerTwoIsDead
    {
        get { return _playerTwoIsDead; }
        set { _playerTwoIsDead = value; }
    }

    private void Awake()
    {
        //playerOneHp = PlayerPrefs.GetInt("playerOneHp");
        //playerTwoHp = PlayerPrefs.GetInt("playerTwoHp");
    }
    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (_playerOneIsDead && _playerTwoIsDead)
        {
            EndGame();
        }
    }
    public void EndGame()
    {
        gameOverPanel.SetActive(true);
    }

}
