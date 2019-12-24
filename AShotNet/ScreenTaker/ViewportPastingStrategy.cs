namespace AShotNet.ScreenTaker
{
    using System;
    using OpenQA.Selenium;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public class ViewportPastingStrategy : VerticalPastingShootingStrategy
    {
        public ViewportPastingStrategy(int scrollTimeout, int headerToCut)
            : base(scrollTimeout, headerToCut) {}

        public ViewportPastingStrategy(int scrollTimeout)
            : base(scrollTimeout, 0) {}

        public override int getFullHeight(IWebDriver driver)
        {
            var js = (IJavaScriptExecutor
                ) driver;
            return (int) ((System.Int64) js.ExecuteScript("return window.innerHeight"));
        }

        public override int getFullWidth(IWebDriver driver)
        {
            var js = (IJavaScriptExecutor
                ) driver;
            return (int) ((System.Int64)js.ExecuteScript("return window.innerWidth"));
        }

        public override int getWindowHeight(IWebDriver driver)
        {
            var js = (IJavaScriptExecutor
                ) driver;
            return (int) ((System.Int64)js.ExecuteScript("return window.outerHeight"));
        }
    }
}
