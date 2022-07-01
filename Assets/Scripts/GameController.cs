using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text winIndexText;
    public Text loseIndexText;
    public GameObject winContainerCanvas;
    public GameObject loseContainerCanvas;
    public List<GameObject> playerPawns;
    public List<GameObject> enemyPawns;
    public float retractionForce = 5;
    public float pushForce = 35;
    public bool playerTurn = true;
    public float heightDeath = -5;

    private void Start()
    {
        loseIndexText.text = "Поражений: " + PlayerPrefs.GetInt("LoseIndex", 0);
        winIndexText.text = "Побед: " + PlayerPrefs.GetInt("WinIndex", 0);
    }

    private void Update()
    {
        for (int i = playerPawns.Count - 1; i > -1; i--)
        {
            if (playerPawns[i].transform.position.y < heightDeath)
            {
                var delObject = playerPawns[i];
                playerPawns.Remove(delObject);
                Destroy(delObject);
            }
        }
        
        for (int i = enemyPawns.Count - 1; i > -1; i--)
        {
            if (enemyPawns[i].transform.position.y < heightDeath)
            {
                var delObject = enemyPawns[i];
                enemyPawns.Remove(delObject);
                Destroy(delObject);
            }
        }

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
