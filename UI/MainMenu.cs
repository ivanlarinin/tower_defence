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

            // Log the full path to the save file for debugging purposes
            // var path = FileHandler.GetFullPath(MapCompletion.Filename);
            // Debug.Log($"[MainMenu] Save file path: {path}");
        }
        public void StartGame()
        {
            FileHandler.DeleteFile(MapCompletion.Filename);
            FileHandler.DeleteFile(Upgrades.filename);
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
