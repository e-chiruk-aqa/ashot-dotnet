namespace AShotNet.Coordinates
{
    using System.Collections.Generic;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    public abstract class CoordsPreparationStrategy
    {
        public static CoordsPreparationStrategy simple()
        {
            return new _CoordsPreparationStrategy_19();
        }

        public static CoordsPreparationStrategy intersectingWith(Screenshot screenshot)
        {
            return new _CoordsPreparationStrategy_28(screenshot);
        }

        public abstract ICollection<Coords> prepare(ICollection<Coords> coordinates);

        private sealed class _CoordsPreparationStrategy_19 : CoordsPreparationStrategy
        {
            public override ICollection<Coords> prepare(ICollection<Coords> coordinates)
            {
                return new HashSet<Coords>(coordinates);
            }
        }

        private sealed class _CoordsPreparationStrategy_28 : CoordsPreparationStrategy
        {
            private readonly Screenshot screenshot;

            public _CoordsPreparationStrategy_28(Screenshot screenshot)
            {
                this.screenshot = screenshot;
            }

            public override ICollection<Coords> prepare(ICollection<Coords> coordinates)
            {
                return Coords.Intersect(this.screenshot.getCoordsToCompare(), Coords.setReferenceCoords(this.screenshot.getOriginShift(), new HashSet<Coords>(coordinates)));
            }
        }
    }
}
