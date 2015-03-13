using Sharpen;

namespace ru.yandex.qatools.elementscompare.tests
{
	/// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	public class ScreenTakerTest
	{
		/// <exception cref="System.IO.IOException"/>
		public static org.openqa.selenium.WebDriver getDriver()
		{
			org.openqa.selenium.WebDriver driverMock = org.mockito.Mockito.mock<org.openqa.selenium.WebDriver
				>(org.mockito.Mockito.withSettings().extraInterfaces(Sharpen.Runtime.getClassForType
				(typeof(org.openqa.selenium.TakesScreenshot)), Sharpen.Runtime.getClassForType(typeof(
				org.openqa.selenium.JavascriptExecutor))));
			org.mockito.Mockito.when(asTakingScreenshot(driverMock).getScreenshotAs(org.openqa.selenium.OutputType
				.BYTES)).thenReturn(ru.yandex.qatools.ashot.util.ImageTool.toByteArray(ru.yandex.qatools.elementscompare.tests.DifferTest
				.IMAGE_A_SMALL));
			org.mockito.Mockito.when(asJavascriptExecutor(driverMock).executeScript(org.mockito.Matchers.any
				<string>())).thenReturn("{0,0,100,100}");
			return driverMock;
		}

		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void testDpr()
		{
			ru.yandex.qatools.ashot.Screenshot screenshot = new ru.yandex.qatools.ashot.AShot
				().dpr(2).takeScreenshot(getDriver());
			NUnit.Framework.Assert.assertThat(screenshot.getImage(), ru.yandex.qatools.ashot.util.ImageTool
				.equalImage(ru.yandex.qatools.elementscompare.tests.DifferTest.loadImage("img/expected/dpr.png"
				)));
		}

		private static org.openqa.selenium.TakesScreenshot asTakingScreenshot(org.openqa.selenium.WebDriver
			 driver)
		{
			return (org.openqa.selenium.TakesScreenshot)driver;
		}

		private static org.openqa.selenium.JavascriptExecutor asJavascriptExecutor(org.openqa.selenium.WebDriver
			 driver)
		{
			return (org.openqa.selenium.JavascriptExecutor)driver;
		}
	}
}
