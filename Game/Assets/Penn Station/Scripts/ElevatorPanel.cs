using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
		[SerializeField] private int _elevatorID = 0;
		public static event Action<int> OnElevatorCall;

		void Start()
		{
				_elevatorID = transform.parent.GetInstanceID();
		}
		
		private void OnTriggerEnter(Collider other)
		{
				if (other.tag == "Player")
				{
						OnElevatorCall?.Invoke(_elevatorID);
				}
		}
}
