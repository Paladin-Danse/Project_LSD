/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.AI;

namespace UnrealFPS.Runtime
{
	[System.Serializable]
	public class PatrolSystem
	{
		public enum PatrolType
		{
			Random,
			Sequential
		}

		[SerializeField] PatrolType patrolType;
		[SerializeField] private Transform[] points;
		[SerializeField] private float updatePointDistance = 0.5f;

		private Transform transform;
		private NavMeshAgent navMeshAgent;
		private int point = 0;

		/// <summary>
		/// Init is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		public virtual void Initialize(Transform transform, NavMeshAgent navMeshAgent)
		{
			this.transform = transform;
			this.navMeshAgent = navMeshAgent;

			if (points.Length > 0)
			{
				switch (patrolType)
				{
					case PatrolType.Random:
						point = Random.Range(0, points.Length);
						break;
					case PatrolType.Sequential:
						point = 0;
						break;
				}
			}

		}

		public virtual void Update()
		{
			if (points.Length < 0)
				return;

			switch (patrolType)
			{
				case PatrolType.Random:
					RandomPatrol();
					break;
				case PatrolType.Sequential:
					SequentialPatrol();
					break;
			}
		}

		public virtual void RandomPatrol()
		{
			if (Vector3.Distance(transform.position, points[point].position) <= updatePointDistance)
				point = Random.Range(0, points.Length);
			else
				navMeshAgent.destination = points[point].position;
		}

		public virtual void SequentialPatrol()
		{
			if (Vector3.Distance(transform.position, points[point].position) <= updatePointDistance)
				point = (point < points.Length - 1) ? point + 1 : 0;
			else
				navMeshAgent.destination = points[point].position;
		}

		public PatrolType GetPatrolType()
		{
			return patrolType;
		}

		public void SetPatrolType(PatrolType value)
		{
			patrolType = value;
		}

		public Transform[] GetPoints()
		{
			return points;
		}

		public void SetPoints(Transform[] value)
		{
			points = value;
		}

		public float GetUpdatePointDistance()
		{
			return updatePointDistance;
		}

		public void SetUpdatePointDistance(float value)
		{
			updatePointDistance = value;
		}

		public Transform GetTransform()
		{
			return transform;
		}

		public NavMeshAgent GetNavMeshAgent()
		{
			return navMeshAgent;
		}

        public int GetCurrectPointIndex()
        {
            return point;
        }
    }
}