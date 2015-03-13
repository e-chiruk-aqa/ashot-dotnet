namespace AShotNet.Cropper.Indent
{
    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    public class IndentFilerFactory
    {
        public static IndentFilter blur()
        {
            return new BlurFilter();
        }

        public static IndentFilter monochrome()
        {
            return new MonochromeFilter();
        }
    }
}
