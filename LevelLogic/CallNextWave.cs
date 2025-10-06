using UnityEngine;

namespace TowerDefence
{
    public class CallNextWave : MonoBehaviour
    {
        private EnemyWavesManager manager;
        void Start()
        {
            manager = FindObjectsByType<EnemyWavesManager>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0];
            if (manager == null)
            {
                Debug.LogError("EnemyWavesManager not found in the scene.");
            }
        }

        public void CallWave()
        {
            if (manager != null)
            {
                manager.CallNextWave();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}