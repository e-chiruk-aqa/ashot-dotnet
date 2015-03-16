namespace AShotNet.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SBM.Common.Extentions;

	/// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
 [TestClass]
	public class CroppersTest
	{
		private sealed class _HashSet_23 : HashSet<Coords>
		{
			public _HashSet_23()
			{
				{
					this.add(new Coords(20, 20, 200, 90));
				}
			}
		}

		public static readonly System.Collections.Generic.ICollection<Coords> OUTSIDE_IMAGE = new _HashSet_23();

		/// <exception cref="System.Exception"/>
     [TestMethod]
		public virtual void TestElementOutsideImageDefCropper()
		{
		Screenshot screenshot = new DefaultCropper
				().cropScreenshot(DifferTest.IMAGE_A_SMALL
				, new HashSet<Coords>(OUTSIDE_IMAGE
				));
Assert.assertThat(screenshot.getImage(), ImageTool
				.equalImage(DifferTest.loadImage("img/expected/outside_dc.png")));
		}

		/// <exception cref="System.Exception"/>
     [TestMethod]
		public virtual void TestElementOutsideImageIndentCropper()
		{
			ru.yandex.qatools.ashot.Screenshot screenshot = new ru.yandex.qatools.ashot.cropper.indent.IndentCropper
				(10).cropScreenshot(DifferTest.IMAGE_A_SMALL
				, new HashSet<Coords>(OUTSIDE_IMAGE
				));
			Assert.assertThat(screenshot.getImage(), ImageTool
				.equalImage(DifferTest.loadImage("img/expected/outside_ic.png"
				)));
		}
	}
}
