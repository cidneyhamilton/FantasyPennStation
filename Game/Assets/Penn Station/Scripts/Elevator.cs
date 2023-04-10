using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
		[SerializeField]
		private Transform _targetTop, _targetBottom;

		[SerializeField]
		private float _speed = 3.0f;
		private bool _movingUp = false;

		void Update()
		{
				if (_movingUp)
				{
						Move(_targetTop);
				}
				else
				{
						Move(_targetBottom);
				}

				if (transform.position.y == _targetTop.position.y)
				{
						_movingUp = false;
				}

				if (transform.position.y == _targetBottom.position.y)
				{
						_movingUp = true;
				}
		}

		///
		/// Move towards the target platform
		/// 
		void Move(Transform target)
		{
				transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
		}
}
