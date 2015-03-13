namespace AShotNet.Coordinates
{
    using System.Collections.Generic;
    using System.Drawing;

    /// <author><a href="pazone@yandex-team.ru">Pavel Zorin</a></author>
	[System.Serializable]
	public class Coords : Rectangle
	{
		public static System.Collections.Generic.ICollection<Coords
			> intersection(System.Collections.Generic.ICollection<Coords
			> coordsPool1, System.Collections.Generic.ICollection<Coords
			> coordsPool2)
		{
			System.Collections.Generic.ICollection<Coords
				> intersectedCoords = new HashSet<Coords
				    >();
			foreach (Coords coords1 in coordsPool1)
			{
				foreach (Coords coords2 in coordsPool2)
				{
					Coords intersection = ((Coords
						)coords1.intersection(coords2));
					if (!intersection.IsEmpty())
					{
						intersectedCoords.Add(intersection);
					}
				}
			}
			return intersectedCoords;
		}

		public static System.Collections.Generic.ICollection<Coords
			> setReferenceCoords(Coords reference, System.Collections.Generic.ICollection
			<Coords> coordsSet)
		{
			System.Collections.Generic.ICollection<Coords
				> referencedCoords = new HashSet<Coords
				>();
			foreach (Coords coords in coordsSet)
			{
				referencedCoords.Add(new Coords(coords.x - reference
					.x, coords.y - reference.y, coords.width, coords.height));
			}
			return referencedCoords;
		}

		public static Coords unity(System.Collections.Generic.ICollection
			<Coords> coordsCollection)
		{
			Coords unity = coordsCollection.GetEnumerator
				().Current;
			foreach (Coords coords in coordsCollection)
			{
				unity = ((Coords)unity.union(coords));
			}
			return unity;
		}

		public static Coords ofImage(Bitmap image)
		{
			return new Coords(image.Width, image.Height);
		}

		public Coords(Rectangle rectangle) : base(rectangle)
		{
		}

		public Coords(int x, int y, int width, int height)
			: base(x, y, width, height)
		{
		}

		public Coords(int width, int height)
			: base(width, height)
		{
		}

		public virtual void reduceBy(int pixels)
		{
			if (pixels < Width / 2 && pixels < Height / 2)
			{
				this.x += pixels;
				this.y += pixels;
				this.width -= pixels;
				this.height -= pixels;
			}
		}

		public override Rectangle union(Rectangle r)
		{
			return new Coords(base.Union(r));
		}

		public override Rectangle intersection(Rectangle r)
		{
			return new Coords(base.intersection(r));
		}

		public override string ToString()
		{
			return new com.google.gson.Gson().toJson(this);
		}
	}
}
