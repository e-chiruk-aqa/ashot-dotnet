namespace AShotNet
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using Coordinates;
    using Cropper;
    using Extentions;
    using OpenQA.Selenium;
    using ScreenTaker;

    /// <author>
    ///     <a href="ozozulenko@gmail.com">Oleksii Zozulenko</a>
    /// </author>
    [Serializable]
    public class AShot
    {
        private CoordsProvider coordsProvider = new JqueryCoordsProvider();

        private ImageCropper cropper = new DefaultCropper();

        private ICollection<Coords> ignoredAreas = new HashSet<Coords>();
        private ICollection<By> ignoredLocators = new HashSet<By>();

        private ScreenTaker.ScreenTaker taker = new ScreenTaker.ScreenTaker();

        public virtual AShot CoordsProvider(CoordsProvider coordsProvider)
        {
            this.coordsProvider = coordsProvider;
            return this;
        }

        public virtual AShot ImageCropper(ImageCropper cropper)
        {
            this.cropper = cropper;
            return this;
        }

        /// <summary>Sets taker impl.</summary>
        /// <remarks>
        ///     Sets taker impl.
        ///     Usually is not used.
        /// </remarks>
        /// <param name="taker">ScreenTaker</param>
        /// <returns>this;</returns>
        /// <seealso cref="AShotNet.ScreenTaker.ScreenTaker" />
        public virtual AShot ScreenTaker(ScreenTaker.ScreenTaker taker)
        {
            this.taker = taker;
            return this;
        }

        /// <summary>Sets the list of locators to ignore during image comparison.</summary>
        /// <param name="ignoredElements">list of By</param>
        /// <returns>this</returns>
        public virtual AShot IgnoredElements(ICollection<By> ignoredElements)
        {
            lock (this)
            {
                this.ignoredLocators = ignoredElements;
                return this;
            }
        }

        /// <summary>Adds selector of ignored element.</summary>
        /// <param name="selector">By</param>
        /// <returns>this</returns>
        public virtual AShot AddIgnoredElement(By selector)
        {
            lock (this)
            {
                this.ignoredLocators.Add(selector);
                return this;
            }
        }

        /// <summary>Sets a collection of wittingly ignored coords.</summary>
        /// <param name="ignoredAreas">Set of ignored areas</param>
        /// <returns>aShot</returns>
        public virtual AShot IgnoredAreas(ICollection<Coords> ignoredAreas)
        {
            lock (this)
            {
                this.ignoredAreas = ignoredAreas;
                return this;
            }
        }

        /// <summary>Adds coordinated to set of wittingly ignored coords.</summary>
        /// <param name="area">coords of wittingly ignored coords</param>
        /// <returns>aShot;</returns>
        public virtual AShot AddIgnoredArea(Coords area)
        {
            lock (this)
            {
                this.ignoredAreas.Add(area);
                return this;
            }
        }

        /// <summary>Sets the policy of taking screenshot.</summary>
        /// <param name="strategy">shooting strategy</param>
        /// <returns>this</returns>
        /// <seealso cref="AShotNet.ScreenTaker.ShootingStrategy" />
        public virtual AShot ShootingStrategy(ShootingStrategy strategy)
        {
            this.taker.withShootingStrategy(strategy);
            return this;
        }

        /// <summary>Sets device pixel ratio.</summary>
        /// <remarks>
        ///     Sets device pixel ratio.
        ///     for example, Retina = 2.
        /// </remarks>
        /// <param name="dpr">device pixel ratio</param>
        /// <returns>this</returns>
        public virtual AShot Dpr(int dpr)
        {
            this.taker.withDpr(dpr);
            return this;
        }

        /// <summary>
        ///     Takes the screenshot of given elements
        ///     If elements were not found screenshot of whole page will be returned
        /// </summary>
        /// <param name="driver">WebDriver instance</param>
        /// <returns>Screenshot with cropped image and list of ignored areas on screenshot</returns>
        /// <exception cref="System.Exception">when something goes wrong</exception>
        /// <seealso cref="AShotNet.Screenshot" />
        public virtual Screenshot TakeScreenshot(IWebDriver driver, ICollection<IWebElement> elements)
        {
            ICollection<Coords> elementCoords = this.coordsProvider.ofElements(driver, elements);
            Bitmap shot = this.taker.take(driver);
            Screenshot screenshot = this.cropper.crop(shot, elementCoords);
            ICollection<Coords> ignoredAreas = this.CompileIgnoredAreas(driver, CoordsPreparationStrategy.intersectingWith(screenshot));
            screenshot.setIgnoredAreas(ignoredAreas);
            return screenshot;
        }

        /// <summary>Takes the screenshot of given element</summary>
        /// <param name="driver">WebDriver instance</param>
        /// <returns>Screenshot with cropped image and list of ignored areas on screenshot</returns>
        /// <exception cref="System.Exception">when something goes wrong</exception>
        /// <seealso cref="AShotNet.Screenshot" />
        public virtual Screenshot TakeScreenshot(IWebDriver driver, IWebElement element)
        {
            return this.TakeScreenshot(driver, new Collection<IWebElement> {element});
        }

        /// <summary>Takes the screenshot of whole page</summary>
        /// <param name="driver">WebDriver instance</param>
        /// <returns>
        ///     Screenshot with whole page image and list of ignored areas on screenshot
        /// </returns>
        /// <seealso cref="AShotNet.Screenshot" />
        public virtual Screenshot TakeScreenshot(IWebDriver driver)
        {
            var screenshot = new Screenshot(this.taker.take(driver));
            screenshot.setIgnoredAreas(this.CompileIgnoredAreas(driver, CoordsPreparationStrategy.simple()));
            return screenshot;
        }

        protected internal virtual ICollection<Coords> CompileIgnoredAreas(IWebDriver driver, CoordsPreparationStrategy
            preparationStrategy)
        {
            lock (this)
            {
                ICollection<Coords> ignoredCoords = new HashSet<Coords>();
                foreach (By ignoredLocator in this.ignoredLocators)
                {
                    IList<IWebElement> ignoredElements = driver.FindElements(ignoredLocator);
                    if (!ignoredElements.IsEmpty())
                    {
                        ignoredCoords.AddAll(preparationStrategy.prepare(this.coordsProvider.ofElements(driver, ignoredElements.AsEnumerable())));
                    }
                }
                foreach (Coords ignoredArea in this.ignoredAreas)
                {
                    ignoredCoords.AddAll(preparationStrategy.prepare(new List<Coords> {ignoredArea}));
                }
                return ignoredCoords;
            }
        }

        public virtual ICollection<By> GetIgnoredLocators()
        {
            lock (this)
            {
                return this.ignoredLocators;
            }
        }
    }
}
