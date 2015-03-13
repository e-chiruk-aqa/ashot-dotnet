namespace AShotNet.Cropper.Indent
{
    using System.Drawing;

    /// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	[System.Serializable]
	public class MonochromeFilter : IndentFilter
	{
		public override Bitmap apply(Bitmap image
			)
		{
			return this.darken(image);
		}

    private Bitmap darken(Bitmap image)
		{
			return this.toBufferedImage(javax.swing.GrayFilter.createDisabledImage(image));
		}

    private Bitmap toBufferedImage(Image img)
		{
        if (img is Bitmap)
			{
				return (java.awt.image.BufferedImage)img;
			}
			java.awt.image.BufferedImage bufferedImage = new java.awt.image.BufferedImage(img
				.getWidth(null), img.getHeight(null), java.awt.image.BufferedImage.TYPE_INT_ARGB
				);
			java.awt.Graphics2D graphics = bufferedImage.createGraphics();
			graphics.drawImage(img, 0, 0, null);
			graphics.dispose();
			return bufferedImage;
		}
	}
}
