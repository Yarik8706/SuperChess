using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}
