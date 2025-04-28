using System.Collections;
using System.Collections.Generic;
using DOS;
using UnityEngine;
using UnityEngine.UI;

namespace DOS
{
	public class HealthBarDOS : MonoBehaviour
	{
		public BossHealthDOS bossHealth;
		public Slider slider;

		void Start()
		{
			slider.maxValue = bossHealth.health;
		}

		// Update is called once per frame
		void Update()
		{
			slider.value = bossHealth.health;
		}
	}
}