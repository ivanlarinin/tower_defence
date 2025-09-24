using System;
using UnityEngine;

namespace TowerDefence
{
    [DisallowMultipleComponent]
    [SelectionBase]
    [RequireComponent(typeof(Player))]
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance
        {
            get
            {
                return Player.Instance as TDPlayer;
            }
        }

        private static event System.Action<int> OnGoldChanged;
        public static void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldChanged += act;
        }
        private static event System.Action<int> OnLivesChanged;
        public static void LivesUpdateSubscribe(Action<int> act)
        {
            OnLivesChanged += act;
            act(Instance.NumLives);
        }

        [SerializeField] private int m_gold = 0;

        public void ChangeGold(int change)
        {
            m_gold += change;
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
            ChangeGold(-towerAsset.goldCost);
            var tower = Instantiate(m_towerPrefab, buildSite.position, Quaternion.identity);
            tower.GetComponentInChildren <SpriteRenderer>().sprite = towerAsset.sprite;
            Destroy(buildSite.gameObject);
        }
    }
}
