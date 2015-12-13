using UnityEngine;

namespace TowerDefense
{
	[AddComponentMenu("TOWER DEFENSE/Tower Pooling")]
	public class TowerPooling : Pooling<TowerAgent>
	{
		public TowerAgent SpawnTower(Vector3 position, Quaternion rotation, TowerSettings settings)
		{
			var tower = GetObjectFromPool ();

			tower.Setup (position + new Vector3(0, .25f, 0), rotation, settings);

			return tower;
		}

		public void HideTower(TowerAgent agent)
		{
			SetObjectToPool (agent);
		}
	}
}