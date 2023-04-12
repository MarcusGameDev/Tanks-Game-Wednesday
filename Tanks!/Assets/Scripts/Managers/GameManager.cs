using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public HighScores m_HighScores;

    // reference to the overlay text to display winning text,etc.
    public TextMeshProUGUI m_MessageText;
    public TextMeshProUGUI m_TimerText;

    public GameObject m_HighScorePanel;
    public TextMeshProUGUI m_HighScoresText;

    public Button m_NewGameButton;
    public Button m_HighScoresButton;


    // Will check each object in this array and determine whether or not the tank is 'alive'. When there is only one tank left alive, the GameManager will end the game.
    public GameObject[] m_Tanks;

    // Used to count the time the player survived in game. Use this value as a score, with players that destroy the enemy tank faster ranking higher.
    private float m_gameTime = 0;
    // Added 'public accessor funcion' (called a 'property') so we can access this variable from another script.
    public float gameTime { get { return m_gameTime; } }


    // Store the gamestate
    public enum GameState
    {
        Start,
        Playing,
        GameOver
    };

    private GameState m_GameState;
    public GameState State {  get { return m_GameState; } }

    private void Awake()
    {
        m_GameState = GameState.Start;
    }

    // Called after the Awake function. 
    private void Start()
    {
       // Disables all tanks
        for (int i = 0; i < m_Tanks.Length; i++) 
        {
            m_Tanks[i].SetActive(false);
        }

        // Hide Timer text, and set message text
        m_TimerText.gameObject.SetActive(false);
        m_MessageText.text = "Get Ready";

        // Hide UI elements when game starts
        m_HighScorePanel.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (m_GameState)
        {
            case GameState.Start:
                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    //Active timer text and reset message text
                    m_TimerText.gameObject.SetActive(true);
                    m_MessageText.text = "";

                    m_GameState = GameState.Playing;

                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);
                    }
                }
                break;
            case GameState.Playing:
                bool isGameOver = false;

                m_gameTime += Time.deltaTime;
                int seconds = Mathf.RoundToInt(m_gameTime);
                // Play the timer text
                m_TimerText.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));

                if (OneTankLeft() == true)
                {
                    isGameOver = true;
                }
                else if (IsPlayerDead() == true)
                {
                    isGameOver = true;
                }
                if (isGameOver== true)
                {
                    m_GameState = GameState.GameOver;
                    // Turn off timer and add message for winning and losing depending on scenario
                    m_TimerText.gameObject.SetActive(false);

                    //Activate button UI
                    m_NewGameButton.gameObject.SetActive(true);
                    m_HighScoresButton.gameObject.SetActive(true);

                    if(IsPlayerDead() == true)
                    {
                        m_MessageText.text = "TRY AGAIN";
                    }
                    else
                    {
                        m_MessageText.text = "Winner";

                        // Save the score
                        m_HighScores.AddScore(Mathf.RoundToInt(m_gameTime));
                        m_HighScores.SaveScoresToFile();
                    }
                }
                break;
            case GameState.GameOver:
                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    m_gameTime = 0;
                    m_GameState = GameState.Playing;
                    // Reset message text, turn on timer
                    m_MessageText.text = "";
                    m_TimerText.gameObject.SetActive(true);

                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);
                    }
                }
                break;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    // Check how many tanks are left in the scene. If only one tank is remaining the Update() function will stop the game.
    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == true)
            {
                numTanksLeft++;
            }
        }
        return numTanksLeft <= 1;
    }

    //Will check if the player tank is destroyed. Update() will end the game as soon as the player dies.
    private bool IsPlayerDead()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == false)
            {
                if (m_Tanks[i].tag == "Player")
                    return true;
            }
        }
        return false;
    }

    public void OnNewGame()
    {
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);
        m_HighScorePanel.gameObject.SetActive(false);

        m_gameTime = 0;
        m_GameState = GameState.Playing;
        m_TimerText.gameObject.SetActive(true);
        m_MessageText.text = "";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(true);
        }
    }

    public void OnHighScores()
    {
        m_MessageText.text = "";

        m_HighScoresButton.gameObject.SetActive(false);
        m_HighScorePanel.SetActive(true);

        string text = "";
        for (int i = 0; i < m_HighScores.scores.Length; i++)
        {
            int seconds = m_HighScores.scores[i];
            text += string.Format("{0:D2}:{1:D2}\n", (seconds / 60), (seconds % 60));
        }
        m_HighScoresText.text = text;
    }
}
