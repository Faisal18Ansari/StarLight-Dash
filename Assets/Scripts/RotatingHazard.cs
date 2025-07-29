using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class RotatingHazard : MonoBehaviour
    {

        public Vector3 rotationAxis = Vector3.up;
        public float rotationSpeed = 90f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }
}
