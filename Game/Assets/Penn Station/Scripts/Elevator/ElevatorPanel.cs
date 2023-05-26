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

		private bool summonedElevator;

		void OnEnable()
		{
				ElevatorEvents.OnAfterElevatorCall += OpenDoors;
				ElevatorEvents.OnAfterElevatorReachesDestination += OpenAllDoors;
		}

		void OnDisable()
		{
				ElevatorEvents.OnAfterElevatorCall -= OpenDoors;
				ElevatorEvents.OnAfterElevatorReachesDestination -= OpenAllDoors;
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
						summonedElevator = true;
						
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
						summonedElevator = false;
				}
		}


		// Open doors on a single side
		void OpenDoors(int id, int floor)
		{
				ToggleDoors(id, floor, true);
		}

		
		// Close doors on single side
		void CloseDoors(int id, int floor)
		{
				ToggleDoors(id, floor, false);
		}

		// Open doors on all sides
		void OpenAllDoors(int id, int floor)
		{
		
				if (id == _elevatorID && floor == _floor)
				{
						Debug.Log($"Opening all doors for {id} on {floor}");
						_frameDoors.SetBool("Open", true);
						_carDoors.SetBool("Open", true);
				}		
		}

		// Close doors on all sides
		public void CloseAllDoors(int id, int floor)
		{
		
				if (id == _elevatorID && floor == _floor)
				{
						Debug.Log($"Closing all doors for {id} on {floor}");
						_frameDoors.SetBool("Open", false);
						_carDoors.SetBool("Open", false);
				}		
		}
		
		void ToggleDoors(int id, int floor, bool isOpen)
		{
				if (id == _elevatorID && floor == _floor && summonedElevator)
				{
						Debug.Log($"Toggling doors for {id} on {floor} from {gameObject.name}");
						_frameDoors.SetBool("Open", isOpen);
						_carDoors.SetBool("Open", isOpen);
				}
		}
		
}
