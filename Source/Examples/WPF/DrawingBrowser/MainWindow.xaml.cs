namespace DrawingDemo
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    using DrawingDemo.Annotations;

    using OxyPlot.Drawing;

    using SvgLibrary;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private ExampleInfo selectedExample;

        public ICommand SavePngCommand { get; private set; }
        public ICommand SavePdfCommand { get; private set; }
        public ICommand SaveSvgCommand { get; private set; }
        public DrawingModel Drawing { get; private set; }

        public IList<ExampleInfo> Examples { get; private set; }

        public ExampleInfo SelectedExample
        {
            get
            {
                return this.selectedExample;
            }

            set
            {
                this.selectedExample = value;
                this.OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            var svg = new Svg() { ViewBox = "0 0 900 900" };
            var g = new SvgGroup() { Id = "g1" };
            g.Elements.Add(new SvgPath() { Id = "p1" });
            g.Elements.Add(new SvgPath() { Id = "p2" });
            svg.Elements.Add(g);
            using (var s = File.Create(@"D:\test.svg"))
                svg.Save(s);
            this.SavePngCommand = new DelegateCommand(this.SavePng);
            this.SavePdfCommand = new DelegateCommand(this.SavePdf);
            this.SaveSvgCommand = new DelegateCommand(this.SaveSvg);

            this.Examples = new ObservableCollection<ExampleInfo>(DrawingDemo.Examples.Get());
            this.Drawing = new DrawingModel();

            /*            Drawing.Background = OxyColors.Orange;
                        this.Drawing.Add(new Rectangle(-40, -30, 40, 30) { Thickness = 0.5 });
                        this.Drawing.Add(new Text(-39, 29, "Drawing example") { FontSize = 2 });
                        this.Drawing.Add(new Text(-39, 25, "Fixed font size") { FontSize = -11 });

                        this.Drawing.Add(new Ellipse(0, 0, 30, 20));
                        this.Drawing.Add(new Ellipse(0, 0, 20, 15) { Thickness = 1, Fill = OxyColors.LightGreen });
                        this.Drawing.Add(new Rectangle(-30, -20, 30, 20));

                        var polyline = new Polyline();
                        for (double r = 0; r < 360; r += 15)
                            this.Drawing.Add(new Text(0, 0, "Drawing") { Rotate = r, FontSize = 3, Color = OxyColors.White });

                        var lines = new Lines() { Thickness = -4, Color = OxyColors.IndianRed };
                        lines.Add(0, -10, 0, 10);
                        lines.Add(-15, 0, 15, 0);
                        this.Drawing.Add(lines);

                        var imageSource = new OxyImage(File.ReadAllBytes("test.png"));
                        for (double r = 0; r < 360; r += 90)
                        {
                            this.Drawing.Add(
                                new Image
                                    {
                                        X = 0,
                                        Y = 0,
                                        Width = -32,
                                        Height = -32,
                                        //  Rotation = r,
                                        Source = imageSource,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                        VerticalAlignment = VerticalAlignment.Top
                                    });
                        }

                        this.Drawing.Add(new Image
                        {
                            X = 20,
                            Y = 10,
                            Width = 4,
                            Height = 4,
                            SourceX = 6,
                            SourceY = 6,
                            SourceWidth = 20,
                            SourceHeight = 20,
                            Source = imageSource
                        });*/
        }

        private void SavePng()
        {
            //  PngExporter.Export(this.Drawing, "test.png", 100, 100);
        }

        private void SavePdf()
        {
        }

        private void SaveSvg()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}