using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControls
{
    public class PlayerController : MonoBehaviour
    {
		
		
		[SerializeField]
		SpriteRenderer playerSprite;
		[SerializeField]
		Animator playerAnim;
		Vector2Int moveVector;
		enum PlayerDirects {Down, Left, Right, Up};
		[SerializeField]
		Sprite[] idleSprites;

		float playerSpeed;
		string [] triggerNames;
		List<Vector2Int> dirVectors;
		Vector2Int nextPos;
		Vector3 nextPosF;
        bool walkingState = false;
		PlayerDirects currentDir;


        void Start()
        {
			triggerNames = new string[4]{"startDown", "startLeft", "startRight", "startUp"};
			dirVectors = new List<Vector2Int>{new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, 1)};

			playerAnim.enabled = false;
			currentDir = PlayerDirects.Down;
			playerSprite.sprite = idleSprites[(int)currentDir];
			playerSpeed = GameManager.gmInst.walkSpeed;

			GameManager.gmInst.CorrectPlayerPosition(gameObject);
        }

		void Update() 
		{
			CheckInputs();
			WalkCycle();	
		}

		void CheckInputs()
		{
			if (!walkingState)
			{
				if (Input.GetKeyUp(KeyCode.W))
				{
					currentDir = PlayerDirects.Up;
					CheckForWalking();
				}
				if (Input.GetKeyUp(KeyCode.A))
				{
					currentDir = PlayerDirects.Left;
					CheckForWalking();
				}
				if (Input.GetKeyUp(KeyCode.S))
				{
					currentDir = PlayerDirects.Down;
					CheckForWalking();
				}
				if (Input.GetKeyUp(KeyCode.D))
				{
					currentDir = PlayerDirects.Right;
					CheckForWalking();
				}
			}
		}

		void CheckForWalking()
		{
			moveVector = dirVectors[(int)currentDir];
			playerSprite.sprite = idleSprites[(int)currentDir];
			if(GameManager.gmInst.CanMoveFurtherPlayer(moveVector, transform.position))
			{
				playerAnim.enabled = true;
				playerAnim.SetTrigger(triggerNames[(int)currentDir]);
				nextPosF = transform.position + new Vector3(moveVector.x, moveVector.y, 0) * GameManager.gmInst.walkDistance;
				walkingState = true;
			}
		}

        void WalkCycle()
        {
            if (walkingState)
            {
                float step = playerSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, nextPosF, step);

                if (Vector3.Distance(transform.position, nextPosF) < 0.001f)
                {
					transform.position = nextPosF;
					walkingState = false;
					playerAnim.SetTrigger("startIdle");
					playerAnim.enabled = false;
					playerSprite.sprite = idleSprites[(int)currentDir];
                }
            }
        }
    }
}
