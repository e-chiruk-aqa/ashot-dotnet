namespace AShotNet.Test
{
    using System.Collections.Generic;
    using Coordinates;
    using Cropper;
    using Cropper.Indent;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Util;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [TestClass]
    public class CroppersTest
    {
        public static readonly ICollection<Coords> OUTSIDE_IMAGE = new _HashSet_23();

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void TestElementOutsideImageDefCropper()
        {
            Screenshot screenshot = new DefaultCropper().cropScreenshot(DifferTest.IMAGE_A_SMALL, new HashSet<Coords>(OUTSIDE_IMAGE));
            var matcher = ImageTool.equalImage(DifferTest.loadImage("img/expected/outside_dc.png"));
            Assert.IsTrue(matcher.Matches(screenshot.getImage()));
        }

        /// <exception cref="System.Exception" />
        [TestMethod]
        [DeploymentItem("img", "./img/")]
        public virtual void TestElementOutsideImageIndentCropper()
        {
            Screenshot screenshot = new IndentCropper(10).cropScreenshot(DifferTest.IMAGE_A_SMALL, new HashSet<Coords>(OUTSIDE_IMAGE));
            var matcher = ImageTool.equalImage(DifferTest.loadImage("img/expected/outside_ic.png"));
            Assert.IsTrue(matcher.Matches(screenshot.getImage()) );
        }

        private sealed class _HashSet_23 : HashSet<Coords>
        {
            public _HashSet_23()
            {
                {
                    this.Add(new Coords(20, 20, 200, 90));
                }
            }
        }
    }
}
