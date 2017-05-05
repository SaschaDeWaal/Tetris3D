using UnityEngine;
using System.Collections;

namespace Sascha {
    public class Camera : MonoBehaviour {

        private const float ROTATE_SPEED = 5f;

        private void Update() {
            transform.RotateAround(new Vector3(4.5f, 0f, 4.5f), new Vector3(0, 1, 0), Input.GetAxis("Mouse X") * ROTATE_SPEED);
        }
    }
}
