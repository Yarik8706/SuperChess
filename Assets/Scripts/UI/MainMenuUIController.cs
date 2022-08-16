using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
