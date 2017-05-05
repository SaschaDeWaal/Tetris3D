using UnityEngine;
using System.Collections;

//pascal case
namespace Aaron {
    //pascal case
    public class ThisIsAConvetionScript : MonoBehaviour {
        //no regions

        //const "variables"
        private const int JUMP_HEIGHT = 10;

        //static variables
        private static int jumpHeight = 10;
        //public variables
        public int myJumpHeight = 10;
        //protected variables
        protected int someJumpHeight = 10;
        //private variables
        private int secretJumpHeight = 10;


        //unity functions
            //awake
            //start
            //update
            //fixedupdate
            //lateupdate
        //same order for functions

        //camelCase
        //access specifiers are mandatory
        private int someVariable = 0;
        private int someOtherVariable = 1;

    	// Use this for initialization
    	private void Start() {
            ThisIsAFunction(5, 5.6f);
    	}
    	
        //access specifiers are mandatory
    	// Update is called once per frame
    	private void Update() {
            int x = 10;
            switch(x) {
                case JUMP_HEIGHT:
                    //do code
                    break;
            }
    	}

        //example of parameter spacing
        private void ThisIsAFunction(int someInt, float someFloat) {

        }
    }
}
