using System.Collections;
using UnityEngine;

namespace TestCompany.Common
{
    public class Timer : Singleton<Timer>
    {
        public event EventHandler OnOneSecondPassed;

        private void Start()
        {
            StartCoroutine(OneSecondCycle());
        }

        private IEnumerator OneSecondCycle()
        {
            var oneSecond = new WaitForSeconds(1);

            while (true)
            {
                yield return oneSecond;
                OnOneSecondPassed?.Invoke();
            }
        }
    }
}