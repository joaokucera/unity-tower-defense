using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Menu UI")]
	public class MenuUI : MonoBehaviour 
	{
		[SerializeField] private Text m_textBestScore;

		void Start()
		{
			var bestScore = PlayerPrefs.GetInt ("bestscore");

			m_textBestScore.text = string.Format ("Best Score: <color=#FFC000FF>{0}</color>", bestScore);
		}

		public void Play()
		{
			SoundManager.PlaySoundEffect ("ButtonClick");

			Application.LoadLevel ("Level");
		}
	}
}