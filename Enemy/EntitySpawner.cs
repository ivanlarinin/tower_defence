using UnityEngine;

namespace TowerDefence
{
    public class EntitySpawner : Spawner
    {
        [SerializeField] private GameObject[] m_EntityPrefabs;
        protected override GameObject GenerateSpawnedEntity()
        {
            return Instantiate(m_EntityPrefabs[Random.Range(0, m_EntityPrefabs.Length)]);
        }
    }
}