using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the Elevator module
/// </summary>
public class ElevatorFloor : MonoBehaviour
{
		
		// Unique ID for this elevator
		private int _elevatorID = 0;
		
		[SerializeField]
		private int _floor;

		[SerializeField]
		private ElevatorPanel _LeftPanel, _RightPanel;
		
		void Start()
		{
				// Assign the elevator ID automatically
				_elevatorID = transform.GetComponentInParent<Elevator>().GetInstanceID();
		}
		
		// Close doors
		void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						_LeftPanel.CloseAllDoors(_elevatorID, _floor);
						_RightPanel.CloseAllDoors(_elevatorID, _floor);
				}
		}
}
