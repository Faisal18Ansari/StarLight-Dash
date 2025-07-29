using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Pendulem : MonoBehaviour
    {
        public float speed = 50f;//max speed
        public float maxAngle = 45f;//angle to reach by pendulem
        public float currentAngle = 0f;//current angle
        public float direction = 1;//forward=1 backward=-1
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            // changes angle based on speed and direction and time
            currentAngle += speed * direction;

            // reverse direction when reached the max angle
            if (currentAngle > maxAngle)
            {
                currentAngle = maxAngle;
                direction = -1;
            }
            else if (currentAngle < -maxAngle)
            {
                currentAngle = -maxAngle;
                direction = 1;
            }
            // applying rotation around z axis for swinging
            transform.localRotation=Quaternion.Euler(0f,0f,currentAngle);
        }
    }
}
