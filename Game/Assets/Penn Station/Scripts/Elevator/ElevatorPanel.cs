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
		public delegate void ElevatorEvent(int ID, Direction direction);
		
		public static ElevatorEvent OnElevatorCall;

		[SerializeField]
		private Direction _moveDirection = Direction.Up;
		

		void Start()
		{
				// Assign the elevator ID automatically
				_elevatorID = transform.GetComponentInParent<Elevator>().GetInstanceID();
		}

		// Currently this is a trigger
		private void OnTriggerEnter(Collider other)
		{
				if (other.tag == "Player")
				{
						Debug.Log($"Summoning elevator {_elevatorID}");
						
						// Invoke event to summon the elevator
						OnElevatorCall?.Invoke(_elevatorID, _moveDirection);
				}
		}
}
