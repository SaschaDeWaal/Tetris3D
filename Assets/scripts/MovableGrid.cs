using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Sascha;

namespace Sascha {
    public class MovableGrid : MonoBehaviour, IBricksGrid {

        private string[] templates = new string[] { "-1,0,0;0,0,0;1,0,0", "-2,0,0;-1,0,0;0,0,0;1,0,0;2,0,0", "0,0,0", "-1,0,0;0,0,0;1,0,0;-1,1,0;0,1,0;1,1,0", "-1,0,0;0,0,0;1,0,0;-1,1,0;0,1,0;1,1,0;0,0,1" };
        
        private Dictionary<IntVector3, Brick> bricksList = new Dictionary<IntVector3, Brick>();
        private IntVector3 position = IntVector3.zero;
        private Color color = Color.white;
        private WorldGrid worldGrid;
        private GameObject camera;
        private bool keyDown = false;
        private int rotation = 0;

        private void Update() {
            if(Input.GetKeyDown(KeyCode.E)) {
                RotateGrid(1);
            }

            if(Input.GetKeyDown(KeyCode.Q)) {
                RotateGrid(-1);
            }

            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
                if(!keyDown) {
                    IntVector3 moveDir = new IntVector3(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")), 0, Mathf.RoundToInt(Input.GetAxisRaw("Vertical")));
                    moveDir.Rotate(new IntVector3(0, Mathf.RoundToInt(360 - camera.transform.eulerAngles.y), 0));
                    if(CanMoveTo(moveDir))
                        Move(moveDir);
                    keyDown = true;
                }
            } else {
                keyDown = false;
            }
        }

        public void Init(WorldGrid _worldGrid, GameObject _camera) {
            worldGrid = _worldGrid;
            camera = _camera;
        }

        public void Clear() {
            bricksList.Clear();
        }

        public void Merge(IBricksGrid grid) {

        }

        public Brick GetBrick(IntVector3 pos) {
            return bricksList[pos];
        }

        public void AddBrick(IntVector3 pos, Brick brick) {
            bricksList.Add(pos, brick);
        }

        public Brick[] GetBricks() {
            Brick[] list = new Brick[bricksList.Count];
            bricksList.Values.CopyTo(list, 0);

            return list;
        }

        public IntVector3 GetPosition() {
            return position;
        }

        public IntVector3 GetRotation() {
            return new IntVector3(Mathf.RoundToInt(transform.eulerAngles.x), Mathf.RoundToInt(transform.eulerAngles.y), Mathf.RoundToInt(transform.eulerAngles.z));
        }

        public void RemoveBrick(IntVector3 pos) {
            bricksList.Remove(pos);
        }

        public bool BrickExsist(IntVector3 pos) {
            return bricksList.ContainsKey(pos);
        }

        public IntVector3 GetGridSize() {
            IntVector3 maxNumber = new IntVector3();
            IntVector3 minNumbers = new IntVector3();

            Brick[] list = GetBricks();

            for(int i = 0; i < list.Length; i++) {
                if(list[i].localPosition.x >= maxNumber.x)
                    maxNumber.x = list[i].localPosition.x;
                if(list[i].localPosition.y >= maxNumber.y)
                    maxNumber.y = list[i].localPosition.y;
                if(list[i].localPosition.z >= maxNumber.z)
                    maxNumber.z = list[i].localPosition.z;

                if(list[i].localPosition.x <= minNumbers.x)
                    minNumbers.x = list[i].localPosition.x;
                if(list[i].localPosition.y <= minNumbers.y)
                    minNumbers.y = list[i].localPosition.y;
                if(list[i].localPosition.z <= minNumbers.z)
                    minNumbers.z = list[i].localPosition.z;
            }
            return (maxNumber + (minNumbers*-1));
        }

        public void Move(IntVector3 vecDir) {
            SoundManager.instance.PlaySound("move");
            position = position + vecDir;
            transform.position = position.ToVector3();
        }

        public bool CheckColision() {
            Brick[] list = GetBricks();
            for(int i = 0; i < list.Length; i++) {

                IntVector3 checkPos = IntVector3.Parse(list[i].transform.localPosition);
                checkPos += position + IntVector3.down;

                if(worldGrid.BrickExsist(checkPos)) {
                    return true;
                }
            }
            return false;
        }

        public void CreateRandomGroup() {
            color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            position = new IntVector3(5, 10, 5);
            rotation = 0;
            transform.eulerAngles = Vector3.zero;
            transform.position = position.ToVector3();
            Clear();

            string[] bricksText = templates[UnityEngine.Random.Range(0, templates.Length)].Split(';');
            for(int i = 0; i < bricksText.Length; i++) {
                string[] pos = bricksText[i].Split(',');

                IntVector3 brickPos = new IntVector3(int.Parse(pos[0]), int.Parse(pos[1]), int.Parse(pos[2]));
                CreateBrick(brickPos);
            }

            SoundManager.instance.PlaySound("newBrick");
        }

        private void CreateBrick(IntVector3 pos) {
            bricksList.Add(pos, ObjectPool.Instance.GetObject<Brick>("brick") as Brick);
            bricksList[pos].transform.localPosition = (pos + position).ToVector3();
            bricksList[pos].transform.parent = transform;
            bricksList[pos].Setup(color, pos);
        }

        private bool CanMoveTo(IntVector3 to) {
            Brick[] list = GetBricks();
            IntVector3 gridSize = worldGrid.GetGridSize();

            for(int i = 0; i < list.Length; i++) {
                IntVector3 checkPos = IntVector3.Parse(list[i].transform.localPosition) + to;

                if(checkPos.x + position.x >= 0 && checkPos.x + position.x < gridSize.x &&
                    checkPos.y + position.y >= 0 && checkPos.y + position.y < gridSize.y &&
                    checkPos.z + position.z >= 0 && checkPos.z + position.z < gridSize.z &&
                    !worldGrid.BrickExsist(checkPos + position)) {
                } else if(checkPos.y + position.y < gridSize.y) {
                    return false;
                }
            }

            return true;
        }

        private void RotateGrid(int dir) {
            SoundManager.instance.PlaySound("rotate");

            Brick[] bricks = GetBricks();
            for(int i = 0; i < bricks.Length; i++) {
                IntVector3 pos = IntVector3.Parse(bricks[i].transform.localPosition);

                pos.Rotate(new IntVector3(0, 90 * dir, 0));

                bricks[i].transform.localPosition = pos.ToVector3();
            }

            PushToInside();
        }

        private void PushToInside() {
            Brick[] bricks = GetBricks();
            IntVector3 gridSize = worldGrid.GetGridSize();
            IntVector3 move = IntVector3.zero;

            for(int i = 0; i < bricks.Length; i++) {
                IntVector3 pos = IntVector3.Parse(bricks[i].transform.localPosition) + position;

                if(pos.x < 0 && move.x > pos.x)
                    move.x = pos.x;
                if(pos.z < 0 && move.z > pos.z)
                    move.z = pos.z;

                if(pos.x - gridSize.x >= 0 && (pos.x - gridSize.x + 1) < pos.x)
                    move.x = pos.x - gridSize.x + 1;
                if(pos.z - gridSize.z >= 0 && (pos.z - gridSize.z + 1) < pos.z)
                    move.z = pos.z - gridSize.z + 1;
            }

            if(move != IntVector3.zero) {
                Move(move * -1);
            }
        }
    }
}