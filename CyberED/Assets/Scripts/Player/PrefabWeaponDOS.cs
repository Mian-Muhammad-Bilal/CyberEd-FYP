using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DOS;

namespace DOS
{
	public class PrefabWeaponDOS : MonoBehaviour {

		public Transform firePoint;
		public GameObject bulletPrefab;

		void Update () {
			if (Input.GetButtonDown("Fire1"))  // Still works for PC input
			{
				Shoot();
			}
		}

		void Shoot ()
		{
			Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		}

		// 📱 For Android UI Button
		public void Fire()
		{
			Shoot();
		}
	}
}