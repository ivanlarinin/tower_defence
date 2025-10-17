using UnityEngine;
using System.Collections;

namespace TowerDefence
{
    public abstract class Ability : ScriptableObject
    {
        public float Cooldown { get; protected set; }
        public bool IsOnCooldown { get; private set; }

        public void TryUse()
        {
            if (IsOnCooldown) return;
            Use();
            Abilities.Instance.StartCoroutine(CooldownRoutine());
        }

        protected abstract void Use();

        private IEnumerator CooldownRoutine()
        {
            IsOnCooldown = true;
            yield return new WaitForSeconds(Cooldown);
            IsOnCooldown = false;
        }
    }
}
