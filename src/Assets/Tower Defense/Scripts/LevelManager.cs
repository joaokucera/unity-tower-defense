using UnityEngine;
using System.Collections;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Level Manager")]
	[RequireComponent(typeof(LevelUI))]
	public class LevelManager : Singleton<LevelManager>
	{
		private const float TimeLimit = 35f;

		private float m_time;
		private int m_level;
		private int m_lives = 20;
		private int m_gold = 80;
		private int m_score;
		private int m_bestScore;

		private LevelUI m_levelUI;
		private TowerPooling m_towerPooling;
		private CreepPooling m_creepPooling;

		[Range(1, 10)] [SerializeField] private float m_waitTimeToNextWave;

		public static bool IsPlaying;
		public static ITowerHandler CurrentTowerHandler;
		public static TowerShotPooling TowerShotPooling;

		void Start()
		{
			m_levelUI = GetComponent<LevelUI> ();

			m_towerPooling = GameObject.FindObjectOfType<TowerPooling>();
			m_creepPooling = GameObject.FindObjectOfType<CreepPooling>();

			TowerShotPooling = GameObject.FindObjectOfType<TowerShotPooling>();
		}

		void Update()
		{
			if (!IsPlaying) return;

			m_time -= Time.deltaTime;
			m_levelUI.UpdateTextTime (m_time);

			if (m_time <= 0) 
			{
				NextLevel();
			}
		}

		private IEnumerator NextLevel (float time)
		{
			m_time = TimeLimit;
			m_levelUI.UpdateTextTime (Instance.m_time);

			m_level++;
			m_levelUI.UpdateTextLevel (Instance.m_level);

			yield return new WaitForSeconds (time);

			m_creepPooling.SpawnWave (Instance.m_level);
		}

		private void CheckBestScore ()
		{
			const string key = "bestscore";

			m_bestScore = PlayerPrefs.GetInt (key);

			if (m_score > m_bestScore)
			{
				m_bestScore = m_score;

				PlayerPrefs.SetInt(key, m_bestScore);
			}
		}

		#region Static Methods

		public static void TryShowPanel (TowerSettings settings)
		{
			if (CurrentTowerHandler == null)
			{
				Instance.m_levelUI.ShowPanels (settings);
			}
		}

		public static void TryHidePanels ()
		{
			if (CurrentTowerHandler == null)
			{
				Instance.m_levelUI.HidePanels ();
			}
		}

		public static void SetTower(ITowerHandler tower, bool showPanelToUpgrade = false)
		{
			CurrentTowerHandler = tower;

			Instance.m_levelUI.ShowPanels (CurrentTowerHandler.Settings, showPanelToUpgrade);
		}

		public static TowerAgent BuyTower(Vector3 position, Quaternion rotation)
		{
			Instance.m_gold -= CurrentTowerHandler.Settings.Cost;
			Instance.m_levelUI.UpdateTextGold (Instance.m_gold);

			var tower = Instance.m_towerPooling.SpawnTower(position, rotation, CurrentTowerHandler.Settings);

			Instance.m_levelUI.HidePanels ();

			return tower;
		}

		public static void UpgradeTower ()
		{
			Instance.m_gold -= CurrentTowerHandler.Settings.CostToUpgrade;
			Instance.m_levelUI.UpdateTextGold (Instance.m_gold);

			CurrentTowerHandler.Settings.Upgrade ();

			Instance.m_levelUI.HidePanels ();
		}

		public static void SellTower ()
		{
			var gold = CurrentTowerHandler.Settings.GoldToSell;
			Instance.m_towerPooling.HideTower (CurrentTowerHandler as TowerAgent);

			Instance.m_gold += gold;
			Instance.m_levelUI.UpdateTextGold (Instance.m_gold);

			Instance.m_levelUI.HidePanels ();
		}

		public static bool CanBuyOrUpgrade(int cost)
		{
			return Instance.m_gold >= cost;
		}

		public static void NextLevel (bool immediately = false)
		{
			float time = immediately ? 0 : Instance.m_waitTimeToNextWave;

			Instance.StartCoroutine (Instance.NextLevel (time));
		}

		public static void LoseLife(CreepAgent creep)
		{
			Instance.m_creepPooling.HideCreep (creep);

			Instance.m_lives--;

			if (Instance.m_lives > 0)
			{
				SoundManager.PlaySoundEffect ("LoseLife");

				Instance.m_levelUI.UpdateTextLives(Instance.m_lives);
			}
			else
			{
				IsPlaying = false;

				Instance.CheckBestScore();

				Instance.m_levelUI.GameOver(Instance.m_bestScore, Instance.m_score);
			}
		}

		public static void AddScore(CreepAgent creep)
		{
			var points = creep.Settings.Points;

			Instance.m_creepPooling.HideCreep (creep);

			Instance.m_score += points;
			Instance.m_levelUI.UpdateTextScore (Instance.m_score);

			Instance.m_gold += points;
			Instance.m_levelUI.UpdateTextGold (Instance.m_gold);
		}
		
		#endregion
	}
}