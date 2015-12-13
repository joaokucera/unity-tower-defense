using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Tower Shot Pooling")]
	public class TowerShotPooling : Pooling<TowerShot>
	{
		[Header("Shot Settings")]
		[Range(100f, 1000f)] public float m_thrustForce;

		public void SpawnShot(Vector3 position, Quaternion rotation, int damage, Transform target)
		{
			var shot = GetObjectFromPool ();

			shot.Setup (position, rotation, damage, target, m_thrustForce);
		}

		public void HideShot(TowerShot shot)
		{
			SetObjectToPool (shot);
		}
	}
}