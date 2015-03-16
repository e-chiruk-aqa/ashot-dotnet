namespace AShotNet.ScreenTaker
{
    using System;
    using System.Drawing;
    using System.Threading;
    using Extentions;
    using OpenQA.Selenium;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    ///     <p />
    ///     Pastes together parts of screenshots
    ///     Used when driver shoots viewport only
    /// </author>
    [Serializable]
    public abstract class VerticalPastingShootingStrategy : HeadCuttingShootingStrategy
    {
        protected internal int scrollTimeout = 0;

        protected internal VerticalPastingShootingStrategy(int scrollTimeout, int headerToCut)
            : base(headerToCut)
        {
            this.scrollTimeout = scrollTimeout;
        }

        public virtual void setScrollTimeout(int scrollTimeout)
        {
            this.scrollTimeout = scrollTimeout;
        }

        public override Bitmap getScreenshot(IWebDriver wd)
        {
            var js = (IJavaScriptExecutor) wd;
            int allH = this.getFullHeight(wd);
            int allW = this.getFullWidth(wd);
            int winH = this.getWindowHeight(wd);
            int scrollTimes = allH/winH;
            int tail = allH - winH*scrollTimes;
            var finalImage = new Bitmap(allW, allH);
            Graphics graphics = Graphics.FromImage(finalImage);
            for (int n = 0; n < scrollTimes; n++)
            {
                js.ExecuteScript("scrollTo(0, arguments[0])", winH*n);
                this.waitForScrolling();
                Bitmap part = base.getScreenshot(wd);
                graphics.DrawImage(part, 0, n*winH);
            }
            if (tail > 0)
            {
                js.ExecuteScript("scrollTo(0, document.body.scrollHeight)");
                this.waitForScrolling();
                Bitmap last = base.getScreenshot(wd);
                Bitmap tailImage = last.GetSubImage(0, last.Height - tail, last.Width, tail);
                graphics.DrawImage(tailImage, 0, scrollTimes*winH);
            }
            graphics.Dispose();
            return finalImage;
        }

        private void waitForScrolling()
        {
            try
            {
                Thread.Sleep(this.scrollTimeout);
            }
            catch (Exception) {}
        }

        public abstract int getFullHeight(IWebDriver driver);

        public abstract int getFullWidth(IWebDriver driver);

        public abstract int getWindowHeight(IWebDriver driver);
    }
}
