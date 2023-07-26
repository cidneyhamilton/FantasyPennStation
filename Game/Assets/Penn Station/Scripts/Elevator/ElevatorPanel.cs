using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ElevatorPanel.cs
///
/// Used to summon the elevator
/// </summary>
public class ElevatorPanel : ElevatorPart
{

		private int _floor;

		[SerializeField]
		private Animator _frameDoors, _carDoors;

		private bool _summonedElevator;

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

		protected override void Start()
		{
				base.Start();
				_floor = GetComponentInParent<ElevatorFloor>().Floor;
		}
		
		private void OnTriggerEnter(Collider other)
		{
				
				if (other.tag == "Player")
				{
						// Only summon the elevator if player movement is enabled; ie, not when elevator actually moving
						_summonedElevator = true;
						
						if (other.GetComponent<PlayerMovement>().MovementEnabled)
						{
								Logger.Log($"Summoning elevator {_elevatorID}");
								
								// Invoke event to summon the elevator
								ElevatorEvents.OnElevatorCall(_elevatorID, _floor);
						}
				}
		}

		private void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						CloseDoors(_elevatorID, _floor);
						_summonedElevator = false;
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
				if (id == _elevatorID && floor == _floor && _summonedElevator)
				{
						Logger.Log($"Toggling doors for {id} on {floor} from {gameObject.name}, open: {isOpen}");
						_frameDoors.SetBool("Open", isOpen);
						_carDoors.SetBool("Open", isOpen);
				}
		}
		
}
