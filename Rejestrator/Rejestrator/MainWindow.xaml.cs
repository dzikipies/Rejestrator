using LiveCharts;
using Rejestrator.Logic;
using System.Windows;
using System.Windows.Controls;

namespace Rejestrator
{
    public partial class MainWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        private readonly FileLoader FileLoader;
        public string NameX { get; set; }
        public string NameY { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection(); 
            FileLoader = new FileLoader();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            FileLoader.RemoveFile();
            plotList.Items.Clear();
            SeriesCollection.Clear();
            DataContext = null;
    
            FileLoader.PreparationFile();
            SeriesCollection = FileLoader.LoadFiles();

            foreach(var item in SeriesCollection)
                plotList.Items.Add(item.Title);

            DataContext = this;
        }

        private void TextBox_TextChanged_X(object sender, TextChangedEventArgs e)
        {
            if (textX != null)
                NameX = textX.Text.ToString();
        }

        private void TextBox_TextChanged_Y(object sender, TextChangedEventArgs e)
        {
            if (textY != null)
                NameY = textY.Text.ToString();
        }

        private void PlotList_SelectionRemoved(object sender, SelectionChangedEventArgs e)
        {
            if (plotList.SelectedIndex >= 0)
            {
                SeriesCollection.RemoveAt(plotList.SelectedIndex);
                plotList.Items.RemoveAt(plotList.Items.IndexOf(plotList.SelectedItem));
            }
        }
    }
}

