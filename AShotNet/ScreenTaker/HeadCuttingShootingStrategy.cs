namespace AShotNet.ScreenTaker
{
    using System.Drawing;
    using OpenQA.Selenium;

    /// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	[System.Serializable]
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
			return baseImage.getSubimage(0, this.headerToCut, w, h - this.headerToCut);
		}
	}
}
