using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UI_TextUpdate : MonoBehaviour
    {
        public enum UpdateSource
        {
            PlayerLifes,
            PlayerMoney
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
            }
        }
    }
}

