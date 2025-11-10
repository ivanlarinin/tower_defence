using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace TowerDefence
{
    public class Player : SingletonBase<Player>
    {

        [SerializeField] private int m_NumLives;
        public int NumLives { get { return m_NumLives; } }

        public event Action PlayerDied;

        private int m_Score;
        private int m_NumKills;

        protected void TakeDamage(int m_damage)
        {
            m_NumLives -= m_damage;
            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                PlayerDied?.Invoke();
            }
        }

        protected void AddLives(int lives)
        {
            m_NumLives += lives;
        }
    }
}