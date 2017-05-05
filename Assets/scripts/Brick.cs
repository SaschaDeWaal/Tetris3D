using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sascha;

namespace Sascha {
    public class Brick : MonoBehaviour, IReusableObject {

        public IntVector3 localPosition;
        private Color color;

        public void Setup(Color color, IntVector3 pos) {
            this.color = color;
            localPosition = pos;

            GetComponent<Renderer>().material.color = color;

        }
    }
}
