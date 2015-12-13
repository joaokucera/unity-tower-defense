using UnityEngine;

namespace TowerDefense
{
    public static class ComponentExtensions
    {
		public static bool IsTower(this Collider collider)
		{
			return collider.CompareTag ("Tower");
		}

		public static bool IsTowerShot(this Collider collider)
		{
			return collider.CompareTag ("TowerShot");
		}

		public static bool IsCreep(this Collider collider)
		{
			return collider.CompareTag ("Creep");
		}

		public static bool IsCreepExit(this Collider collider)
		{
			return collider.CompareTag ("CreepExit");
		}

		public static bool IsWall(this Collider collider)
		{
			return collider.CompareTag ("Wall");
		}
    }
}