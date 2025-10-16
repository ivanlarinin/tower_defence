using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class Abilities : SingletonBase<Abilities>
    {
        [SerializeField] private FireAbility fireAbility;
        [SerializeField] private TimeAbility timeAbility;
        [SerializeField] private Button fireButton;
        [SerializeField] private Button timeButton;
        [SerializeField] private GameObject targetingPrefab;

        private GameObject targetingInstance;

        private void Start()
        {

            targetingInstance = Instantiate(targetingPrefab);
            targetingInstance.SetActive(false);

            int fireLevel = Upgrades.GetUpgradeLevel(UpgradeAsset.UpgradeType.FireAbility);
            bool hasFire = fireLevel > 0;
            fireButton.interactable = hasFire;

            int timeLevel = Upgrades.GetUpgradeLevel(UpgradeAsset.UpgradeType.TimeAbility);
            bool hasTime = timeLevel > 0;
            timeButton.interactable = hasTime;

            fireButton.onClick.AddListener(() => fireAbility.TryUse());
            timeButton.onClick.AddListener(() => timeAbility.TryUse());
        }

        public IEnumerator FireTargetingRoutine(FireAbility ability)
        {
            targetingInstance.SetActive(true);
            const float scaleFactor = 100f;
            RectTransform imageRect = targetingInstance.GetComponentsInChildren<RectTransform>()[1];
            imageRect.sizeDelta = Vector2.one * ability.Radius * 2f * scaleFactor;

            while (true)
            {
                Vector2 screenPos = Input.mousePosition;
                imageRect.position = screenPos;

                if (Input.GetMouseButtonDown(0))
                {
                    ApplyFireDamage(screenPos, ability);
                    break;
                }
                else if (Input.GetMouseButtonDown(1))
                    break;

                yield return null;
            }

            targetingInstance.SetActive(false);
        }

        private void ApplyFireDamage(Vector2 screenPos, FireAbility ability)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Vector2 worldPoint2D = new(worldPos.x, worldPos.y);

            foreach (var hit in Physics2D.OverlapCircleAll(worldPoint2D, ability.Radius))
            {
                hit.GetComponentInParent<Enemy>()?.TakeDamage(ability.Damage, DamageType.Physical);
            }
        }
    }
}