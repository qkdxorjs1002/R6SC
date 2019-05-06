using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace R6SC
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public struct Postiton
        {
            public int sti, eni;

            public Postiton(int sti, int eni)
            {
                this.sti = sti;
                this.eni = eni;
            }
        }

        public string fileStream, filePath;

        public MainWindow()
        {
            InitializeComponent();

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Config files (*.ini)|*.ini|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                fileStream = File.ReadAllText(openFileDialog.FileName);
                ServerTextblock.Text = "(" + GetHint(fileStream) + ")";
            }
            else
            {
                this.Close();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void EasButton_Click(object sender, RoutedEventArgs e)
        {
            fileStream = SetHint(fileStream, "eas");
            ServerTextblock.Text = "(" + GetHint(fileStream) + ")";
            File.WriteAllText(filePath, fileStream);
        }

        private void WjaButton_Click(object sender, RoutedEventArgs e)
        {
            fileStream = SetHint(fileStream, "wja");
            ServerTextblock.Text = "(" + GetHint(fileStream) + ")";
            File.WriteAllText(filePath, fileStream);
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            fileStream = SetHint(fileStream, "default");
            ServerTextblock.Text = "(" + GetHint(fileStream) + ")";
            File.WriteAllText(filePath, fileStream);
        }

        private Postiton WhereHint(string target)
        {
            int sti = target.IndexOf("DataCenterHint=") + 15;
            int eni = target.IndexOf('\r', sti);

            return new Postiton(sti, eni);
        }

        private string GetHint(string target)
        {
            Postiton position = WhereHint(target);

            return target.Substring(position.sti, position.eni - position.sti);
        }
        
        private string SetHint(string target, string server)
        {
            Postiton position = WhereHint(target);

            target = target.Remove(position.sti, position.eni - position.sti).Insert(position.sti, server);

            return target;
        }
    }
}
