using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControls
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager gmInst;
		public Dictionary<GameObject, BoxController> boxesContainer;
		
		public Action OnScoreChange;
		public Action OnLevelCompleted;

		
		public FieldManager fieldManager;

		public int walkDistance{get; private set;}
		public float walkSpeed;
		public int currentScore{get; private set;}

		[SerializeField]
		int maxScore;


        void Awake()
        {
            if (gmInst == null)
            {
                gmInst = this;
            }

			currentScore = 0;
			walkDistance = 1;

			boxesContainer = new Dictionary<GameObject, BoxController>();
        }

		// Correct player position due chosen FieldManager
		public void CorrectPlayerPosition(GameObject go)
		{
			go.transform.position = fieldManager.GetAdaptedCoordinates(go.transform.position);
		}

		public void AddBoxToContaner(GameObject go, BoxController bc)
		{
			// Check coords and correct them due chosen FieldManager
			go.transform.position = fieldManager.GetAdaptedCoordinates(go.transform.position);

			boxesContainer.Add(go, bc);
		}

		public bool CanMoveFurtherPlayer(Vector2Int dir, Vector3 coords)  // Check possibility of moving for player
		{	// Check, if here can be boxed
			Vector3 boxCoords = coords + new Vector3Int(dir.x, dir.y, 0) * walkDistance;
			
			GameObject box = null;
			foreach (var item in boxesContainer)
			{
				if (item.Key.transform.position == boxCoords)
				{
					box = item.Key;
					break;
				}
			}
			if (box != null)
			{
				bool check = CanMoveFurtherBox(dir, box.transform.position);
				
				if (check)
				{
					boxesContainer[box].moveVector = dir;
					boxesContainer[box].StartMove();
				}
				return check;
			}

			
			// Get next tile coords
			return fieldManager.IsPassible(boxCoords);
		}
		public bool CanMoveFurtherBox(Vector2Int dir, Vector3 coords) // Check possibility of moving for box
		{
			// Also check if there is no boxes
			Vector3 newPos = new Vector3Int(dir.x, dir.y, 0) * walkDistance + coords;
			foreach (var item in boxesContainer)
			{
				if (item.Key.transform.position == newPos)
				return false;
			}

			// Get next tile coords
			return fieldManager.IsPassible(newPos);
		}

		public bool ReachDestination(Vector3 coords)
		{
			// Check if here is target place
			return fieldManager.IsTarget(coords);
		}

		public void GetTarget()
		{
			currentScore += 1;
			currentScore = Mathf.Min(currentScore, maxScore);
			OnScoreChange();

			if (currentScore == maxScore)
			{
				OnLevelCompleted();
			}
		}

		public void LeaveTarget()
		{
			currentScore -= 1;
			currentScore = Mathf.Max(currentScore, 0);
			OnScoreChange();
		}

		public void FinalizeLevel()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		} 
    }

}