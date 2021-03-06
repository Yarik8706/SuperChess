using System.Collections.Generic;
using ActionFigures;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text winIndexText;
    public Text loseIndexText;
    public GameObject winContainerCanvas;
    public GameObject loseContainerCanvas;
    public List<Figure> playerPawns;
    public List<Figure> enemyPawns;
    public float retractionForce = 5;
    public bool playerTurn = true;
    public const float HeightDeath = -5;

    private void Start()
    {
        loseIndexText.text = "Поражений: " + PlayerPrefs.GetInt("LoseIndex", 0);
        winIndexText.text = "Побед: " + PlayerPrefs.GetInt("WinIndex", 0);
        foreach (var figure in playerPawns)
        {
            figure.gameController = this;
        }
        foreach (var figure in enemyPawns)
        {
            figure.gameController = this;
        }
    }

    private void Update()
    {
        if(winContainerCanvas.activeSelf || loseContainerCanvas.activeSelf) return; 
        if (enemyPawns.Count == 0)
        {
            PlayerPrefs.SetInt("WinIndex", PlayerPrefs.GetInt("WinIndex", 0) + 1);
            winContainerCanvas.SetActive(true);
        } 
        else if (playerPawns.Count == 0)
        {
            PlayerPrefs.SetInt("LoseIndex", PlayerPrefs.GetInt("LoseIndex", 0) + 1);
            loseContainerCanvas.SetActive(true);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
