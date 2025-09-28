using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class Player : SingletonBase<Player>
    {
        public static SpaceShip SelectedSpaceShip;

        [SerializeField] private int m_NumLives;
        public int NumLives { get { return m_NumLives; } }

        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        // [SerializeField] private StarfieldParallaxController m_ParallaxController;

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

        private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        private int m_Score;
        private int m_NumKills;

        public int Score => m_Score;
        public int NumKills => m_NumKills;

        public SpaceShip ShipPrefab
        {
            get
            {
                if (SelectedSpaceShip == null)
                {
                    return m_PlayerShipPrefab;
                }
                else
                {
                    return SelectedSpaceShip;
                }
            }
        }

        private void Start()
        {
            // Respawn();
        }

        private void Update()
        {
            CheckForRemainingEnemies();
        }

        private void CheckForRemainingEnemies()
        {
            // Find ALL active instances of the Enemy class in the scene.
            // Replace 'EnemyScript' with the actual class name on your enemies.
            // If the Enemy script is not in the TowerDefence namespace, you may need
            // to adjust access or its namespace.
            // Assuming the enemy script is called 'Enemy' for this example.

            // NOTE: If your enemy script is not in the same namespace,
            // you may need to specify its full path or add a 'using' statement.
            var remainingEnemies = FindObjectsByType<Enemy>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
                );

            if (remainingEnemies.Length == 0)
            {
                SceneManager.LoadScene(1);
            }
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(ShipPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            m_FollowCamera.SetTarget(m_Ship.transform);
            // m_ShipInputController.SetTargetShip(m_Ship);

            m_Ship.EventOnDeath.AddListener(OnShipDeath);

            // if (m_ParallaxController != null)
            // {
            //     m_ParallaxController.target = m_Ship.transform;
            // }
        }

        public void AddKill()
        {
            m_NumKills += 1;
        }

        public void AddScore(int num)
        {
            m_Score += num;
        }

        protected void TakeDamage(int m_damage)
        {
            m_NumLives -= m_damage;
            if (m_NumLives <= 0)
            {
                SceneManager.LoadScene(0);

                // if (LevelController.Instance != null)
                // {
                //     LevelController.Instance.RestartLevel();
                // }
                // else
                // {
                //     Debug.Log("No LevelController found");
                // }
            }
        }
    }
}