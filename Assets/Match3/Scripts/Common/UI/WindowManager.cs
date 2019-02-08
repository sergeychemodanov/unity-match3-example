using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestCompany.Common.UI
{
    public class WindowManager : MonoBehaviour
    {
        public List<WindowBase> Windows { get; }

        private int _sortingOrder;

        public WindowManager()
        {
            Windows = new List<WindowBase>();
        }

        public T Show<T>() where T : WindowBase
        {
            _sortingOrder++;

            var type = typeof(T);
            var windowPrefab = Resources.Load<T>($"Windows/{type.Name}");
            var window = Instantiate(windowPrefab, transform, false);

            window.Bind(this);
            window.Canvas.sortingOrder = _sortingOrder;
            Windows.Add(window);

            return window;
        }

        public void Close(WindowBase window)
        {
            var lastWindow = Windows.LastOrDefault();
            if (lastWindow == window)
                _sortingOrder--;

            DestroyWindow(window);
        }

        public void Close<T>() where T : WindowBase
        {
            var lastWindow = Windows.LastOrDefault();
            if (lastWindow is T)
            {
                _sortingOrder--;
                DestroyWindow(lastWindow);
                return;
            }

            DestroyWindow(Windows.LastOrDefault(w => w is T));
        }

        public void CloseAll()
        {
            foreach (var window in Windows)
                Destroy(window.gameObject);

            Windows.Clear();
            _sortingOrder = 0;
        }

        private void DestroyWindow(WindowBase window)
        {
            if (window == null)
                return;

            Windows.Remove(window);
            Destroy(window.gameObject);

            if (Windows.Count <= 0)
                _sortingOrder = 0;
        }
    }
}