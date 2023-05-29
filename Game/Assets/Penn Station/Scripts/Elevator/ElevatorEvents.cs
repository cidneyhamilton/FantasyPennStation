using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Event class for elevators
/// </summary>
public class ElevatorEvents 
{
		
		public delegate void ElevatorEvent(int id, Direction direction);
		public delegate void ElevatorDoorEvent(int id, int floor);
		
		// Event to summon the elevator
		public static ElevatorEvent OnElevatorCall;

		// Event after elevator is summoned
		public static ElevatorDoorEvent OnAfterElevatorCall;

		// Event after the elevator reaches the destination floor
		public static ElevatorDoorEvent OnAfterElevatorReachesDestination;

}
