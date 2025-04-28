using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DOS;

namespace DOS
{
    public class BossHealthDOS : MonoBehaviour
    {
        public int health = 150;
        public GameObject deathEffect;
        public bool isInvulnerable = false;

        public void TakeDamage(int damage)
        {
            if (isInvulnerable)
                return;

            health -= damage;

            if (health <= 50)
            {
                GetComponent<Animator>().SetBool("IsEnraged", true);
            }

            if (health <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
