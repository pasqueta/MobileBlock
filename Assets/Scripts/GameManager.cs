using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance { get => instance; }
    public bool GameIsPause { get => gameIsPause; }

    [SerializeField]
    TextMeshProUGUI scoreTextInGame = null;
    [SerializeField]
    TextMeshProUGUI gameResultText = null;
    [SerializeField]
    TextMeshProUGUI scoreTextInPause = null;
    [SerializeField]
    GameObject pauseUI = null;

    int playerPoint = 0;
    bool gameIsPause = false;

    public UnityEvent OnGamePause;
    public UnityEvent OnGameContinue;
    public UnityEvent OnGameOver;

    #region Untiy
    // Start is called before the first frame update
    void Start()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;

        // UnityEvent initialisation
        if (OnGamePause == null)
        {
            OnGamePause = new UnityEvent();
        }
        if (OnGameContinue == null)
        {
            OnGameContinue = new UnityEvent();
        }
        if (OnGameOver == null)
        {
            OnGameOver = new UnityEvent();
        }

        gameIsPause = false;
        pauseUI.SetActive(false);
        scoreTextInGame.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    public void AddPoint(int point)
    {
        playerPoint += point;
        scoreTextInGame.text = playerPoint.ToString();
    }

    public void RemovePoint(int point)
    {
        playerPoint -= point;
        scoreTextInGame.text = playerPoint.ToString();
    }

    public void BlockPassLine()
    {
        GameOver();
    }

    void GameOver()
    {
        gameResultText.enabled = true;
        gameResultText.text = "Game Over";
        gameResultText.color = Color.red;
        Pause();
        OnGameOver.Invoke();
    }

    /*private void OnApplicationPause(bool pause)
    {
        Pause();
    }*/

    void Pause()
    {
        gameIsPause = true;
        scoreTextInGame.enabled = false;
        pauseUI.SetActive(true);
        scoreTextInPause.text = "Score: " + scoreTextInGame.text;
        OnGamePause.Invoke();
    }

    public void PauseButton()
    {
        Pause();
    }

    public void ContinueButton()
    {
        gameIsPause = false;
        scoreTextInGame.enabled = true;
        pauseUI.SetActive(false);
        OnGameContinue.Invoke();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}