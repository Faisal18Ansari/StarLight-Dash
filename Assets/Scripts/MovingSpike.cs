using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MovingSpike : MonoBehaviour
    {
        public Vector3 moveDirection = Vector3.right;
        public float moveDistance = 3f;
        public float moveSpeed = 2f;
        private bool isMoving=true;
        private Vector3 startPosition;
        private Vector3 targetPosition;


        // Start is called before the first frame update
        void Start()
        {
            startPosition = transform.position;
            targetPosition=startPosition+moveDirection.normalized*moveDistance;
        }

        // Update is called once per frame
        void Update()
        {
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
