using UnityEngine;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Creep Agent")]
	[RequireComponent(typeof(NavMeshAgent))]
	public class CreepAgent : MonoBehaviour 
	{
		private CreepSettings m_settings;
		private NavMeshAgent m_agent;
		private Renderer m_renderer;

		public CreepSettings Settings { get { return m_settings; } }

		void OnTriggerEnter(Collider collider)
		{
			if (collider.IsTowerShot())
			{
				var shot = collider.GetComponent<TowerShot>();
				var damage = shot.Damage;
				shot.Hide();

				m_settings.Energy -= damage;

				if (m_settings.Energy <= 0)
				{
					SoundManager.PlaySoundEffect ("CreepDeath");

					LevelManager.AddScore(this);
				}
			}
			else if (collider.IsCreepExit())
			{
				LevelManager.LoseLife(this);
			}
		}

		public void Setup (Vector3 position, Quaternion rotation, CreepSettings settings, Transform destination)
		{
			transform.localPosition = position;
			transform.localRotation = rotation;

			m_settings = settings;

			if (m_renderer == null) m_renderer = GetComponent<Renderer> ();
			m_renderer.sharedMaterial = m_settings.Race;

			if (m_agent == null) m_agent = GetComponent<NavMeshAgent> ();
			m_agent.speed = m_settings.Speed;
			m_agent.SetDestination (destination.position);
		}
	}
}