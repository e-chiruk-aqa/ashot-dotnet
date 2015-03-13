namespace AShotNet.Cropper.Indent
{
    using System.Drawing;

    /// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	[System.Serializable]
	public class BlurFilter : IndentFilter
	{
		public override Bitmap apply(Bitmap image
			)
		{
			java.awt.image.Kernel kernel = new java.awt.image.Kernel(3, 3, new float[] { 1f /
				 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f });
			java.awt.image.BufferedImageOp blurOp = new java.awt.image.ConvolveOp(kernel, java.awt.image.ConvolveOp
				.EDGE_NO_OP, null);
			return blurOp.filter(image, null);
		}
	}
}
