namespace AShotNet.ScreenTaker
{
    using System.Drawing;
    using OpenQA.Selenium;

    /// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	[System.Serializable]
	public class ScreenTaker
	{
		public const int STANDARD_DRP = 1;

		protected internal float dprX = STANDARD_DRP;

		protected internal float dprY = STANDARD_DRP;

		protected internal ShootingStrategy shootingStrategy
			 = ShootingStrategy.simple();

		public virtual Bitmap take(IWebDriver driver
			)
		{
			Bitmap screen = this.shootingStrategy.getScreenshot(driver);
			return this.scale(screen);
		}

		public ScreenTaker(ScreenTaker other)
		{
			this.dprX = other.dprX;
			this.dprY = other.dprY;
			this.shootingStrategy = other.shootingStrategy;
		}

		public ScreenTaker()
		{
		}

		public virtual ScreenTaker withShootingStrategy
			(ShootingStrategy shootingStrategy)
		{
			this.shootingStrategy = shootingStrategy;
			return this;
		}

		public virtual ScreenTaker withDpr(float dpr)
		{
			this.dprX = dpr;
			this.dprY = dpr;
			return this;
		}

		public virtual ScreenTaker withDprX(float dprX
			)
		{
			this.dprX = dprX;
			return this;
		}

		public virtual ScreenTaker withDprY(float dprY
			)
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
			int scaledWidth = (int)(screen.Width / this.dprX);
			int scaledHeight = (int)(screen.Height / this.dprY);
		Bitmap bufferedImage = new Bitmap(scaledWidth
				, scaledHeight, Bitmap.TYPE_INT_RGB);
			java.awt.Graphics2D graphics2D = bufferedImage.createGraphics();
			graphics2D.setComposite(java.awt.AlphaComposite.Src);
			graphics2D.setRenderingHint(java.awt.RenderingHints.KEY_INTERPOLATION, java.awt.RenderingHints
				.VALUE_INTERPOLATION_BILINEAR);
			graphics2D.setRenderingHint(java.awt.RenderingHints.KEY_RENDERING, java.awt.RenderingHints
				.VALUE_RENDER_QUALITY);
			graphics2D.setRenderingHint(java.awt.RenderingHints.KEY_ANTIALIASING, java.awt.RenderingHints
				.VALUE_ANTIALIAS_ON);
			graphics2D.drawImage(screen, 0, 0, scaledWidth, scaledHeight, null);
			graphics2D.dispose();
			return bufferedImage;
		}
	}
}
