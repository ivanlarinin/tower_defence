using System;
using UnityEngine;

namespace TowerDefence
{

    /// <summary>
    /// Represents the player in the Tower Defence game, managing gold, lives, and tower building.
    /// </summary>
    [DisallowMultipleComponent]
    [SelectionBase]
    [RequireComponent(typeof(Player))]
    public class TDPlayer : Player
    {
        #region Events

        public static new TDPlayer Instance { get; private set; }
        private static event Action<int> OnGoldChanged;
        private static event Action<int> OnLivesChanged;

        #endregion

        #region Fields / Properties

        [SerializeField] private int m_gold = 0;
        public int Gold => m_gold;
        [SerializeField] private int m_goldSpent = 0;
        public int GoldSpent => m_goldSpent;

        [SerializeField] private Tower m_towerPrefab;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            Instance = this;

            Upgrades.Instance.ApplyUpgradesToPlayer(this);

            // Abilities.Instance.InitializeUI();
        }

        #endregion

        #region Public Methods

        public static void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldChanged += act;
            act(Instance.m_gold);
        }

        public static void LivesUpdateSubscribe(Action<int> act)
        {
            OnLivesChanged += act;
            act(Instance.NumLives);
        }

        public void ChangeGold(int change)
        {
            m_gold += change;

            if (change < 0) m_goldSpent += Mathf.Abs(change);
            if (OnGoldChanged != null)
            {
                OnGoldChanged(m_gold);
            }
        }

        public void ReduceLife(int change)
        {
            TakeDamage(change);
            OnLivesChanged?.Invoke(NumLives);
        }

        public void AddLife(int amount)
        {
            AddLives(amount);
            OnLivesChanged?.Invoke(NumLives);
        }

        public void TryBuild(TowerAssets towerAsset, Transform buildSite)
        {
            if (m_gold < towerAsset.goldCost) return;
            ChangeGold(-towerAsset.goldCost);
            var tower = Instantiate(m_towerPrefab, buildSite.position, Quaternion.identity);
            tower.GetComponentInChildren<SpriteRenderer>().sprite = towerAsset.sprite;
            tower.GetComponentInChildren<Turret>().SetTurretProperties(towerAsset.turretProperties);
            Destroy(buildSite.gameObject);
        }

        internal static void LivesUpdateUnsubscribe(Action<int> act)
        {
            OnLivesChanged -= act;
        }

        internal static void GoldUpdateUnsubscribe(Action<int> act)
        {
            OnGoldChanged -= act;
        }

        #endregion
    }
}
