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
            switch (source)
            {
                case UpdateSource.PlayerLifes:
                    TDPlayer.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.PlayerMoney:
                    TDPlayer.LivesUpdateSubscribe(UpdateText);
                    break;
                default:
                    break;
            }
        }

        private void UpdateText(int value)
        {
            m_text.text = value.ToString();
        }
    }
}

