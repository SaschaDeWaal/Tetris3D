using UnityEngine;
using System.Collections;

namespace Sascha {
    public class GameManager : MonoBehaviour {

        const float GAME_SPEED = 0.5f;
        const float ADD_SPEED = 0.45f;
        const float SEQUENCES_TIME = 1.5f;

        public GameObject movableGridObj;
        public GameObject worldGridObj;
        public GameObject camera;
        public Hud hud;

        private WorldGrid worldGrid;
        private MovableGrid movableGrid;

        private void Start() {
            StartGame();

            StartCoroutine(MainUpdateLoop());
        }

        public void StartGame() {
            movableGrid = movableGridObj.GetComponent<MovableGrid>();
            worldGrid = worldGridObj.GetComponent<WorldGrid>();

            movableGrid.CreateRandomGroup();
            movableGrid.Init(worldGrid, camera);
            worldGrid.Init(hud);

        }

        public void GameOver() {
            hud.GameOver();
        }

        private IEnumerator MainUpdateLoop() {
            while(true) {

                movableGrid.Move(IntVector3.down);

                if(movableGrid.CheckColision()) {
                    worldGrid.Merge(movableGrid);

                    if(worldGrid.CheckForSequences()) {
                        yield return new WaitForSeconds(SEQUENCES_TIME);
                    }

                    if(!worldGrid.CheckIfGameOver()) {
                        movableGrid.CreateRandomGroup();
                    } else {
                        GameOver();
                        yield break;
                    }
                }

                yield return new WaitForSeconds(GAME_SPEED - (Input.GetAxis("speed up") * ADD_SPEED));
            }
        }

    }
}
