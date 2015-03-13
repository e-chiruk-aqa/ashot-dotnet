namespace AShotNet.ScreenTaker
{
    using System.Drawing;
    using System.Threading;
    using OpenQA.Selenium;

    /// <author>
	/// <a href="pazone@yandex-team.ru">Pavel Zorin</a>
	/// <p/>
	/// Pastes together parts of screenshots
	/// Used when driver shoots viewport only
	/// </author>
	[System.Serializable]
	public abstract class VerticalPastingShootingStrategy : HeadCuttingShootingStrategy
	{
		protected internal int scrollTimeout = 0;

		public virtual void setScrollTimeout(int scrollTimeout)
		{
			this.scrollTimeout = scrollTimeout;
		}

		protected internal VerticalPastingShootingStrategy(int scrollTimeout, int headerToCut
			)
			: base(headerToCut)
		{
			this.scrollTimeout = scrollTimeout;
		}

    public override Bitmap getScreenshot(IWebDriver
			 wd)
		{
			IJavaScriptExecutor js = (IJavaScriptExecutor)wd;
			int allH = getFullHeight(wd);
			int allW = getFullWidth(wd);
			int winH = getWindowHeight(wd);
			int scrollTimes = allH / winH;
			int tail = allH - winH * scrollTimes;
			Bitmap finalImage = new Bitmap(allW, 
				allH, java.awt.image.BufferedImage.TYPE_4BYTE_ABGR);
			java.awt.Graphics2D graphics = finalImage.createGraphics();
			for (int n = 0; n < scrollTimes; n++)
			{
				js.executeScript("scrollTo(0, arguments[0])", winH * n);
				this.waitForScrolling();
				java.awt.image.BufferedImage part = base.getScreenshot(wd);
				graphics.drawImage(part, 0, n * winH, null);
			}
			if (tail > 0)
			{
				js.executeScript("scrollTo(0, document.body.scrollHeight)");
				this.waitForScrolling();
				java.awt.image.BufferedImage last = base.getScreenshot(wd);
				java.awt.image.BufferedImage tailImage = last.getSubimage(0, last.getHeight() - tail
					, last.getWidth(), tail);
				graphics.drawImage(tailImage, 0, scrollTimes * winH, null);
			}
			graphics.dispose();
			return finalImage;
		}

		private void waitForScrolling()
		{
			try
			{
				Thread.Sleep(this.scrollTimeout);
			}
			catch (System.Exception)
			{
			}
		}

		public abstract int getFullHeight(IWebDriver driver);

		public abstract int getFullWidth(IWebDriver driver);

		public abstract int getWindowHeight(IWebDriver driver);
	}
}
