using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace TowerDefence
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button m_ContinueButton;

        private void Start()
        {
            m_ContinueButton.interactable = FileHandler.FileExists(MapCompletion.Filename);
        }
        public void StartGame()
        {
            FileHandler.DeleteFile(MapCompletion.Filename);
            SceneManager.LoadScene(1);
        }
        public void ContinueGame()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

    }
}
