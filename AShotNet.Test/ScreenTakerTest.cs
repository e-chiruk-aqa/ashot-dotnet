namespace AShotNet.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using Util;
    using Screenshot = AShotNet.Screenshot;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [TestClass]
    public class ScreenTakerTest
    {
        /// <exception cref="System.IO.IOException" />
        public static IWebDriver getDriver()
        {
            IWebDriver driverMock = new FirefoxDriver();


            /*
            new Mock<IWebDriver>()(org.mockito.Mockito.withSettings().extraInterfaces(typeof(ITakesScreenshot), typeof(IJavaScriptExecutor))));
			org.mockito.Mockito.when(asTakingScreenshot(driverMock).getScreenshotAs(org.openqa.selenium.OutputType
				.BYTES)).thenReturn(ru.yandex.qatools.ashot.util.ImageTool.toByteArray(ru.yandex.qatools.elementscompare.tests.DifferTest
				.IMAGE_A_SMALL));
			org.mockito.Mockito.when(asJavascriptExecutor(driverMock).ExecuteScript(org.mockito.Matchers.any
				<string>())).thenReturn("{0,0,100,100}");*/
            return driverMock;
        }

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void testDpr()
        {
            Screenshot screenshot = new AShot().Dpr(2).TakeScreenshot(getDriver());
            Assert.AreEqual(screenshot.getImage(), ImageTool.equalImage(DifferTest.loadImage("img/expected/dpr.png")));
        }

        private static ITakesScreenshot asTakingScreenshot(IWebDriver
            driver)
        {
            return (ITakesScreenshot) driver;
        }

        private static IJavaScriptExecutor asJavascriptExecutor(IWebDriver
            driver)
        {
            return (IJavaScriptExecutor) driver;
        }
    }
}
