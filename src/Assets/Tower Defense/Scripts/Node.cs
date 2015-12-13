using UnityEngine;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Node")]
	[RequireComponent(typeof(Renderer))]
	public class Node : MonoBehaviour
	{
		private Grid m_grid;
		private TowerAgent m_agentPlaced;
		private Renderer m_renderer;

		public int GridX { get; private set; }
		public int GridY { get; private set; }

		private bool IsValid { get { return IsNotPlaced && LevelManager.CurrentTowerHandler != null; } }

		public bool IsNotPlaced { get { return m_agentPlaced == null || !m_agentPlaced.gameObject.activeInHierarchy; } }

		void Start()
		{
			m_renderer = GetComponent<Renderer> ();
		}

		public void Setup(Transform parent, Vector3 position, float scale, Grid grid, int x, int y)
		{
			transform.SetParent (parent);

			transform.localPosition = position;
			transform.localScale = new Vector3 (scale, 1, scale);

			m_grid = grid;

			GridX = x;
			GridY = y;
		}

		void OnMouseDown()
		{
			if (!IsValid) return;

			if (LevelManager.CanBuyOrUpgrade (LevelManager.CurrentTowerHandler.Settings.Cost) && m_grid.CanPlace(this))
			{
				m_agentPlaced = LevelManager.BuyTower(transform.position, Quaternion.identity);

				m_renderer.enabled = false;
			}
		}

		void OnMouseEnter()
		{
			if (!IsValid) return;

			m_renderer.enabled = true;
		}

		void OnMouseExit()
		{
			if (!IsValid) return;

			m_renderer.enabled = false;
		}
	}
}