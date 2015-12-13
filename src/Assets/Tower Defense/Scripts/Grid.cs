using UnityEngine;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Grid")]
	public class Grid : MonoBehaviour
	{
		private int m_gridSizeX, m_gridSizeY;
		private float m_nodeDiameter = 1;
		private Node[,] m_grid;

		[Header("Grid Settings")]
		[SerializeField] private Node m_nodePrefab;
		[SerializeField] private Vector2 m_gridWorldSize;

		void Start()
		{
			m_gridSizeX = Mathf.RoundToInt (m_gridWorldSize.x);
			m_gridSizeY = Mathf.RoundToInt (m_gridWorldSize.y);

			CreateGrid ();
		}

		void OnDrawGizmos()
		{
			Gizmos.DrawWireCube(transform.position, new Vector3(m_gridWorldSize.x, 1, m_gridWorldSize.y));
		}

		public bool CanPlace (Node node)
		{
			const int minSpot = 4, maxSpot = 7;

			int x = node.GridX;
			int y = node.GridY;

			if (x == 0 && y >= minSpot && y <= maxSpot)
			{
				int spotsFree = 0;
				for (int i = minSpot; i <= maxSpot; i++) 
				{
					if (m_grid[0, i].IsNotPlaced) spotsFree++;
				}
				if (spotsFree < 2) return false;
			}

			if (x == m_gridSizeX - 1 && y >= minSpot && y <= maxSpot)
			{
				int spotsFree = 0;
				for (int i = m_gridSizeX - 1; i <= maxSpot; i++) 
				{
					if (m_grid[m_gridSizeX - 1, i].IsNotPlaced) spotsFree++;
				}
				if (spotsFree < 2) return false;
			}

			return true;
		}

		private void CreateGrid ()
		{
			m_grid = new Node[m_gridSizeX, m_gridSizeY];
			Vector3 worldBottomLeft = transform.position - Vector3.right * m_gridWorldSize.x / 2 - Vector3.forward * m_gridWorldSize.y / 2;

			for (int x = 0; x < m_gridSizeX; x++) 
			{
				for (int y = 0; y < m_gridSizeY; y++) 
				{
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * m_nodeDiameter + m_nodeDiameter / 2) + Vector3.forward * (y * m_nodeDiameter + m_nodeDiameter / 2);

					Node node = Instantiate(m_nodePrefab) as Node;
					node.Setup(transform, worldPoint, m_nodeDiameter, this, x, y);

					m_grid[x, y] = node;
				}
			}
		}
	}
}