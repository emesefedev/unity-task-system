using UnityEngine;
using System.Collections;
using System;


namespace TaskSystem {

    public class Worker : MonoBehaviour, IWorker {

        public void MoveTo(Vector3 position, Action onArrivedAtPosition = null)
        {
            StartCoroutine(MoveToCoroutine(position, onArrivedAtPosition));
        }

        private IEnumerator MoveToCoroutine(Vector3 position, Action onArrivedAtPosition = null)
        {
            Vector3 direction = position - gameObject.transform.position;
            float distanceToTarget = direction.magnitude;

            Debug.Log(GetPosition());
            while (distanceToTarget > 0.1f)
            {
                if (TryMove(direction.normalized, 40f * Time.deltaTime))
                {
                    direction = position - gameObject.transform.position;
                    distanceToTarget = direction.magnitude;
                    Debug.Log(GetPosition());
                    yield return null;
                }
                else
                {
                    yield break;    
                }
            }
            
            onArrivedAtPosition?.Invoke();
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private bool CanMove(Vector3 direction, float distance)
        {
            return Physics2D.Raycast(transform.position, direction, distance).collider == null;
        }

        private bool TryMove(Vector3 baseDirection, float distance)
        {
            Vector3 direction = baseDirection;
            bool canMove = CanMove(direction, distance);
            if (!canMove)
            {
                // Hit something. Can't move diagonally
                // Test if can move horizontally
                direction = new Vector3(baseDirection.x, 0, 0).normalized;
                canMove = direction.x != 0 && CanMove(direction, distance);

                if (!canMove)
                {
                    // Can't move horizontally
                    // Test if can move vertically
                    direction = new Vector3(0, baseDirection.y, 0).normalized;
                    canMove = direction.y != 0 && CanMove(direction, distance);
                }
            }

            if (canMove)
            {
                // Can move vertically
                transform.position += direction * distance;
                return true;
            }

            return false;
        }

    }

}