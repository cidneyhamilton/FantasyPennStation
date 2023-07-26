using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Manage the Elevator Car module
/// </summary>
public class ElevatorCar : ElevatorPart
{
		[SerializeField]
		/// <summary>
		/// Coordinates for each floor the elevator can navigate to
		/// </summary>
		private Transform[] _floors;

		private int maxFloor, minFloor;
		
		[SerializeField]
		/// <summary>
		/// Speed of the elevator
		/// </summary>
		private float _speed = 3.0f;

		[SerializeField]
		/// <summary>
		/// Floor of the elevator (0 for ground floor, 1 for top floor)
		/// <summary>
		private int _floor = 0;

		/// <summary>
		/// Floor of the current destination
		/// </summary>
		private int _destinationFloor = 0;


		[SerializeField]/// <summary>
		/// Current direction the elevator is moving
		/// </summary>
		private Direction _moveDirection = Direction.None;

		[SerializeField]
		/// <summary>
		/// True if the elevator reached the target floor
		/// </summary>
		private bool _reachedDestination = true;

		[SerializeField]
		/// <summary>
		/// Reference to the player object
		/// </summary>
		private Transform _player;

		// The amount to offset the player's transform by when in the elevator
		const float Y_OFFSET = 1.1176f;

		void OnEnable()
		{
				ElevatorEvents.OnElevatorCall += OnCallElevator;
				ElevatorEvents.OnAfterElevatorReachesDestination += StopMoving;
		}
		
		void OnDisable()
		{
				ElevatorEvents.OnElevatorCall -= OnCallElevator;
				ElevatorEvents.OnAfterElevatorReachesDestination -= StopMoving;
		}		

		protected void Start()
		{
				base.Start();

				// Elevator starts on specified floor
				transform.SetPositionAndRotation(_floors[_floor].position, _floors[_floor].rotation);
				minFloor = 0;
				maxFloor = _floors.Length;
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

		void FixedUpdate()
		{
				Transform target = _floors[_destinationFloor];
				
				if (_reachedDestination == false && _moveDirection != Direction.None)
				{
						// Logger.Log($"Target position: {target.position.y}, current position: { transform.position.y }");
						transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);

						// Wrap things up if we reach the destination floor
						if (Mathf.Approximately(transform.position.y, target.position.y))
						{
								
								_floor = _destinationFloor;
								_reachedDestination = true;
								_moveDirection = Direction.None;
								ElevatorEvents.OnAfterElevatorReachesDestination(_elevatorID, _destinationFloor);
						}
				}

		}

		/// <summary>
		/// Called when the player enters the elevator
		/// </summary>
		private void OnPlayerEntered(Collider other)
		{
				Logger.Log($"Player entering elevator on floor {_floor}.");				

				MovePlayerToElevator(other.transform);
				
				// Immediately go up or down
				TogglePlayerMovement(false);
				
				CallElevatorToOppositeFloor();

				
		}

		private void StopMoving(int id, int floor)
		{				
				TogglePlayerMovement(true);

				MovePlayerFromElevator();
		}
		
		/// <summary>
		/// Makes sure the player is on the elevator
		/// </summary>
		private void MovePlayerToElevator(Transform player)
		{
				player.SetParent(this.transform);
				player.localScale = Vector3.one;
				player.localPosition = new Vector3(0, 0, 0);
				_player = player;
		}

		/// <summary>
		/// Makes sure the player is off the elevator
		/// </summary>
		private void MovePlayerFromElevator()
		{
				if (_player)
				{
						_player.SetParent(null, true);
						_player.localScale = Vector3.one;
						_player = null;
				}
		}
		
		/// <summary>
		/// Called when the player exits the elevator
		/// </summary>
		private void OnPlayerExited(Collider other)
		{
				Logger.Log("Player exiting elevator.");				

		}
		
		/// <summary>
		/// Enables or disables the player's movement when on the elevator
		/// </summary>
		private void TogglePlayerMovement(bool enabled)
		{
				Logger.Log("Toggling player movement.");
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
		/// Handles calling the elevator with the given ID from the given floor
		/// </summary>
		void OnCallElevator(int ID, int floorCalledFrom)
		{

				Logger.Log($"Attempt to call elevator {ID} from {_floor} to floor {floorCalledFrom}, _elevatorID: {_elevatorID}");
				
				if (ID == _elevatorID)
				{
						if (floorCalledFrom != _floor)
						{
								Logger.Log($"Calling elevator to floor {floorCalledFrom}");
								Direction direction = floorCalledFrom > _floor ? Direction.Up : Direction.Down;					 
								CallElevator(direction);
						}
						else
						{
								// Called from this floor; can immediately open the doors
								ElevatorEvents.OnAfterElevatorCall(ID, _floor);
						}
				}
		}

		
		/// <summary>
		/// Call the elevator in the given direction
		/// </summary>
		/// <param name="direction">the target direction</direction>
		void CallElevator(Direction direction)
		{
				Logger.Log($"Calling elevator in direction {direction}");
				if (direction == Direction.Up && _floor < maxFloor)
				{
						CallElevatorToFloor(_destinationFloor, direction);
				}
				else if (direction == Direction.Down && _floor > minFloor)
				{
						CallElevatorToFloor(_destinationFloor, direction);
				}
				
				ElevatorEvents.OnAfterElevatorCall(_elevatorID, _floor);
		}

		/// <summary>
		/// Call the elevator to the nth floor in the given direction
		/// </summary>
		/// <param name="floor">the target floor</param>
		/// <param name="direction">the target direction</direction>
		void CallElevatorToFloor(int floor, Direction direction)
		{

				Logger.Log($"Calling elevator to floor {floor}");

				_reachedDestination = false;
				_moveDirection = direction;
				_destinationFloor = floor;

		}

		/// <summary>
		/// Call the elevator to the opposite floor
		/// </summary>
		void CallElevatorToOppositeFloor()
		{
				int targetFloor = _floor == 0 ? 1 : 0;
				Direction targetDirection = targetFloor == 1 ? Direction.Down : Direction.Up;				
				CallElevatorToFloor(targetFloor, targetDirection);				
		}

}
