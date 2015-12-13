using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Tower Agent UI")]
	[RequireComponent(typeof(Outline))]
	public class TowerAgentUI : MonoBehaviour, ITowerHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
	{
		private Outline m_outine;

		[Header("Tower Settings")]
		[SerializeField] private TowerSettings m_settings;

		void Start()
		{
			m_outine = GetComponent<Outline>();
		}

		#region ITowerHandler implementation

		public TowerSettings Settings { get { return m_settings; } }

		#endregion

		#region IPointerDownHandler implementation

		public void OnPointerDown (PointerEventData eventData)
		{
			LevelManager.SetTower (this);
		}

		#endregion

		#region IPointerEnterHandler implementation

		public void OnPointerEnter (PointerEventData eventData)
		{
			m_outine.enabled = true;

			LevelManager.TryShowPanel (m_settings);
		}

		#endregion

		#region IPointerExitHandler implementation
		
		public void OnPointerExit (PointerEventData eventData)
		{
			m_outine.enabled = false;

			LevelManager.TryHidePanels ();
		}
		
		#endregion
	}
}