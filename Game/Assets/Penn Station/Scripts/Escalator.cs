using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escalator : MonoBehaviour
{
		public Vector3 MovementDirection;
		public GameObject Staircase;
		
		private GameObject _player;
		private bool _onEscalator;

		void Start()
		{
				// Logger.Log($"Staircase rotation: {Staircase.transform.rotation.eulerAngles.y}");
				if (Staircase.transform.rotation.eulerAngles.y != 180)
				{
						Logger.Log("Flipping staircase.");
						// Flip movement direction						
						MovementDirection = new Vector3(MovementDirection.x * -1, MovementDirection.y, MovementDirection.z);
				}
		}
		void OnTriggerEnter(Collider other)
		{
				if (other.tag == "Player")
				{
						Logger.Log("Player entering escalator.");
						_player = other.gameObject;
						_onEscalator = true;
				}
		}

		void OnTriggerExit(Collider other)
		{
				if (other.tag == "Player")
				{
						Logger.Log("Player exiting escalator.");
						_player = null;
						_onEscalator = false;
				}
		}

		void FixedUpdate()
		{
				if (_onEscalator && _player)
				{
						_player.transform.Translate(MovementDirection * Time.deltaTime, Space.World);
				}
		}
}
