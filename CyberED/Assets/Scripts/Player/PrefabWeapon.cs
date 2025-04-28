using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    // This method will be called by your button
    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
