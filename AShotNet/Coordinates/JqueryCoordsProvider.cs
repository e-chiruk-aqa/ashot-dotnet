namespace AShotNet.Coordinates
{
    using OpenQA.Selenium;
    using Util;

    /// <author>pazone</author>
	[System.Serializable]
	public class JqueryCoordsProvider : CoordsProvider
	{
		public override Coords ofElement(IWebDriver driver, IWebElement element)
		{
			return JsCoords.findCoordsWithJquery(driver, element);
		}
	}
}
