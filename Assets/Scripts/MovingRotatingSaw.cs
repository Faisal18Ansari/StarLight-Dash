using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MovingRotatingSaw : MonoBehaviour
    {
        [Header("Rotation")]
        public Vector3 rotationAxis = Vector3.forward;//rotate around z axis
        public float rotationSpeed = 360f;//rotation speed

        [Header("Movement")]
        public Vector3 moveDirection = Vector3.right;
        public float moveSpeed = 2f;
        public float moveDistance = 3f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private bool isMoving = true;
        // Start is called before the first frame update
        void Start()
        {
            startPosition = transform.position;
            targetPosition=startPosition+moveDirection*moveDistance;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
            if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    isMoving = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startPosition) < 0.01f)
                {
                    isMoving = true;
                }
            }
        }
    }
}
