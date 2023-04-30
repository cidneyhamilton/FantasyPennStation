using System;
using UnityEngine;

/// <summary>
/// Manages Staircase module
/// </summary>
public class Staircase : MonoBehaviour
{
/*

		// Reference to Escalator Pro managers (one up and one down)
		private EscalatorPro.EscalatorPro[] _escalators;

		// Reference to each individual stairstep
		private EscalatorPro.Stair[] _stairs;
		
		void Awake()
		{
				// Cache escalators and stairs
				//_escalators = GetComponentsInChildren<EscalatorPro.EscalatorPro>();
				//_stairs = GetComponentsInChildren<EscalatorPro.Stair>();

				// Disable escalators at start
				ToggleEscalator(false);
		}

		/// <summary> Switches on or off escalator and stair animations </summary>
		void ToggleEscalator(bool isEnabled)
		{
				Array.ForEach(_escalators, esc => esc.enabled = isEnabled);
				Array.ForEach(_stairs, stair => stair.enabled = isEnabled);											
		}


		/// <summary> Turn on escalator when the player is close enough to use </summary>
		void OnTriggerEnter(Collider other)
		{
				if (other.tag == "Player")
				{
						ToggleEscalator(true);
				}
		}

		/// <summary> Turn off escalator when player is out of range </summary>
		void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						ToggleEscalator(false);
				}
		}
*/
}
