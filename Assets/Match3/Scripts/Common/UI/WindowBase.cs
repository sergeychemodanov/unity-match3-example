using UnityEngine;

namespace TestCompany.Common.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class WindowBase : MonoBehaviour
    {
        public Canvas Canvas { get; private set; }

        private const string UICameraTag = "UICamera";

        private WindowManager _windowManager;

        public void Bind(WindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public virtual void Close()
        {
            _windowManager.Close(this);
        }

        protected virtual void Awake()
        {
            Canvas = GetComponent<Canvas>();

            var uiCameraGo = GameObject.FindWithTag(UICameraTag);
            if (uiCameraGo == null)
                return;

            var uiCamera = uiCameraGo.GetComponent<Camera>();
            Canvas.worldCamera = uiCamera;
        }
    }
}