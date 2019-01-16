using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace Rejestrator.Logic
{
    public class FileLoader
    {
        private string actualPath { get; set; }

        public FileLoader()
        {
            actualPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            actualPath += @"\data";
        }

        public void PreparationFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);

                string path = "";
                foreach (var line in lines)
                {
                    if (!line.StartsWith("#"))
                    {
                        File.AppendAllText("data/" + path + ".txt", line);
                    }
                    else
                    {
                        path = line;
                        File.AppendAllText("data/" + path + ".txt", path);
                    }
                    File.AppendAllText("data/" + path + ".txt", Environment.NewLine);
                }
            }
        }

        public SeriesCollection LoadFiles()
        {
            SeriesCollection SeriesCollection = new SeriesCollection();

            string[] filePaths = Directory.GetFiles(actualPath, "*.txt", SearchOption.TopDirectoryOnly);

            foreach (var file in filePaths)
            {
                ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint>();

                string[] dataFile = File.ReadAllLines(file);

                foreach (var line in dataFile)
                {
                    if (!line.StartsWith("#"))
                    {
                        var value = line.Split(' ');

                        double x = 0.0;
                        double y = 0.0;

                        Double.TryParse(value[0], out x);
                        Double.TryParse(value[1], out y);

                        points.Add(new ObservablePoint
                        {
                            X = x,
                            Y = y
                        });
                    }
                    else
                    {
                        SeriesCollection.Add(new LineSeries
                        {
                            Title = line.Replace("#", ""),
                            Values = points
                        });
                    }
                }
            }
            return SeriesCollection;
        }

        public void RemoveFile()
        {
            DirectoryInfo dir = new DirectoryInfo(actualPath);

            foreach (FileInfo file in dir.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
