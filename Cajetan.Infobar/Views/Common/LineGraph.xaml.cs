using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cajetan.Infobar.Views
{
    /// <summary>
    /// Interaction logic for LineGraph.xaml
    /// </summary>
    public partial class LineGraph : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(nameof(LineThickness), typeof(double), typeof(LineGraph), new PropertyMetadata(1.0));
        public static readonly DependencyProperty TopMarginProperty = DependencyProperty.Register(nameof(TopMargin), typeof(int), typeof(LineGraph), new PropertyMetadata(2));
        public static readonly DependencyProperty BottomMarginProperty = DependencyProperty.Register(nameof(BottomMargin), typeof(int), typeof(LineGraph), new PropertyMetadata(1));

        public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(nameof(Values), typeof(ObservableCollection<int>), typeof(LineGraph), new PropertyMetadata((o, e) => { ((LineGraph)o).ValuesChanged(); }));


        public double LineThickness
        {
            get { return (double)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }

        public int TopMargin
        {
            get { return (int)GetValue(TopMarginProperty); }
            set { SetValue(TopMarginProperty, value); }
        }

        public int BottomMargin
        {
            get { return (int)GetValue(BottomMarginProperty); }
            set { SetValue(BottomMarginProperty, value); }
        }

        public ObservableCollection<int> Values
        {
            get { return (ObservableCollection<int>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        #endregion


        #region Private Fields

        private int _width = 0;
        private int _height = 0;

        private readonly List<int> _values;

        #endregion


        #region Constructor

        public LineGraph()
        {
            InitializeComponent();
            _values = new List<int>();
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Add a number to the list of values. If number of items exceed maximum number of allowed items the oldest is removed.
        /// </summary>
        /// <param name="val">An integer between 0 and 100.</param>
        private void AddValue(int val)
        {
            if (_width > 0)
            {
                if (val < 0)
                    val = 0;
                if (val > 100)
                    val = 100;

                if (_values.Count >= _width / 2)
                    _values.RemoveAt(0);
                _values.Add(val);

                UpdateGraph();
            }
        }

        private void UpdateGraph()
        {
            List<Point> p = new List<Point>();
            List<Point> t = new List<Point>();

            int startX = _width + TopMargin;
            int startY = _height - BottomMargin;

            int n = _values.Count * 2;
            foreach (int v in _values)
            {
                int x = startX - n;
                int y = startY - MapValueToRange(v, 1, startY - BottomMargin);
                t.Add(new Point(x, y));
                n -= 2;
            }

            if (t.Count > 0)
            {
                double v = t[0].Y;
                for (int i = 0; i < _width - (_values.Count * 2); i += 2)
                {
                    p.Add(new Point(i, v));
                }
                p.AddRange(t);
            }

            line.Points = new PointCollection(p);
        }

        private static int MapValueToRange(int value, int heightMin, int heightMax)
        {
            double map = (value - 0.0) / (100.0 - 0.0) * (heightMax - heightMin) + heightMin;
            int round = Convert.ToInt32(Math.Round(map));

            return round;
        }

        private void ValuesChanged()
        {
            if (Values is null) return;

            Values.CollectionChanged += (s, e) =>
            {
                foreach (object i in e.NewItems)
                    Application.Current.Dispatcher.Invoke(() => AddValue((int)i));
            };
        }

        /// <summary>
        /// EventHandler for Loaded event. Sets width and height.
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _width = Convert.ToInt32(ActualWidth);
            _height = Convert.ToInt32(ActualHeight);
        }

        /// <summary>
        /// EventHandler for SizeChanged event. Updates width and height variables with new values.
        /// </summary>
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _width = Convert.ToInt32(ActualWidth);
            _height = Convert.ToInt32(ActualHeight);
        }

        #endregion
    }
}
