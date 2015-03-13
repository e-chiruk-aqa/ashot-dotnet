namespace AShotNet.ScreenTaker
{
    using System.Drawing;
    using System.IO;
    using OpenQA.Selenium;

    /// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	[System.Serializable]
	public abstract class ShootingStrategy
	{
		public static ShootingStrategy simple()
		{
			return new _ShootingStrategy_21();
		}

		private sealed class _ShootingStrategy_21 : ShootingStrategy
		{
			public _ShootingStrategy_21()
			{
			}

			public override Bitmap getScreenshot(IWebDriver wd)
			{
				Stream imageArrayStream = null;
				ITakesScreenshot takesScreenshot = (ITakesScreenshot) wd;
				try
				{
				    var sreenshot = takesScreenshot.GetScreenshot();
				    var ms = new MemoryStream(sreenshot.AsByteArray);

					return new Bitmap(ms);
				}
				catch (System.IO.IOException e)
				{
					throw new System.Exception("Can not parse screenshot data", e);
				}
				finally
				{
					try
					{
						if (imageArrayStream != null)
						{
							imageArrayStream.Close();
						}
					}
					catch (System.IO.IOException)
					{
					}
				}
			}
		}

		public abstract Bitmap getScreenshot(IWebDriver wd);
	}
}
