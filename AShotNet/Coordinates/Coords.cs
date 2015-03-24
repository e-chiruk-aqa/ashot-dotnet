namespace AShotNet.Coordinates
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <author>
    ///     <a href="pazone@yandex-team.ru">Pavel Zorin</a>
    /// </author>
    [Serializable]
    public class Coords
    {
        private Rectangle _rectangle;

        public Coords(Rectangle rectangle)
        {
            this._rectangle = rectangle;
        }

        public Coords(int x, int y, int width, int height)
        {
            this._rectangle = new Rectangle(x, y, width, height);
        }


        public bool IsEmpty { get { return this._rectangle.IsEmpty; } }

        public int Height { get { return this._rectangle.Height; } set { this._rectangle.Height = value; } }

        public int Width { get { return this._rectangle.Width; } set { this._rectangle.Width = value; } }

        public int Y { get { return this._rectangle.Y; } set { this._rectangle.Y = value; } }

        public int X { get { return this._rectangle.X; } set { this._rectangle.X = value; } }
        public Rectangle Rectangle { get { return this._rectangle; } set { this._rectangle = value; } }

        public static ICollection<Coords> Intersect(ICollection<Coords> coordsPool1, ICollection<Coords> coordsPool2)
        {
            ICollection<Coords> intersectedCoords = new HashSet<Coords>();
            foreach (Coords coords1 in coordsPool1)
            {
                foreach (Coords coords2 in coordsPool2)
                {
                    Coords intersection = coords1.Intersect(coords2);
                    if (!intersection.IsEmpty)
                    {
                        intersectedCoords.Add(intersection);
                    }
                }
            }
            return intersectedCoords;
        }

        public Coords Intersect(Coords coords2)
        {
            this._rectangle.Intersect(coords2._rectangle);

            return this;
        }

        public static ICollection<Coords> setReferenceCoords(Coords reference, ICollection<Coords> coordsSet)
        {
            ICollection<Coords> referencedCoords = new HashSet<Coords>();
            foreach (Coords coords in coordsSet)
            {
                referencedCoords.Add(new Coords(coords.X - reference.X, coords.Y - reference.Y, coords.Width, coords.Height));
            }
            return referencedCoords;
        }

        public static Coords unity(ICollection<Coords> coordsCollection)
        {
            Coords unity = coordsCollection.First();
            foreach (Coords coords in coordsCollection)
            {
                unity = unity.Union(coords);
            }
            return unity;
        }

        public static Coords ofImage(Bitmap image)
        {
            return new Coords(0, 0, image.Width, image.Height);
        }


        public virtual void ReduceBy(int pixels)
        {
            if (pixels < this.Width/2 && pixels < this.Height/2)
            {
                this.X += pixels;
                this.Y += pixels;
                this.Width -= pixels;
                this.Height -= pixels;
            }
        }

        public Coords Union(Coords r)
        {
            return new Coords(Rectangle.Union(this._rectangle, r._rectangle));
        }


        /*public override string ToString()
		{
			return new com.google.gson.Gson().toJson(this);
		}*/

        public bool IntersectsWith(Coords coords)
        {
            return this._rectangle.IntersectsWith(coords._rectangle);
        }

        public bool Contains(Point point)
        {
            return this._rectangle.Contains(point);
        }

        public override bool Equals(object obj)
        {
            if (! (obj is Coords))
            {
                return false;
            }
            
            var r = (Coords) obj;
            return r.X == this.X && r.Y == this.Y && r.Width == this.Width && r.Height == this.Height;
        }
    }
}
