using UnityEngine;
using System.Collections;
using System;


namespace TaskSystem {

    public class Worker : MonoBehaviour, IWorker {
        
        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void MoveTo(Vector3 position, Action onArrivedAtPosition = null)
        {
            StartCoroutine(MoveToCoroutine(position, onArrivedAtPosition));
        }
        
        public void PlayVictoryAnimation(Action onFinishedAnimation = null)
        {
            StartCoroutine(PlayVictoryAnimationCoroutine(onFinishedAnimation));
        }
        
        public void PlayCleanUpAnimation(Action onFinishedAnimation = null)
        {
            StartCoroutine(PlayCleanUpAnimationCoroutine(onFinishedAnimation));
        }
        
        private IEnumerator PlayCleanUpAnimationCoroutine(Action onFinishedAnimation = null)
        {
            // TODO: Play Clean Up Animation and Wait for it to end to invoke onFinishedAnimation
            yield return new WaitForSeconds(2);
            
            onFinishedAnimation?.Invoke();
        }
        
        private IEnumerator PlayVictoryAnimationCoroutine(Action onFinishedAnimation = null)
        {
            // TODO: Play Victory Animation and Wait for it to end to invoke onFinishedAnimation
            yield return new WaitForSeconds(2);
            
            onFinishedAnimation?.Invoke();
        }

        private IEnumerator MoveToCoroutine(Vector3 position, Action onArrivedAtPosition = null)
        {
            Vector3 direction = position - gameObject.transform.position;
            float distanceToTarget = direction.magnitude;
            
            while (distanceToTarget > 0.1f)
            {
                if (TryMove(direction.normalized, 10f * Time.deltaTime))
                {
                    direction = position - gameObject.transform.position;
                    distanceToTarget = direction.magnitude;
                    yield return null;
                }
                else
                {
                    yield break;    
                }
            }
            
            onArrivedAtPosition?.Invoke();
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