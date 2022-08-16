using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameUIManager : MonoBehaviour
    {
        public TMP_Text yourTurnText;
        public TMP_Text winIndexText;
        public TMP_Text loseIndexText;
        public TMP_Text robotMoveText;
        public TMP_Text robotThinkText;
        public GameObject winContainerCanvas;
        public GameObject loseContainerCanvas;

        public static GameUIManager Instance;

        private void Start()
        {
            Instance = this;
            loseIndexText.text = "Поражений: " + PlayerPrefs.GetInt("LoseIndex", 0);
            winIndexText.text = "Побед: " + PlayerPrefs.GetInt("WinIndex", 0);
        }
    
        public void Lose()
        {
            PlayerPrefs.SetInt("LoseIndex", PlayerPrefs.GetInt("LoseIndex", 0) + 1);
            loseContainerCanvas.SetActive(true);
        }
    
        public void Win()
        {
            PlayerPrefs.SetInt("WinIndex", PlayerPrefs.GetInt("WinIndex", 0) + 1);
            winContainerCanvas.SetActive(true);
        }

        public void LoadOtherScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}