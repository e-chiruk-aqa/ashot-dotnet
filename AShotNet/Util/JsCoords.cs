namespace AShotNet.Util
{
    using System;
    using System.IO;
    using System.Reflection;
    using Coordinates;
    using Newtonsoft.Json.Linq;
    using OpenQA.Selenium;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    public class JsCoords
    {
        public static Coords findCoordsWithJquery(IWebDriver driver, IWebElement element)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = "Resources.js.coords-single.js";
                var resourceFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "coords-single.js"));

                string result;

                using (Stream stream = resourceFile.OpenRead())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                var jsResult = ((IJavaScriptExecutor) driver).ExecuteScript(result, element);
                JObject jsonObj = JObject.Parse(jsResult.ToString());
                var x = jsonObj["x"].Value<int>();
                var y = jsonObj["y"].Value<int>();
                var w = jsonObj["width"].Value<int>();
                var h = jsonObj["height"].Value<int>();

                return new Coords(x, y, w, h);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
