using System;
using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu(menuName = "Abilities/FireAbility")]
    public class FireAbility : Ability
    {
        [SerializeField] private int cost = 10;
        [SerializeField] private int damage = 2;
        [SerializeField] private float radius = 2f;

        public FireAbility() { Cooldown = 5f; } 

        protected override void Use()
        {
            var player = TDPlayer.Instance;
            if (player == null || player.Gold < cost)
            {
                Debug.Log("Not enough gold for Fire Ability");
                return;
            }
            player.ChangeGold(-cost);
            
            Abilities.Instance.StartCoroutine(Abilities.Instance.FireTargetingRoutine(this));
        }

        public int Damage => damage;
        public float Radius => radius;
    }

}
