using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to summon the elevator
/// </summary>
public class ElevatorPanel : MonoBehaviour
{
		// Unique ID for this elevator
		private int _elevatorID = 0;

		// Event to summon the elevator
		public static event Action<int> OnElevatorCall;

		void Start()
		{
				// Assign the elevator ID automatically
				_elevatorID = transform.parent.GetInstanceID();
		}

		// Currently this is a trigger
		private void OnTriggerEnter(Collider other)
		{
				if (other.tag == "Player")
				{
						// Invoke event to summon the elevator
						OnElevatorCall?.Invoke(_elevatorID);
				}
		}
}
