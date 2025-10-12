using System;
using UnityEngine;

namespace TowerDefence
{
    [DisallowMultipleComponent]
    [SelectionBase]
    [RequireComponent(typeof(Player))]
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance { get; private set; }
        private static event System.Action<int> OnGoldChanged;

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        public static void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldChanged += act;
            act(Instance.m_gold);
        }
        private static event System.Action<int> OnLivesChanged;
        public static void LivesUpdateSubscribe(Action<int> act)
        {
            OnLivesChanged += act;
            act(Instance.NumLives);
        }

        [SerializeField] private int m_gold = 0;
        [SerializeField] private int m_goldSpent = 0;
        public int GoldSpent => m_goldSpent;

        public void ChangeGold(int change)
        {
            m_gold += change;

            if (change < 0) m_goldSpent += Mathf.Abs(change);
            if (OnGoldChanged != null)
            {
                // Debug.Log("Invoking OnGoldChanged with " + m_gold);
                OnGoldChanged(m_gold);
            }
        }

        public void ReduceLife(int change)
        {
            TakeDamage(change);
            OnLivesChanged?.Invoke(NumLives);
        }

        [SerializeField] private Tower m_towerPrefab ;

        public void TryBuild(TowerAssets towerAsset, Transform buildSite)
        {
            if (m_gold < towerAsset.goldCost) return;
            ChangeGold(-towerAsset.goldCost);
            var tower = Instantiate(m_towerPrefab, buildSite.position, Quaternion.identity);
            tower.GetComponentInChildren <SpriteRenderer>().sprite = towerAsset.sprite;
            tower.GetComponentInChildren<Turret>().SetTurretProperties(towerAsset.turretProperties);
            Destroy(buildSite.gameObject);
        }
    }
}
