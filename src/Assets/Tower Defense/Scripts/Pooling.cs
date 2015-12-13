using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
	public abstract class Pooling<T> : MonoBehaviour where T : Component
    {
		private List<T> m_pool = new List<T>();

		[Header("Pool Settings")]
        [SerializeField] private T m_prefab;
		[SerializeField] private Transform m_parent;
        [SerializeField] private int m_poolSize = 1;
        [SerializeField] private bool m_poolCanGrow = false;

        public virtual void Awake()
        {
            if (m_prefab == null)
            {
                Debug.LogError("Has not been defined a prefab!");
            }

            GeneratePool();
        }

		public void SetObjectToPool(T oldObject)
		{
			oldObject.gameObject.SetActive(false);

			oldObject.transform.localPosition = Vector3.zero;
			oldObject.transform.localRotation = Quaternion.identity;
		}

		protected T GetObjectFromPool(bool active = true)
        {
            for (int i = 0; i < m_pool.Count; i++)
            {
                T obj = m_pool[i];

				if (!obj.gameObject.activeInHierarchy)
				{
					obj.gameObject.SetActive(active);
					
					return obj;
				}
            }

            if (m_poolCanGrow)
            {
                var obj = CreateNewObject();

                obj.gameObject.SetActive(active);

                return obj;
            }

            return null;
        }

        private void GeneratePool()
        {
            for (int i = 0; i < m_poolSize; i++)
            {
                CreateNewObject();
            }
        }

		private T CreateNewObject()
        {
            var newObject = Instantiate(m_prefab) as T;

			newObject.gameObject.SetActive(false);
			
			newObject.transform.SetParent(m_parent);
			newObject.transform.localPosition = Vector3.zero;
			newObject.transform.localRotation = Quaternion.identity;

            m_pool.Add(newObject);

            return newObject;
        }
    }
}