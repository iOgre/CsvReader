using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using CsvReader.Service;
using Microsoft.Win32;

namespace CsvReader.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellation = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
            Task.Factory.StartNew(PopulateGridAsync).Wait();
        }

        private async void LoadFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Comma Separated | *.csv",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = openFileDialog.FileName;
                
                await LoadCsvFileAsync(fileName);
            }
        }

        private async Task LoadCsvFileAsync(string fileName)
        {

            ProgressBar.Maximum = await CsvService.GetLinesAsync(fileName);
            var progress = new Progress<int>(async k => await PopulateGridAsync());
            progress.ProgressChanged += (sender, i) =>
            {

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ProgressBar.Value += i;
                    Status.Content = $"Loaded {ProgressBar.Value} from {ProgressBar.Maximum} lines";
                });
            }; 
          
            await CsvService.DeleteParticipantsAsync()
                .ContinueWith(t => CsvService.StoreParticipantsAsync(fileName, progress, _cancellation.Token, blockSize: 10000
                    )
                    .ContinueWith(k => PopulateGridAsync()));
        }

        private async Task PopulateGridAsync()
        {
            var service = new ParticipantService(new ParticipantRepository());
            var items = await service.GetAsync();
            Application.Current.Dispatcher.Invoke(() => { ParticipantsGrid.ItemsSource = items; });
        }


        private async void ClearDatabase_OnClick(object sender, RoutedEventArgs e)
        {
            await CsvService.DeleteParticipantsAsync()
                .ContinueWith(t => PopulateGridAsync());
        }

        private async void ParticipantsGrid_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await PopulateGridAsync();
        }

        private async void ParticipantsGrid_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var item = e.Row.Item as ParticipantDTO;
            switch (e.EditAction)
            {
                case DataGridEditAction.Cancel:
                    await PopulateGridAsync();
                    break;
                case DataGridEditAction.Commit:
                    var command = new CreateOrUpdateParticipantCommand(item);
                    await command.ExecuteAsync().ContinueWith(t => PopulateGridAsync());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (ParticipantsGrid.SelectedItem is ParticipantDTO selectedItem)
            {
                var command = new DeleteParticipantsCommand(t => t.Id == selectedItem.Id);
                await command.ExecuteAsync().ContinueWith(t => PopulateGridAsync());
            }
           
        }

        private void StopLoading_OnClick(object sender, RoutedEventArgs e)
        {
           _cancellation.Cancel();
        }
    }
}