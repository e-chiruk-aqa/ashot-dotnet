namespace AShotNet.Coordinates
{
    using System;
    using System.Collections.Generic;
    using OpenQA.Selenium;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public abstract class CoordsProvider
    {
        public abstract Coords ofElement(IWebDriver driver, IWebElement element);

        public virtual ICollection<Coords> ofElements(IWebDriver driver, IEnumerable<IWebElement> elements)
        {
            ICollection<Coords> elementsCoords = new HashSet<Coords>();
            foreach (IWebElement element in elements)
            {
                Coords elementCoords = this.ofElement(driver, element);
                if (!elementCoords.IsEmpty)
                {
                    elementsCoords.Add(elementCoords);
                }
            }
            return elementsCoords;
        }

        public virtual ICollection<Coords> ofElements(IWebDriver driver, params IWebElement[] elements)
        {
            return this.ofElements(driver, elements);
        }

        public virtual ICollection<Coords> locatedBy(IWebDriver driver, By locator)
        {
            return ofElements(driver, driver.FindElements(locator));
        }
    }
}
