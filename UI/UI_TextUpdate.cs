using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UI_TextUpdate : MonoBehaviour
    {
        public enum UpdateSource
        {
            PlayerLifes,
            PlayerMoney,
            WaveNumber,
            BonusAmount
        }
        public UpdateSource source = UpdateSource.PlayerMoney;
        private Text m_text;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_text = GetComponent<Text>();
            Debug.Log("UI_TextUpdate: Subscribing to " + source.ToString());
            switch (source)
            {
                case UpdateSource.PlayerLifes:
                    TDPlayer.LivesUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.PlayerMoney:
                    TDPlayer.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.WaveNumber:
                    EnemyWavesManager.WaveNumberUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.BonusAmount:
                    EnemyWavesManager.WaveNumberUpdateSubscribe(UpdateBonus);
                    break;
                default:
                    break;
            }
        }

        private void UpdateText(int value)
        {
            if (m_text != null)
            {
                m_text.text = value.ToString();
            }
            else
            {
                Debug.LogError("UI_TextUpdate: No Text component found!");
            }
        }

        private void UpdateBonus(int waveNumber)
        {
            int bonus = waveNumber * 3;
            if (m_text != null)
            {
                m_text.text = bonus.ToString();
            }
            else
            {
                Debug.LogError("UI_TextUpdate: No Text component found!");
            }
        }

        private void OnDestroy()
        {
            switch (source)
            {
                case UpdateSource.PlayerLifes:
                    TDPlayer.LivesUpdateUnsubscribe(UpdateText);
                    break;
                case UpdateSource.PlayerMoney:
                    TDPlayer.GoldUpdateUnsubscribe(UpdateText);
                    break;
                case UpdateSource.WaveNumber:
                    EnemyWavesManager.WaveNumberUpdateUnsubscribe(UpdateText);
                    break;
                case UpdateSource.BonusAmount:
                    // Unsubscribe the bonus updater
                    EnemyWavesManager.WaveNumberUpdateUnsubscribe(UpdateBonus);
                    break;
                default:
                    break;
            }
        }
    }
}

