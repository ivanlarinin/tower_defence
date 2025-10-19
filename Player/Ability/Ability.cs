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
            Abilities.Instance?.RefreshButtons();

            yield return new WaitForSeconds(Cooldown);

            IsOnCooldown = false;
            Abilities.Instance?.RefreshButtons();
        }

        // Reset runtime cooldown state (ScriptableObjects persist between scene loads)
        public void ResetCooldownState()
        {
            IsOnCooldown = false;
        }
    }
}
