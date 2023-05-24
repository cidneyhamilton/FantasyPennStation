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

		[SerializeField]
		private int _floor;

		[SerializeField]
		private Direction _moveDirection = Direction.Up;

		[SerializeField]
		private Animator _frameDoors, _carDoors;

		void OnEnable()
		{
				ElevatorEvents.OnAfterElevatorCall += OpenDoors;				
		}

		void OnDisable()
		{
				ElevatorEvents.OnAfterElevatorCall -= OpenDoors;
		}
		
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
						// Only summon the elevator if player movement is enabled; ie, not when elevator actually moving
						if (other.GetComponent<PlayerMovement>().MovementEnabled)
						{
								Debug.Log($"Summoning elevator {_elevatorID}");
								
								// Invoke event to summon the elevator
								ElevatorEvents.OnElevatorCall(_elevatorID, _moveDirection);
						}
				}
		}

		private void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						CloseDoors(_elevatorID, _floor);
				}
		}

		
		void OpenDoors(int id, int floor)
		{
				ToggleDoors(id, floor, true);
		}

		
		void CloseDoors(int id, int floor)
		{
				ToggleDoors(id, floor, false);
		}
		
		void ToggleDoors(int id, int floor, bool isOpen)
		{
				if (id == _elevatorID && floor == _floor)
				{
						Debug.Log($"Toggling doors for {id} on {floor}");
						_frameDoors.SetBool("Open", isOpen);
						_carDoors.SetBool("Open", isOpen);
				}
		}
		
}
