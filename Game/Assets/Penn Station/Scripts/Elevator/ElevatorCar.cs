using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Manage the Elevator Car module
/// </summary>
public class ElevatorCar : MonoBehaviour
{
		// Coordinates for each floor the elevator can navigate to 
		[SerializeField]
		private Transform _targetTop, _targetBottom;

		// Speed of the elevator
		[SerializeField]
		private float _speed = 3.0f;

		// Floor of the elevator (0 for ground floor, 1 for top floor)
		[SerializeField]
		private int _floor = 0;
		
		// Current direction the elevator is moving
		[SerializeField]
		private Direction _moveDirection = Direction.None;

		// True if the elevator reached the target floor
		[SerializeField]
		private bool _reachedDestination = true;

		// Unique id for this elevator
		private int _elevatorID;
		
		// Reference to the player object
		[SerializeField]
		private Transform _player;
		
		// The amount to offset the player's transform by when in the elevator
		const float Y_OFFSET = 1.1176f;

		void OnEnable()
		{
				ElevatorEvents.OnElevatorCall += OnCallElevator;
		}
		
		void OnDisable()
		{
				ElevatorEvents.OnElevatorCall -= OnCallElevator;
		}
		
		void FixedUpdate()
		{
				// Move(_moveDirection);
				// ChangeDirection(transform.position.y);
		}

		void Start()
		{
				// Assign the elevator ID automaticaly
				_elevatorID = transform.GetComponentInParent<Elevator>().GetInstanceID();

				// Elevator starts on specified floor
				if (_floor == 0)
				{
						transform.SetPositionAndRotation(_targetBottom.position, _targetBottom.rotation);
				}
				else if (_floor == 1)
				{
						transform.SetPositionAndRotation(_targetTop.position, _targetTop.rotation);
				}
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

		/*
		/// <summary>
		/// Move in the target direction
		/// </summary>
		/// <param name="direction">The target movement direction</param>
		void Move(Direction direction)
		{
				// If there's no detected player, don't move the elevator
				if (_player == null)
				{
						return;
				}
				
				// Stop moving after reaching the destination
				if (_reachedDestination)
				{
						Debug.Log("Turn on player movement after reaching destination.");
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
		*/

		/// <summary>
		/// Called when the player enters the elevator
		/// </summary>
		private void OnPlayerEntered(Collider other)
		{
				Debug.Log("Player entering elevator.");				

				MovePlayerToElevator(other.transform);
				
				// Immediately go up or down
				TogglePlayerMovement(false);
				
				CallElevatorToOppositeFloor();

				TogglePlayerMovement(true);

				MovePlayerFromElevator(other.transform);
				
		}

		private void MovePlayerToElevator(Transform player)
		{
				player.SetParent(this.transform);
				player.localScale = Vector3.one;
				player.localPosition = new Vector3(0, Y_OFFSET, 0);
				_player = player;
		}

		private void MovePlayerFromElevator(Transform player)
		{
				player.SetParent(null, false);
				player.localScale = Vector3.one;
				_player = null;
		}
		
		/// <summary>
		/// Called when the player exits the elevator
		/// </summary>
		private void OnPlayerExited(Collider other)
		{
				Debug.Log("Player exiting elevator.");				

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
		void OnCallElevator(int ID, Direction direction)
		{
				if (ID == _elevatorID && _moveDirection == Direction.None)
				{
						CallElevator(direction);
				}
		}

		void CallElevator(Direction direction)
		{
				Debug.Log($"Calling elevator in direction {direction}");
				if (direction == Direction.Up && _floor == 1)
				{
						// Don't bother calling the elevator; we're already at the top
				}
				else if (direction == Direction.Down && _floor == 0)
				{
						// Already at the bottom
				}
				else if (direction == Direction.Up && _floor == 0)
				{
						// Call the elevator to the top
						Debug.Log("Call the elevator to the top");
						CallElevatorToFloor(1, direction);
				}
				else if (direction == Direction.Down && _floor == 1)
				{
						// Call the elevator to the bottom
						Debug.Log("Call the elevator to the bottom");
						CallElevatorToFloor(0, direction);
				}

				ElevatorEvents.OnAfterElevatorCall(_elevatorID, _floor);
		}

		void CallElevatorToFloor(int floor, Direction direction)
		{

				Debug.Log($"Calling elevator to floor {floor}");
				Transform target = floor == 0 ? _targetBottom : _targetTop;				

				_reachedDestination = false;
				_moveDirection = direction;
				
				while (transform.position.y != target.position.y)
				{						
						transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
				}

				_floor = floor;
				_reachedDestination = true;
				_moveDirection = Direction.None;
		}

		// Call the elevator to the opposite floor
		void CallElevatorToOppositeFloor()
		{
				int targetFloor = _floor == 0 ? 1 : 0;
				Direction targetDirection = targetFloor == 1 ? Direction.Down : Direction.Up;
				
				CallElevatorToFloor(targetFloor, targetDirection);
				
		}

}
