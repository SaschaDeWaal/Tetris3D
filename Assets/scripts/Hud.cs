using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Sascha {
    public class Hud : MonoBehaviour {

        const string SCORE_TEXT_TEMPLATE = "Score {0}";

        private Text scoreText;
        private int score = 0;
        private GameObject gameOverScreen;

        private void Start() {
            scoreText = transform.FindChild("Score").GetComponent<Text>();
            gameOverScreen = transform.FindChild("GameOver").gameObject;
            gameOverScreen.SetActive(false);

            scoreText.text = String.Format(SCORE_TEXT_TEMPLATE, score);
        }

        public void AddScore(int add) {
            score += add;
            scoreText.text = String.Format(SCORE_TEXT_TEMPLATE, score);
        }

        public void GameOver() {
            gameOverScreen.SetActive(true);
        }

        

    }
}