using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

namespace UIControls
{
    public class UIController : MonoBehaviour
    {

        // Use this for initialization
        public Text scoreText;
		public Text completeText;
		string scoreString = "Коробок собрано: ";
        void Start()
        {
			scoreText.text = scoreString + "0";
			GameManager.gmInst.OnScoreChange += OnChangeScore;
			GameManager.gmInst.OnLevelCompleted += OnLevelComplete;
        }

        void OnChangeScore()
		{
			scoreText.text = scoreString + GameManager.gmInst.currentScore.ToString();
		}

		void OnLevelComplete()
		{
			completeText.gameObject.SetActive(true);
			completeText.transform.localScale = new Vector3(1, 1, 1);
			StartCoroutine(LevelCompleteText());
		}

		IEnumerator LevelCompleteText()
		{
			float delta = 1.005f;
			float initScale = 1.0f;
			while (initScale <= 1.2f)
			{
				initScale *= delta;
				completeText.transform.localScale = new Vector3(initScale, initScale, 1);
				yield return new WaitForEndOfFrame();
			}

			yield return new WaitForSeconds(1.0f);

			completeText.gameObject.SetActive(false);
			GameManager.gmInst.FinalizeLevel();
		}
    }
}