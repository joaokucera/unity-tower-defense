using System.Collections;
using UnityEngine;
using System.Linq;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Tower Agent")]
	[RequireComponent(typeof(NavMeshObstacle))]
	[RequireComponent(typeof(Renderer))]
	public class TowerAgent : MonoBehaviour, ITowerHandler
	{
		private TowerSettings m_settings;
		private Renderer m_renderer;
		private float m_cooldownTime;

		[SerializeField] private LayerMask m_creepMask;
		[SerializeField] private Transform m_weapon;

		void Update()
		{
			if (Time.time - m_cooldownTime > (int)m_settings.Speed)
			{
				var colliders = Physics.OverlapSphere(transform.position, m_settings.Range, m_creepMask.value);

				if (colliders.Length > 0)
				{
					var target = colliders.Aggregate((h1, h2) => 
					                                 Vector3.Distance(h1.transform.position, transform.position) < 
					                                 Vector3.Distance(h2.transform.position, transform.position) ? 
					                                 h1 : h2);

					if (target != null && target.IsCreep())
					{
						m_cooldownTime = Time.time;

						LevelManager.TowerShotPooling.SpawnShot (m_weapon.position, m_weapon.rotation, m_settings.Damage, target.transform);
					}
				}
			}
		}

		void OnMouseDown()
		{
			LevelManager.SetTower (this, true);
		}
		
		void OnDrawGizmos()
		{
			if (m_settings == null) return;

			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere (transform.position, m_settings.Range);
		}

		public void Setup(Vector3 position, Quaternion rotation, TowerSettings settings)
		{
			transform.localPosition = position;
			transform.localRotation = rotation;

			m_settings = settings;

			if (m_renderer == null) m_renderer = GetComponent<Renderer> ();
			m_renderer.sharedMaterial = m_settings.Race;

			m_cooldownTime = Time.time;
		}

		#region ITowerHandler implementation
		
		public TowerSettings Settings { get { return m_settings; } }
		
		#endregion
	}
}