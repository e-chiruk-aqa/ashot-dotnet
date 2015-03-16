namespace AShotNet.ScreenTaker
{
    using System;
    using System.Drawing;
    using OpenQA.Selenium;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public class ScreenTaker
    {
        public const int STANDARD_DRP = 1;

        protected internal int dprX = STANDARD_DRP;

        protected internal int dprY = STANDARD_DRP;

        protected internal ShootingStrategy shootingStrategy
            = ShootingStrategy.simple();

        public ScreenTaker(ScreenTaker other)
        {
            this.dprX = other.dprX;
            this.dprY = other.dprY;
            this.shootingStrategy = other.shootingStrategy;
        }

        public ScreenTaker() {}

        public virtual Bitmap take(IWebDriver driver)
        {
            Bitmap screen = this.shootingStrategy.getScreenshot(driver);
            return this.scale(screen);
        }

        public virtual ScreenTaker withShootingStrategy
            (ShootingStrategy shootingStrategy)
        {
            this.shootingStrategy = shootingStrategy;
            return this;
        }

        public virtual ScreenTaker withDpr(int dpr)
        {
            this.dprX = dpr;
            this.dprY = dpr;
            return this;
        }

        public virtual ScreenTaker withDprX(int dprX)
        {
            this.dprX = dprX;
            return this;
        }

        public virtual ScreenTaker withDprY(int dprY)
        {
            this.dprY = dprY;
            return this;
        }

        private Bitmap scale(Bitmap screen)
        {
            if (this.dprY == 1 && this.dprX == 1)
            {
                return screen;
            }
            int scaledWidth = screen.Width/this.dprX;
            int scaledHeight = screen.Height/this.dprY;
            var bufferedImage = (Bitmap) screen.GetThumbnailImage(scaledWidth, scaledHeight, null, IntPtr.Zero);
            return bufferedImage;
        }
    }
}
