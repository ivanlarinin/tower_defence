using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// Singleton class responsible for playing sound effects and music tracks in the game.
    /// </summary>
    public class SoundPlayer : SingletonBase<SoundPlayer>
    {
        public AudioSource m_AudioSource;

        public AudioClip m_BGM;
        public AudioClip m_Arrow;
        public AudioClip m_ArrowHit;
        public AudioClip m_EnemyDeath;
        public AudioClip m_TowerBuilt;

        private void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.loop = true;
            m_AudioSource.PlayOneShot(m_BGM);
        }

        public void PlaySound(Sound sound)
        {
            switch (sound)
            {
                case Sound.BGM:
                    m_AudioSource.PlayOneShot(m_BGM);
                    break;
                case Sound.Arrow:
                    m_AudioSource.PlayOneShot(m_Arrow);
                    break;
                case Sound.ArrowHit:
                    m_AudioSource.PlayOneShot(m_ArrowHit);
                    break;
                case Sound.EnemyDeath:
                    m_AudioSource.PlayOneShot(m_EnemyDeath);
                    break;
                case Sound.TowerBuilt:
                    m_AudioSource.PlayOneShot(m_TowerBuilt);
                    break;
            }
        }
    }
}