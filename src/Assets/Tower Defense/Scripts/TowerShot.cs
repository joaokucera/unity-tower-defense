using UnityEngine;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Tower Shot")]
	[RequireComponent(typeof(Rigidbody))]
	public class TowerShot : MonoBehaviour 
	{
		private Rigidbody m_rigibody;

		public int Damage { get; private set; }

		void OnBecameInvisible()
		{
			Hide ();
		}

		void OnTriggerEnter(Collider collider)
		{
			if (collider.IsWall())
			{
				Hide ();
			}
		}

		public void Setup(Vector3 position, Quaternion rotation, int damage, Transform target, float thrustForce)
		{
			transform.localPosition = position;
			transform.localRotation = rotation;

			Damage = damage;

			SoundManager.PlaySoundEffect ("TowerShot");

			if (m_rigibody == null) m_rigibody = GetComponent<Rigidbody> ();

			var direction = (target.position - position).normalized;
			m_rigibody.AddForce (direction * thrustForce);
		}

		public void Hide()
		{
			Damage = 0;

			LevelManager.TowerShotPooling.HideShot (this);
		}
	}
}