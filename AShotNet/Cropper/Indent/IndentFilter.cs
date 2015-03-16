namespace AShotNet.Cropper.Indent
{
    using System;
    using System.Drawing;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public abstract class IndentFilter
    {
        public abstract Bitmap apply(Bitmap image);
    }
}
