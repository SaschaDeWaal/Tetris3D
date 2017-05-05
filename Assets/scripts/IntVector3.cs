using UnityEngine;
using System.Collections;
using System;

namespace Sascha {

    /// <summary>
    ///  Representation of 3D vectors and points in integers
    /// </summary>
    public class IntVector3 {

        /// <summary> 
        ///   Shorthand for writing IntVector3(1, 0, 0)
        /// </summary>
        public static IntVector3 left { get { return new IntVector3(1, 0, 0); } }

        /// <summary> 
        ///   Shorthand for writing IntVector3(0, 1, 0)
        /// </summary>
        public static IntVector3 up { get { return new IntVector3(0, 1, 0); } }

        /// <summary> 
        ///   Shorthand for writing IntVector3(0, -1, 0)
        /// </summary>
        public static IntVector3 down { get { return new IntVector3(0, -1, 0); } }

        /// <summary> 
        ///   Shorthand for writing IntVector3(0, 0, 1)
        /// </summary>
        public static IntVector3 forward { get { return new IntVector3(0, 0, 1); } }

        /// <summary> 
        ///   Shorthand for writing IntVector3(0, 0, 0)
        /// </summary>
        public static IntVector3 zero { get { return new IntVector3(0, 0, 0); } }

        /// <summary> 
        ///   Shorthand for writing IntVector3(1, 1, 1)
        /// </summary>
        public static IntVector3 one { get { return new IntVector3(1, 1, 1); } }

        /// <summary> 
        ///   Returns a IntVector3 with the same rounded x, y, z values as the Vector3. 
        /// </summary>
        public static IntVector3 Parse(Vector3 vector3) {
            return new IntVector3(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
        }

        public int x = 0;
        public int y = 0;
        public int z = 0;

        public IntVector3() {
            
        }

        /// <summary> 
        ///   Creates a new IntVector3 with given x, y and z components
        /// </summary>
        public IntVector3(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary> 
        ///   Returns a Vector3 with the x, y, z set to the same as this IntVector3
        /// </summary>
        public Vector3 ToVector3() {
            return new Vector3(x, y, z);
        }

        public override string ToString() {
            return String.Format("({0},{1},{2})", x, y, z);
        }

        /// <summary>
        ///  This class performs an important function.
        /// </summary>
        public void Set(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary> 
        ///   Returns the world position of this IntVector3
        /// </summary>
        public IntVector3 ToWorldPos() {
            return new IntVector3();
        }

        public override bool Equals(object obj) {
            if(obj.GetType() == typeof(IntVector3)) {
                IntVector3 comp = obj as IntVector3;
                if(comp.x == this.x && comp.y == this.y && comp.z == this.z) {
                    return true;
                }
            }
            return false;
        }

        /// <summary> 
        ///   Rotate the IntVector3 with the origen of (0,0,0)
        /// </summary>
        /// <param name="rotation">The rotation</param>
        public void Rotate(IntVector3 rotation) {

            float angle = rotation.y;

            float newX = this.x * Mathf.Cos(angle * Mathf.Deg2Rad) - this.z * Mathf.Sin(angle * Mathf.Deg2Rad);
            float newZ = this.z * Mathf.Cos(angle * Mathf.Deg2Rad) + this.x * Mathf.Sin(angle * Mathf.Deg2Rad);

            this.x = Mathf.RoundToInt(newX);
            this.z = Mathf.RoundToInt(newZ);

        }

        public static bool operator !=(IntVector3 lhs, IntVector3 rhs) {
            return !(lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z);
        }

        public static bool operator ==(IntVector3 lhs, IntVector3 rhs) {
            return (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z);
        }

        public static IntVector3 operator +(IntVector3 a, IntVector3 b) {
            return new IntVector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static IntVector3 operator *(IntVector3 a, int b) {
            return new IntVector3(a.x * b, a.y * b, a.z * b);
        }


        public static IntVector3 operator -(IntVector3 lhs, IntVector3 rhs) {
            return new IntVector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public override int GetHashCode() {
            int id = int.Parse(String.Format("{0}{1}{2}", x, Math.Abs(y), Math.Abs(z)));
            if(y < 0 || z < 0) {
                id *= -1;
            }
            return id;
        }

    }
}