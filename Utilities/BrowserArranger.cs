

namespace HadesFormCommon.Utilities
{
    public class BrowserArranger
    {
        private static readonly object mutex = new();

        private readonly Dictionary<string, BrowserLocation> browserLocationDict = new();
        private readonly List<BrowserLocation> browserLocations = new();

        private const int MarginLeft = -10;
        private const int MinHeight = 900;

        private int maxHeight;
        private int maxWidth;
        private int currentChromeIndex;

        private readonly int total;
        private readonly int xCount;
        private readonly int yCount;
        public BrowserArranger(int total, int xCount, int yCount)
        {
            this.total = total;
            this.xCount = xCount;
            this.yCount = yCount;
            Init();
            browserLocations.AddRange(browserLocationDict.Select(x => x.Value).ToList());
        }
        private void Init()
        {
            Rectangle workingArea = GetWorkingArea();
            Size workingSize = new(workingArea.Width, workingArea.Height);
            maxWidth = workingArea.Width / xCount;
            maxHeight = workingArea.Height / yCount;
            if (maxHeight < MinHeight)
            {
                maxHeight = MinHeight;
            }
            Point point = new(workingArea.X, workingArea.Y);
            for (int i = 0; i < total; i++)
            {
                BrowserLocation chromePoint = new()
                {
                    Size = new Size(maxWidth, maxHeight),
                    Point = point
                };
                browserLocationDict[chromePoint.Id] = chromePoint;
                point = NextPoint(point, workingSize, maxWidth, maxHeight, workingArea.X);
            }
        }
        private static Point NextPoint(Point currentPoint, Size workingSize, int maxWidth, int maxHeight, int marginFirstX)
        {
            Point point = new();
            int w = currentPoint.X + maxWidth;
            if (w < workingSize.Width)
            {
                point.X = currentPoint.X + maxWidth + MarginLeft;
                point.Y = currentPoint.Y;
            }
            else
            {
                if ((currentPoint.Y + maxHeight) < workingSize.Height)
                {
                    point.Y = currentPoint.Y + maxHeight;
                    point.X = marginFirstX;
                }
                else
                {
                    point.X = marginFirstX;
                    point.Y = 0;
                }
            }
            return point;
        }
        private static Rectangle GetWorkingArea()
        {
            return Screen.PrimaryScreen.WorkingArea;
        }


        public BrowserLocation GetBrowserLocation()
        {
            lock (mutex)
            {
                if (currentChromeIndex == browserLocations.Count)
                {
                    currentChromeIndex = 0;
                }
                return browserLocations[currentChromeIndex++];
            }
        }

    }

    public class BrowserLocation
    {
        public BrowserLocation()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; }
        public Point Point { get; set; }
        public Size Size { get; set; }
    }
}
