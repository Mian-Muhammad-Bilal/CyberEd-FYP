using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossHealth1 : MonoBehaviour
{

	public int health = 500;

	public GameObject deathEffect;

	public bool isInvulnerable = false;

	public void TakeDamage(int damage)
	{
		if (isInvulnerable)
			return;

		health -= damage;

		if (health <= 200)
		{
			GetComponent<Animator>().SetBool("IsEnraged", true);
		}

		if (health <= 0)
		{
			Die();
			// StartCoroutine(LoadNextSceneAfterDelay(3f)); // Wait 3 seconds and load the next scene
		}
	}

	void Die()
		{
			Instantiate(deathEffect, transform.position, Quaternion.identity);
			gameObject.SetActive(false);
			// Destroy(gameObject); // Destroy boss immediately

			Invoke("LoadNextScene", 3f); // Call after 3 seconds
		}

		void LoadNextScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}


 	// private IEnumerator LoadNextSceneAfterDelay(float delay)
    // {
    // }
}
