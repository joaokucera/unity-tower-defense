using System;
using UnityEngine;

namespace TowerDefense
{
	public enum TowerSpeed
	{
		Slow = 3,
		Average = 2,
		Fast = 1
	}

	[Serializable]
	public class TowerSettings
	{
		[HideInInspector] public int Level = 1;

		public string Name;
		public string Description;
		public int Cost;
		public int Damage;
		public float Range;
		public TowerSpeed Speed;
		public Material Race;

		public string LevelAfterUpgrade { get { return string.Format ("{0} {1}", Name, Level + 1); } }
		public int DamageAfterUpgrade { get { return Damage * 2; } }
		public int CostToUpgrade { get { return Cost / 3 * 2; } }
		public int GoldToSell { get { return Cost / 2; } }

		public void Upgrade()
		{
			Level++;
			Cost += Cost / 2;
			Damage = DamageAfterUpgrade;
		}
	}
}