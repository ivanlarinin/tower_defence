using UnityEngine;
using UnityEngine.SceneManagement;

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
        public AudioClip m_EnemyBreakthrough;
        public AudioClip m_Win;
        public AudioClip m_Lose;

        private void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.clip = m_BGM;
            m_AudioSource.Play();
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        public void PlaySound(Sound sound)
        {
            switch (sound)
            {
                case Sound.BGM:
                    if (!m_AudioSource.isPlaying)
                    {
                        m_AudioSource.Play();
                    }
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
                case Sound.EnemyBreakthrough:
                    m_AudioSource.PlayOneShot(m_EnemyBreakthrough);
                    break;
                case Sound.Win:
                    m_AudioSource.PlayOneShot(m_Win);
                    break;
                case Sound.Lose:        
                    m_AudioSource.PlayOneShot(m_Lose);
                    break;
            }
        }

        void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            string[] allowedScenes = { "MainMenu", "LevelMap" };

            if (System.Array.Exists(allowedScenes, s => s == newScene.name))
            {
                if (m_AudioSource.clip != m_BGM)
                {
                    m_AudioSource.clip = m_BGM;
                    m_AudioSource.loop = true;
                }

                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.Play();
                }
            }
            else
            {
                m_AudioSource.Stop();
            }
        }

        void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }
}