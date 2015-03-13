using Sharpen;

namespace ru.yandex.qatools.elementscompare.tests
{
	/// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	public class CroppersTest
	{
		private sealed class _HashSet_23 : java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords
			>
		{
			public _HashSet_23()
			{
				{
					this.add(new ru.yandex.qatools.ashot.coordinates.Coords(20, 20, 200, 90));
				}
			}
		}

		public static readonly System.Collections.Generic.ICollection<ru.yandex.qatools.ashot.coordinates.Coords
			> OUTSIDE_IMAGE = new _HashSet_23();

		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void testElementOutsideImageDefCropper()
		{
			ru.yandex.qatools.ashot.Screenshot screenshot = new ru.yandex.qatools.ashot.cropper.DefaultCropper
				().cropScreenshot(ru.yandex.qatools.elementscompare.tests.DifferTest.IMAGE_A_SMALL
				, new java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords>(OUTSIDE_IMAGE
				));
			NUnit.Framework.Assert.assertThat(screenshot.getImage(), ru.yandex.qatools.ashot.util.ImageTool
				.equalImage(ru.yandex.qatools.elementscompare.tests.DifferTest.loadImage("img/expected/outside_dc.png"
				)));
		}

		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void testElementOutsideImageIndentCropper()
		{
			ru.yandex.qatools.ashot.Screenshot screenshot = new ru.yandex.qatools.ashot.cropper.indent.IndentCropper
				(10).cropScreenshot(ru.yandex.qatools.elementscompare.tests.DifferTest.IMAGE_A_SMALL
				, new java.util.HashSet<ru.yandex.qatools.ashot.coordinates.Coords>(OUTSIDE_IMAGE
				));
			NUnit.Framework.Assert.assertThat(screenshot.getImage(), ru.yandex.qatools.ashot.util.ImageTool
				.equalImage(ru.yandex.qatools.elementscompare.tests.DifferTest.loadImage("img/expected/outside_ic.png"
				)));
		}
	}
}
