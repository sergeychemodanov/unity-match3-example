using System;
using System.Collections;
using UnityEngine;

namespace TestCompany.Common
{
    public class Tweener : Singleton<Tweener>
    {
        public void MoveTo(Transform target, Vector3 destination, float time, Action onComplete = null)
        {
            StartCoroutine(Move(target, destination, time, onComplete));
        }

        public void SwapPositions(Transform firstTarget, Transform secondTarget, float time, Action onComplete = null)
        {
            var firstTargetPosition = firstTarget.position;
            var secondTargetPosition = secondTarget.position;

            StartCoroutine(Move(firstTarget, secondTargetPosition, time));
            StartCoroutine(Move(secondTarget, firstTargetPosition, time, onComplete));
        }

        private IEnumerator Move(Transform target, Vector3 destination, float time, Action onComplete = null)
        {
            var startTime = Time.time;
            var startPosition = target.position;
            var progress = 0f;

            while (progress < 1)
            {
                progress = (Time.time - startTime) / time;
                var newPosition = Vector3.Lerp(startPosition, destination, progress);

                if (target != null)
                    target.position = newPosition;

                yield return null;
            }

            if (target != null)
                target.position = destination;

            onComplete?.Invoke();
        }
    }
}