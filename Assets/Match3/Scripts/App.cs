using UnityEngine;
using TestCompany.Common;
using TestCompany.Common.UI;
using TestCompany.Match3.Core;
using TestCompany.Match3.Static;

namespace TestCompany.Match3
{
    public class App : Singleton<App>
    {
        public IStaticDataProvider StaticDataProvider { get; private set; }

        public WindowManager WindowManager { get; private set; }

        public GameController GameController { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            StaticDataProvider = new DefaultStaticDataProvider();

            WindowManager = new GameObject("WindowManager").AddComponent<WindowManager>();
            WindowManager.transform.SetParent(transform);

            GameController = new GameController();
        }
    }
}