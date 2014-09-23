namespace DrawingDemos
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using DemoCore;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.Demos = this.GetDemos(this.GetType().Assembly).OrderBy(e => e.Title).ToArray();
        }

        /// <summary>
        /// Gets the demos.
        /// </summary>
        /// <value>The demos.</value>
        public IList<Demo> Demos { get; private set; }

        /// <summary>
        /// Creates a thumbnail of the specified window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="width">The width of the thumbnail.</param>
        /// <param name="path">The output path.</param>
        private static void CreateThumbnail(Window window, int width, string path)
        {
            var bitmap = ScreenCapture.Capture(
                (int)window.Left,
                (int)window.Top,
                (int)window.ActualWidth,
                (int)window.ActualHeight);
            var newHeight = width * bitmap.Height / bitmap.Width;
            var resizedBitmap = BitmapTools.Resize(bitmap, width, newHeight);
            resizedBitmap.Save(path);
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the ListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the event data.</param>
        private void ListBoxMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            var lb = (ListBox)sender;
            var example = lb.SelectedItem as Demo;
            if (example != null)
            {
                var window = example.Create();
                window.Icon = this.Icon;
                window.Show();

                if (example.Thumbnail == null)
                {
                }

                window.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.F12)
                    {
                        CreateThumbnail(window, 120, System.IO.Path.Combine(@"..\..\Images\", example.ThumbnailFileName));
                        MessageBox.Show(window, "Demo image updated.");
                        e.Handled = true;
                    }
                };
            }
        }

        /// <summary>
        /// Gets the examples in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        /// <returns>A sequence of demos.</returns>
        private IEnumerable<Demo> GetDemos(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var ea = type.GetCustomAttributes(typeof(DemoAttribute), false).FirstOrDefault() as DemoAttribute;
                if (ea != null)
                {
                    yield return new Demo(type, ea.Title, ea.Description);
                }
            }
        }
    }
}
