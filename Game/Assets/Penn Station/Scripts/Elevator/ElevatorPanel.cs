using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to summon the elevator
/// </summary>
public class ElevatorPanel : ElevatorPart
{

		[SerializeField]
		private int _floor;

		[SerializeField]
		private Direction _moveDirection = Direction.Up;

		[SerializeField]
		private Animator _frameDoors, _carDoors;

		private bool summonedElevator;

		void OnEnable()
		{
				ElevatorEvents.OnAfterElevatorCall += OpenAllDoors;
				ElevatorEvents.OnAfterElevatorReachesDestination += OpenAllDoors;
		}

		void OnDisable()
		{
				ElevatorEvents.OnAfterElevatorCall -= OpenAllDoors;
				ElevatorEvents.OnAfterElevatorReachesDestination -= OpenAllDoors;
		}		

		private void OnTriggerEnter(Collider other)
		{
				
				if (other.tag == "Player")
				{
						// Only summon the elevator if player movement is enabled; ie, not when elevator actually moving
						summonedElevator = true;
						
						if (other.GetComponent<PlayerMovement>().MovementEnabled)
						{
								Logger.Log($"Summoning elevator {_elevatorID}");
								
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

		/// <summary>
		/// Open doors on a single side
		/// </summary>
		/// <param name="id">The ID of the elevator</param>
		/// <param name="floor">The floor number to toggle doors on</param>
		void OpenDoors(int id, int floor)
		{
				ToggleDoors(id, floor, true);
		}
				
		/// <summary>
		/// Close doors on single side
		/// </summary>
		/// <param name="id">The ID of the elevator</param>
		/// <param name="floor">The floor number to toggle doors on</param>
		void CloseDoors(int id, int floor)
		{
				ToggleDoors(id, floor, false);
		}

	 
		/// <summary>
		/// Open doors on all sides
		/// </summary>
		/// <param name="id">The ID of the elevator</param>
		/// <param name="floor">The floor number to toggle doors on</param>
		void OpenAllDoors(int id, int floor)
		{
				ToggleAllDoors(id, floor, true);
		}
		
		/// <summary>
		/// Close doors on all sides
		/// </summary>
		/// <param name="id">The ID of the elevator</param>
		/// <param name="floor">The floor number to toggle doors on</param>
		public void CloseAllDoors(int id, int floor)
		{
				ToggleAllDoors(id, floor, false);
		}
		

		/// <summary>
		/// Toggle doors on all sides
		/// </summary>
		/// <param name="id">The ID of the elevator</param>
		/// <param name="floor">The floor number to toggle doors on</param>
		private void ToggleAllDoors(int id, int floor, bool isOpen)
		{				
				if (id == _elevatorID && floor == _floor)
				{
						_frameDoors.SetBool("Open", isOpen);
						_carDoors.SetBool("Open", isOpen);
				}		
		}
		
		/// <summary>
		/// Toggle doors on a given side
		/// </summary>
		/// <param name="id">The ID of the elevator</param>
		/// <param name="floor">The floor number to toggle doors on</param>
		void ToggleDoors(int id, int floor, bool isOpen)
		{
				if (id == _elevatorID && floor == _floor && summonedElevator)
				{
						Logger.Log($"Toggling doors for {id} on {floor} from {gameObject.name}");
						_frameDoors.SetBool("Open", isOpen);
						_carDoors.SetBool("Open", isOpen);
				}
		}
		
}
