
namespace AShotNet.Test
{
	/// <author><a href="eoff@yandex-team.ru">Maksim Mukosey</a></author>
   [TestClass]
    public class SerializeScreenshotTest
	{
		private static readonly Bitmap IMAGE_A_SMALL = DifferTest.loadImage("img/A_s.png");

		private sealed class _HashSet_32 : HashSet<ru.yandex.qatools.ashot.coordinates.Coords
			>
		{
			public _HashSet_32()
			{
				{
					this.add(new Coords(20, 20, 200, 90));
				}
			}
		}

		public static readonly System.Collections.Generic.ICollection<Coords			> IGNORED_AREAS = new _HashSet_32();

		private File serializedFile;

		/// <exception cref="System.IO.IOException"/>
      [TestInitialize]
		public virtual void setUp()
		{
			serializedFile = File.createTempFile("serialized", "screenshot");
		}

     [TestCleanup]
		public virtual void tearDown()
		{
			serializedFile.delete();
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="java.lang.ClassNotFoundException"/>
  [TestMethod]
		public virtual void serializeWithIgnoredAreas()
		{
			ru.yandex.qatools.ashot.Screenshot screenshot = new ru.yandex.qatools.ashot.Screenshot
				(IMAGE_A_SMALL);
			screenshot.setIgnoredAreas(IGNORED_AREAS);
			using (java.io.ObjectOutputStream objectOutputStream = new java.io.ObjectOutputStream
				(new java.io.FileOutputStream(serializedFile)))
			{
				objectOutputStream.writeObject(screenshot);
			}
			using (java.io.ObjectInputStream objectInputStream = new java.io.ObjectInputStream
				(new java.io.FileInputStream(serializedFile)))
			{
				ru.yandex.qatools.ashot.Screenshot deserialized = (ru.yandex.qatools.ashot.Screenshot
					)objectInputStream.readObject();
				checkDeserializedScreenshot(screenshot, deserialized);
			}
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="java.lang.ClassNotFoundException"/>
  [TestMethod]
		public virtual void serializeWithoutIgnoredAreas()
		{
			ru.yandex.qatools.ashot.Screenshot screenshot = new ru.yandex.qatools.ashot.Screenshot
				(IMAGE_A_SMALL);
			using (java.io.ObjectOutputStream objectOutputStream = new java.io.ObjectOutputStream
				(new java.io.FileOutputStream(serializedFile)))
			{
				objectOutputStream.writeObject(screenshot);
			}
			using (java.io.ObjectInputStream objectInputStream = new java.io.ObjectInputStream
				(new java.io.FileInputStream(serializedFile)))
			{
				ru.yandex.qatools.ashot.Screenshot deserialized = (ru.yandex.qatools.ashot.Screenshot
					)objectInputStream.readObject();
				checkDeserializedScreenshot(screenshot, deserialized);
			}
		}

		private void checkDeserializedScreenshot(ru.yandex.qatools.ashot.Screenshot expected
			, ru.yandex.qatools.ashot.Screenshot got)
		{
			org.hamcrest.MatcherAssert.assertThat(got.getCoordsToCompare(), org.hamcrest.Matchers.equalTo
				(expected.getCoordsToCompare()));
			org.hamcrest.MatcherAssert.assertThat(got.getIgnoredAreas(), org.hamcrest.Matchers.equalTo
				(expected.getIgnoredAreas()));
			org.hamcrest.MatcherAssert.assertThat(got.getImage(), ru.yandex.qatools.ashot.util.ImageTool.equalImage
				(expected.getImage()));
		}
	}
}
