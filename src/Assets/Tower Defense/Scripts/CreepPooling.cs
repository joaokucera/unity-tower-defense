using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
	[Serializable]
	public class CreepRace
	{
		public CreepSettings Settings;
		public int FirstLevelToAppear;
		[Range(25, 100)] public int PercentagePerLevel;
	}

	[AddComponentMenu("TOWER DEFENSE/Creep Pooling")]
	public class CreepPooling : Pooling<CreepAgent>
	{
		private List<CreepAgent> m_currentWave = new List<CreepAgent>();

		[Header("Creep Wave Settings")]
		[SerializeField] private Transform entry;
		[SerializeField] private Transform exit;
		[SerializeField] private CreepRace[] m_spawnSettings;

		public void SpawnWave(int currentLevel)
		{
			var dic = new Dictionary<int, int> ();

			for (int i = 0; i < m_spawnSettings.Length; i++)
			{
				if (currentLevel >= m_spawnSettings[i].FirstLevelToAppear)
				{
					int amount = currentLevel * m_spawnSettings[i].PercentagePerLevel / 100;

					dic.Add(i, amount);
				}
			}

			foreach (var item in dic)
			{
				for (int i = 0; i < item.Value; i++)
				{
					var creep = GetObjectFromPool ();
					
					creep.Setup (entry.position, entry.rotation, m_spawnSettings[item.Key].Settings, exit);
					
					m_currentWave.Add(creep);
				}
			}
		}

		public void HideCreep(CreepAgent creep)
		{
			SetObjectToPool (creep);

			m_currentWave.Remove (creep);

			if (m_currentWave.Count == 0)
			{
				LevelManager.NextLevel();
			}
		}
	}
}