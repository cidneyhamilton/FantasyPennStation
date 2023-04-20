using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Manages the Elevator module
/// </summary>
public class Elevator : MonoBehaviour
{

		// Coordinates for each floor the elevator can navigate to 
		[SerializeField]
		private Transform _targetTop, _targetBottom;

		// Speed of the elevator
		[SerializeField]
		private float _speed = 3.0f;

		// Current direction the elevator is moving
		private Direction _moveDirection = Direction.None;

		// True if the elevator reached the target floor
		private bool _reachedDestination = true;

		// Unique id for this elevator
		private int _elevatorID;
		
		// Reference to the player object
		private Transform _player;

		// The amount to offset the player's transform by when in the elevator
		const float Y_OFFSET = 1.1176f;

		void OnEnable()
		{
				ElevatorPanel.OnElevatorCall += OnCallElevator;
		}
		
		void OnDisable()
		{
				ElevatorPanel.OnElevatorCall -= OnCallElevator;
		}
		
		void FixedUpdate()
		{
				Move(_moveDirection);
				ChangeDirection(transform.position.y);
		}

		void Start()
		{
				// Assign the elevator ID automaticaly
				_elevatorID = transform.parent.GetInstanceID();
		}
		
		private void OnTriggerEnter(Collider other)
		{
				if (other.tag == "Player")
				{
						OnPlayerEntered(other);
				}				
		}

		private void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						OnPlayerExited(other);
				}
		}
		
		/// <summary>
		/// Move in the target direction
		/// </summary>
		/// <param name="direction">The target movement direction</param>
		void Move(Direction direction)
		{
				// Stop moving after reaching the destination
				if (_reachedDestination)
				{
						TogglePlayerMovement(true);
						return;
				}

				// Move in the correct direction
				if (direction == Direction.Up)
				{
						Move(_targetTop);
				}
				else if (direction == Direction.Down)
				{
						Move(_targetBottom);
				}
				else
				{
						// Remain stationary
				}

		}
		
		/// <summary>
		/// Move towards the target platform
		/// </summary>
		/// <param name="direction">The target movement transform</param>
		void Move(Transform target)
		{
				transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
		}

		/// <summary>
		/// Changes the direction if at the top or the bottom
		/// </summary>
		/// <param name="direction">The current position of the platform</param>
		void ChangeDirection(float currentPos)
		{
				if (currentPos == _targetTop.position.y)
				{
						if (_moveDirection == Direction.Up)
						{
								_reachedDestination = true;
								_moveDirection = Direction.None;
						}
						else
						{
								_moveDirection = Direction.Down;
						}

				}
				else if (currentPos == _targetBottom.position.y)
				{
						
						if (_moveDirection == Direction.Down)
						{
								_reachedDestination = true;
								_moveDirection = Direction.None;
						}
						else
						{
								_moveDirection = Direction.Up;
						}
				}
		}

		/// <summary>
		/// Called when the player enters the elevator
		/// </summary>
		private void OnPlayerEntered(Collider other)
		{
				// Debug.Log("Player entering elevator.");				
				other.transform.SetParent(this.transform);
				other.transform.localScale = Vector3.one;
				other.transform.localPosition = new Vector3(0, Y_OFFSET, 0);
				_player = other.transform;
				TogglePlayerMovement(false);
				_reachedDestination = false;
		}
		
		/// <summary>
		/// Called when the player exits the elevator
		/// </summary>
		private void OnPlayerExited(Collider other)
		{
				// Debug.Log("Player exiting elevator.");				
				other.transform.SetParent(null, false);
				other.transform.localScale = Vector3.one;
				_player = null;
		}
		
		/// <summary>
		/// Enables or disables the player's movement when on the elevator
		/// </summary>
		private void TogglePlayerMovement(bool enabled)
		{				
				if (_player)
				{
						if (_player.GetComponent<PlayerMovement>() != null)
						{
								_player.GetComponent<PlayerMovement>().MovementEnabled = enabled;
						}
						if (_player.GetComponent<ContinuousMoveProviderBase>() != null)
						{
								_player.GetComponent<ContinuousMoveProviderBase>().enabled = enabled;
						}
				}
		}

		/// <summary>
		/// Handles calling the elevator with the given ID
		/// </summary>
		void OnCallElevator(int ID)
		{
				if (ID == _elevatorID)
				{
						// This elevator is being called; update to show we're not at the destination
						_reachedDestination = false;
				}
		}
}
