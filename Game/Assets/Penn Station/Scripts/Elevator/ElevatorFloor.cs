using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the Elevator module
/// </summary>
public class ElevatorFloor : ElevatorPart
{
		
		[SerializeField]
		private int _floor;

		[SerializeField]
		private ElevatorPanel _LeftPanel, _RightPanel;
				
		// Close doors
		void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						if (_LeftPanel)
						{
								_LeftPanel.CloseAllDoors(_elevatorID, _floor);
						}
						if (_RightPanel)
						{
								_RightPanel.CloseAllDoors(_elevatorID, _floor);
						}
				}
		}
}
