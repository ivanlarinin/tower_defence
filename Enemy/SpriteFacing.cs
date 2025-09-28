using UnityEngine;

namespace TowerDefence
{
    public class SpriteFacing : MonoBehaviour
    {
        private Rigidbody2D rig;
        private SpriteRenderer sr;

        private void Start()
        {
            rig = transform.root.GetComponent<Rigidbody2D>();
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            transform.up = Vector2.up;
            var xMotion = rig.linearVelocity.x;
            if (xMotion > 0.01f) sr.flipX = false;
            else if (xMotion < 0.01f) sr.flipX = true;
        }
    }
}
