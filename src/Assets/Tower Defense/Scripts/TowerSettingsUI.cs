using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Tower Settings UI")]
	public class TowerSettingsUI : MonoBehaviour 
	{
		[Header("Tower Settings UI Components")]
		[SerializeField] private Text m_textTowerName;
		[SerializeField] private Text m_textTowerDescription;
		[SerializeField] private Text m_textTowerCost;
		[SerializeField] private Text m_textTowerDamage;
		[SerializeField] private Text m_textTowerRange;
		[SerializeField] private Text m_textTowerSpeed;

		public void Show(TowerSettings settings)
		{
			m_textTowerName.text = settings.Name;
			m_textTowerDescription.text = settings.Description;
			m_textTowerCost.text = string.Format("COST: <color=#FFC000FF><size=16>{0}</size></color>", settings.Cost);
			m_textTowerDamage.text = string.Format("DAMAGE: <color=#C00000FF><size=16>{0}</size></color>", settings.Damage);
			m_textTowerRange.text = string.Format("RANGE: <color=#0060FFFF><size=16>{0}</size></color>", settings.Range);
			m_textTowerSpeed.text = string.Format("SPEED: <color=#808080FF><size=16>{0}</size></color>", settings.Speed);

			gameObject.SetActive (true);
		}

		public void Hide()
		{
			gameObject.SetActive (false);
		}
	}
}