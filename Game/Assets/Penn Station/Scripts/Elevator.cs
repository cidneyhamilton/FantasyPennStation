using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Potential directions the elevator can be traveling
public enum Direction {
		Up,
		Down,
		None
}

public class Elevator : MonoBehaviour
{

		// Coordinates for each floor the elvator can navigate to
		[SerializeField]
		private Transform _targetTop, _targetBottom;

		// Speed of the elevator
		[SerializeField]
		private float _speed = 3.0f;

		// Current direction the elevator is moving
		[SerializeField]
		private Direction _moveDirection = Direction.None;

		// True if the player is inside the elevator, false otherwise
		[SerializeField]
		private bool _playerInside;

		// True if the elevator reached the target floor
		[SerializeField]
		private bool _reachedDestination = true;

		// Unique id for this elevator
		[SerializeField]
		private int _elevatorID;
		
		// Reference to the player object
		private Transform _player;

		// The amount to offset the player's transform by when in the elevator
		const float yOffset = 1.1176f;

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
				_elevatorID = transform.parent.GetInstanceID();
		}
		
		/// <summary>
		/// Move in the target direction
		/// </summary>
		/// <param name="direction">The target movement direction</param>
		void Move(Direction direction)
		{
				if (_reachedDestination)
				{
						TogglePlayerMovement(true);
						return;
				}
				
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

		private void OnTriggerEnter(Collider other)
		{
				if (other.tag == "Player")
				{
						other.transform.SetParent(this.transform);
						other.transform.localScale = Vector3.one;
						other.transform.localPosition = new Vector3(0, yOffset, 0);
						_player = other.transform;
						TogglePlayerMovement(false);
						PlayerEntered();
				}				
		}

		private void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						other.transform.SetParent(null, false);
						other.transform.localScale = Vector3.one;
						_player = null;
						PlayerExited();
				}
		}

		private void PlayerEntered()
		{
				Debug.Log("Player entering elevator.");
				_playerInside = true;
				_reachedDestination = false;
		}

		private void PlayerExited()
		{
				Debug.Log("Player exiting elevator.");
				_playerInside = false;
		}

		
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

		void OnCallElevator(int ID)
		{
				if (ID == _elevatorID)
				{
						_reachedDestination = false;
				}
		}
}
