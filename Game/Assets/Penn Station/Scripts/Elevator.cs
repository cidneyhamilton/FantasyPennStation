using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
		Up,
		Down,
		None
}

public class Elevator : MonoBehaviour
{
		[SerializeField]
		private Transform _targetTop, _targetBottom;

		[SerializeField]
		private float _speed = 3.0f;

		[SerializeField]
		private Direction _moveDirection = Direction.None;

		[SerializeField]
		private bool _playerInside;

		[SerializeField]
		private bool _reachedDestination = true;

		[SerializeField] private int _elevatorID;
		
		// Reference to the player object
		private Transform _player;

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
						other.transform.parent = this.transform;
						_player = other.transform;
						TogglePlayerMovement(false);
						PlayerEntered();
				}				
		}

		private void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						other.transform.parent = null;
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
						_player.GetComponent<PlayerMovement>().MovementEnabled = enabled;
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
