using System;
using System.Collections;
using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu(menuName = "Abilities/TimeAbility")]
    public class TimeAbility : Ability
    {
        [SerializeField] private float duration = 5f;

        public TimeAbility() { Cooldown = 15f; }

        private Enemy[] enemies;

        protected override void Use()
        {
            Debug.Log("Using Time Ability");

            enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            void Slow(Enemy e) => e.GetComponent<CharacterMotor>().HalveSpeed();

            foreach (var e in enemies)
                e.GetComponent<CharacterMotor>().HalveSpeed();

            EnemyWavesManager.OnEnemySpawn += Slow;
            Abilities.Instance.StartCoroutine(RestoreRoutine(Slow));
        }

        private IEnumerator RestoreRoutine(Action<Enemy> slowCallback)
        {
            yield return new WaitForSeconds(duration);

            foreach (var e in enemies)
            {
                if (e == null) continue;
                e.GetComponent<CharacterMotor>().SetSpeedMultiplier(2);
            }

            Debug.Log("Time Ability ended");
            EnemyWavesManager.OnEnemySpawn -= slowCallback;
        }
    }

}
