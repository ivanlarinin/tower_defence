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

            // сбрасываем статы перед началом эпизода.
            // LevelResultController.ResetPlayerStats();

            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        /// <summary>
        /// Принудительный рестарт уровня.
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        public void FinishCurrentLevel(bool success)
        {
            // после организации переходов
            LevelController.Instance.Show(success);
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