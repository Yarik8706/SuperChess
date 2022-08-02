using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseGameController : MonoBehaviour
{
    public Text winIndexText;
    public Text loseIndexText;
    public GameObject winContainerCanvas;
    public GameObject loseContainerCanvas;
    public const float HeightDeath = -5;
    public bool isGameOver;
    public static BaseGameController Instance;
    public readonly List<IBaseController> Controllers = new List<IBaseController>();
    [HideInInspector] public int activeIndexController;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        loseIndexText.text = "Поражений: " + PlayerPrefs.GetInt("LoseIndex", 0);
        winIndexText.text = "Побед: " + PlayerPrefs.GetInt("WinIndex", 0);
        
        StartCoroutine(Playing());
    }

    private IEnumerator Playing()
    {
        Controllers[1].Active();
        yield return new WaitUntil(() => Controllers[1].IsActive);
        while (!isGameOver)
        {
            for (int i = 0; i < Controllers.Count; i++)
            {
                activeIndexController = i;
                Controllers[i].Active();
                yield return new WaitUntil(() => Controllers[i].IsActive);
                if(isGameOver) break;
            }
        }

        foreach (var controller in Controllers)
        {
            controller.GameOver();
        }
    }

    public List<IBaseController> GetControllersWithoutCurrentController(IBaseController controller)
    {
        return new List<IBaseController>(Controllers.Where(i => i != controller));
    }

    public void Lose()
    {
        isGameOver = true;
        PlayerPrefs.SetInt("LoseIndex", PlayerPrefs.GetInt("LoseIndex", 0) + 1);
        loseContainerCanvas.SetActive(true);
    }

    public void Win()
    {
        isGameOver = true;
        PlayerPrefs.SetInt("WinIndex", PlayerPrefs.GetInt("WinIndex", 0) + 1);
        winContainerCanvas.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}