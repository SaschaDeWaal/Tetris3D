using UnityEngine;
using System.Collections;
using Sascha;

namespace Sascha {
    public class WorldGrid : MonoBehaviour, IBricksGrid {

        private const int size = 10;

        private Brick[,,] bricks = new Brick[size, size, size];
        private IntVector3 position = IntVector3.zero;
        private Hud hud;

        public void Init(Hud hud) {
            this.hud = hud;
        }

        public void Merge(IBricksGrid grid) {
            Brick[] bricksToMerge = grid.GetBricks();

            for(int i = 0; i < bricksToMerge.Length; i++) {

                IntVector3 brickPos = IntVector3.Parse(bricksToMerge[i].transform.localPosition);
                brickPos.Rotate(grid.GetRotation());
                brickPos += grid.GetPosition();

                if(brickPos.x < bricks.GetLength(0) && brickPos.x >= 0 && brickPos.y < bricks.GetLength(1) && brickPos.y >= 0 && brickPos.z < bricks.GetLength(2) && brickPos.z >= 0) {
                    bricks[brickPos.x, brickPos.y, brickPos.z] = bricksToMerge[i];
                    bricks[brickPos.x, brickPos.y, brickPos.z].transform.parent = transform;
                    bricks[brickPos.x, brickPos.y, brickPos.z].transform.position = brickPos.ToVector3();
                }
            }
        }

        public bool CheckIfGameOver() {
            IntVector3 worldSize = GetGridSize();
            for(int x = 0; x < worldSize.x; x++) {
                for(int z = 0; z < worldSize.z; z++) {
                    if(BrickExsist(new IntVector3(x, worldSize.y - 1, z))) {
                        return true;
                    }
                }
            }

            return false;
        }

        public IntVector3 GetPosition() {
            return position;
        }

        public IntVector3 GetRotation() {
            return new IntVector3(Mathf.RoundToInt(transform.eulerAngles.x), Mathf.RoundToInt(transform.eulerAngles.y), Mathf.RoundToInt(transform.eulerAngles.z));
        }

        public Brick GetBrick(IntVector3 pos) {
            return bricks[pos.x, pos.y, pos.z];
        }

        public void AddBrick(IntVector3 pos, Brick brick) {
            bricks[pos.x, pos.y, pos.z] = brick;
        }

        public void RemoveBrick(IntVector3 pos) {
            ObjectPool.Instance.ReturnObject(bricks[pos.x, pos.y, pos.z]);
            bricks[pos.x, pos.y, pos.z] = null;
        }

        public void MoveBrick(IntVector3 from, IntVector3 to) {
            bricks[to.x, to.y, to.z] = bricks[from.x, from.y, from.z];
            bricks[to.x, to.y, to.z].transform.localPosition = to.ToVector3();
            bricks[from.x, from.y, from.z] = null;
        }

        public bool BrickExsist(IntVector3 pos) {
            return ((pos.y < 0) || (bricks[pos.x, pos.y, pos.z] != null));
        }

        public Brick[] GetBricks() {
            Brick[] retBricks = new Brick[bricks.GetLength(0) * bricks.GetLength(1) * bricks.GetLength(2)];

            int index = 0;
            for(int x = 0; x < bricks.GetLength(0); x++) {
                for(int y = 0; y < bricks.GetLength(0); y++) {
                    for(int z = 0; z < bricks.GetLength(0); z++) {
                        retBricks[index] = bricks[x, y, z];
                        index++;
                    }
                }
            }

            return retBricks;
        }

        public IntVector3 GetGridSize() {
            return new IntVector3(size, size, size);
        }

        public void Clear() {
            bricks = new Brick[size, size, size];
        }

        public bool CheckForSequences() {

            bool found = false;

            for(int y = 0; y < bricks.GetLength(1); y++) {

                if(CheckOneDirectionForSequences(y, IntVector3.left)) {
                    found = true;
                }

                if(CheckOneDirectionForSequences(y, IntVector3.forward)) {
                    found = true;
                }
            }

            return found;
        }

        private bool CheckOneDirectionForSequences(int y, IntVector3 dir) {

            int axis1 = dir.x;
            int axis2 = dir.z;

            bool found = false;
            int[] axis = new int[] { 0, 0 };

            for(axis[0] = 0; axis[0] < size; axis[0]++) {
                int rowCount = 0;
                for(axis[1] = 0; axis[1] < size; axis[1]++) {
                    if(!BrickExsist(new IntVector3(axis[axis1], y, axis[axis2])))
                        break;
                    rowCount++;
                }
                if(rowCount == size) {
                    if(axis[axis1] == size)
                        axis[axis1] = 0;
                    if(axis[axis2] == size)
                        axis[axis2] = 0;
                    StartCoroutine(RemoveSequences(new IntVector3(axis[axis1], y, axis[axis2]), dir));
                    found = true;
                }
            }

            return found;
        }

        private IEnumerator RemoveSequences(IntVector3 start, IntVector3 dir) {

            //remove 
            for(int i = 0; i < size; i++) {
                RemoveBrick(start + (dir * i));
                hud.AddScore(1);
                SoundManager.instance.PlaySound("removeBlock");

                yield return new WaitForSeconds(0.05f);
            }

            //move down
            for(int y = start.y + 1; y < bricks.GetLength(1); y++) {
                for(int i = 0; i < size; i++) {
                    IntVector3 pos = new IntVector3(start.x, y, start.z) + (dir * i);
                    if(BrickExsist(pos))
                        MoveBrick(pos, new IntVector3(pos.x, pos.y - 1, pos.z));

                }
                SoundManager.instance.PlaySound("move");

                yield return new WaitForSeconds(0.05f);
            }
        }

    }
}