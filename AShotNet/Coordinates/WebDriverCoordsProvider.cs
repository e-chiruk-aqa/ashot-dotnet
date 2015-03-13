namespace AShotNet.Coordinates
{
    using System;
    using System.Drawing;
    using OpenQA.Selenium;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public class WebDriverCoordsProvider : CoordsProvider
    {
        public override Coords ofElement(IWebDriver driver, IWebElement element)
        {
            Point point = element.Location;
            Size dimension = element.Size;
            return new Coords(point.X, point.Y, dimension.Width, dimension.Height);
        }
    }
}
