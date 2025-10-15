using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace TowerDefence
{
    public class Player : SingletonBase<Player>
    {
        // public static SpaceShip SelectedSpaceShip;

        [SerializeField] private int m_NumLives;
        public int NumLives { get { return m_NumLives; } }

        public event Action PlayerDied;

        // [SerializeField] private SpaceShip m_PlayerShipPrefab;

        private FollowCamera m_FollowCamera;
        // private ShipInputController m_ShipInputController;
        private Transform m_SpawnPoint;

        public FollowCamera FollowCamera => m_FollowCamera;

        // public void Construct(FollowCamera followCamera, ShipInputController shipInputController, Transform spawnPoint)
        // {
        //     m_FollowCamera = followCamera;
        //     m_ShipInputController = shipInputController;
        //     m_SpawnPoint = spawnPoint;
        // }

        // private SpaceShip m_Ship;
        // public SpaceShip ActiveShip => m_Ship;

        private int m_Score;
        private int m_NumKills;

        public int Score => m_Score;
        public int NumKills => m_NumKills;

        private void OnShipDeath()
        {
            m_NumLives--;
        }

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