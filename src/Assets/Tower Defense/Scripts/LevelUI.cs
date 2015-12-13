using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Level UI")]
	public class LevelUI : MonoBehaviour
	{
		private delegate void PlayPauseBehaviour();
		private PlayPauseBehaviour onPlayPauseBehaviour;

		[Header("Level Status UI Components")]
		[SerializeField] private Text m_textTime;
		[SerializeField] private Text m_textLevel;
		[SerializeField] private Text m_textLives;
		[SerializeField] private Text m_textGold;
		[SerializeField] private Text m_textScore;
		[SerializeField] private Text m_textButtonStart;
		[SerializeField] private GameObject m_panelPause;
		[SerializeField] private GameObject m_panelReset;

		[Header("Selected Tower UI Components")]
		[SerializeField] private TowerSettingsUI m_panelSettings;
		[SerializeField] private Text m_textSellButton;
		[SerializeField] private TowerSettingsUI m_panelUpgrade;
		[SerializeField] private GameObject m_upgradeButton;

		[Header("Game Over UI Components")]
		[SerializeField] private GameObject m_panelGameOver;
		[SerializeField] private Text m_textBestScore;
		[SerializeField] private Text m_textFinalScore;

		#region Public Methods

		void Start()
		{
			onPlayPauseBehaviour = PlayBehaviour;
		}
		
		public void PlayPauseLevel()
		{
			SoundManager.PlaySoundEffect ("ButtonClick");

			onPlayPauseBehaviour ();
		}

		public void ResetLevel()
		{
			SoundManager.PlaySoundEffect ("ButtonClick");

			m_panelReset.SetActive (true);

			Time.timeScale = 0;
		}

		public void ResetLevelConfirmation(bool confirm)
		{
			Time.timeScale = 1;

			SoundManager.PlaySoundEffect ("ButtonClick");

			if (confirm)
			{
				Application.LoadLevel ("Level");
			}
			else
			{
				m_panelReset.SetActive (false);
			}
		}

		public void NextWave()
		{
			SoundManager.PlaySoundEffect ("ButtonClick");

			LevelManager.NextLevel (true);
		}

		public void UpgradeTower()
		{
			SoundManager.PlaySoundEffect ("ButtonClick");

			LevelManager.UpgradeTower ();
		}

		public void SellTower()
		{
			SoundManager.PlaySoundEffect ("ButtonClick");

			LevelManager.SellTower ();
		}

		public void UpdateTextTime(float time)
		{
			m_textTime.text = string.Format ("Time: {0:0}", time);
		}
		
		public void UpdateTextLevel(int level)
		{
			m_textLevel.text = string.Format ("Level: {0}", level);
		}
		
		public void UpdateTextLives(int lives)
		{
			m_textLives.text = string.Format ("Lives: {0}", lives);
		}
		
		public void UpdateTextGold(int gold)
		{
			m_textGold.text = string.Format ("Gold: {0}", gold);
		}
		
		public void UpdateTextScore(int score)
		{
			m_textScore.text = string.Format ("Score: {0}", score);
		}

		public void GameOver (int bestScore, int finalScore)
		{
			SoundManager.PlaySoundEffect ("GameOver");

			m_textBestScore.text = string.Format ("Best Score: <color=#FFC000FF>{0}</color>", bestScore);
			m_textFinalScore.text = string.Format ("Final Score: <color=#FFC000FF>{0}</color>", finalScore);

			m_panelGameOver.SetActive (true);
		}
		
		public void ShowPanels(TowerSettings settings, bool showPanelToUpgrade = false)
		{
			ShowPanelSettings (settings, showPanelToUpgrade);
			
			if (showPanelToUpgrade)
			{
				ShowPanelUpgrade(settings);
			}
		}

		public void HidePanels()
		{
			LevelManager.CurrentTowerHandler = null;
			
			m_panelSettings.Hide();
			
			m_panelUpgrade.Hide ();
			
			m_textSellButton.transform.parent.gameObject.SetActive(false);
		}

		#endregion

		#region Private Methods

		private void ShowPanelSettings(TowerSettings settings, bool showSellButton = false)
		{
			m_panelSettings.Show (settings);
			
			if (showSellButton)
			{
				m_textSellButton.text = string.Format ("Sell for {0}", settings.GoldToSell);
			}
			
			m_textSellButton.transform.parent.gameObject.SetActive(showSellButton);
		}
		
		private void ShowPanelUpgrade(TowerSettings settings)
		{
			var newCost = settings.CostToUpgrade;

			var toUpgrade = new TowerSettings
			{
				Name = settings.LevelAfterUpgrade,
				Description = "Increase damage",
				Cost = newCost,
				Damage = settings.DamageAfterUpgrade,
				Range = settings.Range,
				Speed = settings.Speed
			};
			
			m_panelUpgrade.Show (toUpgrade);
			
			m_upgradeButton.SetActive (LevelManager.CanBuyOrUpgrade (newCost));
		}

		private void PlayBehaviour()
		{
			LevelManager.IsPlaying = true;
			
			m_textButtonStart.text = "PAUSE";
			
			onPlayPauseBehaviour = PauseBehaviour;
		}
		
		private void PauseBehaviour()
		{
			LevelManager.IsPlaying = !LevelManager.IsPlaying;
			
			if (LevelManager.IsPlaying)
			{
				m_textButtonStart.text = "PAUSE";
				m_panelPause.SetActive(false);
				
				Time.timeScale = 1;
			}
			else
			{
				m_textButtonStart.text = "RESUME";
				m_panelPause.SetActive(true);
				
				Time.timeScale = 0;
			}
		}

		#endregion
	}
}