using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the Elevator module
/// </summary>
public class ElevatorFloor : ElevatorPart
{		

		/// <summary> The sequence number of this elevator floor </summary>
		public int Floor;
		
		[SerializeField]
		private ElevatorPanel[] doors;
				
		// Close doors
		void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						Array.ForEach(doors, door => door.CloseAllDoors(_elevatorID, Floor));

				}
		}
}
