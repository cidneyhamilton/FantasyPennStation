using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all elevator components
/// </summary>
public abstract class ElevatorPart : MonoBehaviour
{
		
		/// <summary> Unique id for this elevator </summary>
		protected int _elevatorID;

		
		protected void Start()
		{
				// Assign the elevator ID automaticaly
				_elevatorID = transform.GetComponentInParent<Elevator>().GetInstanceID();				
		}
}