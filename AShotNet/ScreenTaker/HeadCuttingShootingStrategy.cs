namespace AShotNet.ScreenTaker
{
    using System;
    using System.Drawing;
    using OpenQA.Selenium;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public class HeadCuttingShootingStrategy : ShootingStrategy
    {
        protected internal int headerToCut;

        public HeadCuttingShootingStrategy(int headerToCut)
        {
            this.headerToCut = headerToCut;
        }

        public override Bitmap getScreenshot(IWebDriver wd)
        {
            Bitmap baseImage = simple().getScreenshot(wd);
            int h = baseImage.Height;
            int w = baseImage.Width;
            var theRect = new Rectangle(0, this.headerToCut, w, h - this.headerToCut);
            return baseImage.Clone(theRect, baseImage.PixelFormat);
        }
    }
}
