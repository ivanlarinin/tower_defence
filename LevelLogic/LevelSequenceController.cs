using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickname = "LevelMap";

        /// <summary>
        /// Текущий эпизод. Выставляется контроллером выбора эпизода перед началом игры.
        /// </summary>
        public Episode CurrentEpisode { get; private set; }

        /// <summary>
        /// Текущий уровень эпизода. Идшник относительно текущего выставленного эпизода.
        /// </summary>
        public int CurrentLevel { get; private set; }

        public void StartEpisode(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }
        public void ExitToMenu()
        {
            SceneManager.LoadScene(MainMenuSceneNickname);
        }
        public void RestartLevel()
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        public void AdvanceLevel()
        {
            CurrentLevel++;

            if (CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
            }
            else
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            }
        }
    }
}