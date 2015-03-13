namespace AShotNet.Util
{
    using System;
    using System.IO;
    using System.Threading;
    using AShotNet.Coordinates;
    using Extentions;
    using OpenQA.Selenium;

    /// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	public class JsCoords
	{
		public const string COORDS_JS_PATH = "js/coords-single.js";

		public static Coords findCoordsWithJquery(IWebDriver driver, IWebElement element)
		{
			try
			{
				string script = org.apache.commons.io.IOUtils.toString(Thread.CurrentThread.getContextClassLoader().getResourceAsStream(COORDS_JS_PATH));
				System.Collections.IList result = (System.Collections.IList)((IJavaScriptExecutor
					)driver).ExecuteScript(script, element);
				if (result.IsEmpty())
				{
					throw new System.Exception("Unable to find coordinates with jQuery.");
				}
				return new com.google.gson.Gson().fromJson<Coords
					>((string)result[0]);
			}
			catch (IOException e)
			{
				throw new Exception(e.Message,e);
			}
		}
	}
}
