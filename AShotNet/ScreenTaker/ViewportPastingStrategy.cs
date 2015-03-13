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
            return ((int) js.ExecuteScript("return $(document).height()"));
        }

        public override int getFullWidth(IWebDriver driver)
        {
            var js = (IJavaScriptExecutor
                ) driver;
            return ((int) js.ExecuteScript("return $(window).width()"));
        }

        public override int getWindowHeight(IWebDriver driver)
        {
            var js = (IJavaScriptExecutor
                ) driver;
            return ((int) js.ExecuteScript("return $(window).height()"));
        }
    }
}
