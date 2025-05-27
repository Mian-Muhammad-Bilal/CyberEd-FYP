using UnityEngine;
using DOS;

namespace DOS
{
    public class BulletDOS : MonoBehaviour
    {
        public float speed = 20f;
        public int damage = 150;
        public Rigidbody2D rb;
        public GameObject impactEffect;

        void Start()
        {
            // Bullet will move in the direction the firePoint is facing
            rb.linearVelocity = transform.right * speed;
        }

        void OnTriggerEnter2D(Collider2D hitInfo)
        {
            BossHealthDOS enemy = hitInfo.GetComponent<BossHealthDOS>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
