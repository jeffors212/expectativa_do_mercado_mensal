using Microsoft.Win32;
using System;
using System.Windows;
using Processo_Seletivo.Models;

namespace Processo_Seletivo {
    public partial class MainWindow : Window {
        private FinancialIndicatorsViewModel viewModel;

        public MainWindow() {
            InitializeComponent();
            viewModel = new FinancialIndicatorsViewModel();
            DataContext = viewModel;

            IndicatorTypeComboBox.ItemsSource = new[] { "IPCA", "IGP-M", "Selic" };
            IndicatorTypeComboBox.SelectedIndex = 0; // Seleciona o primeiro tipo por padrão
        }

        private async void ConsultButton_Click(object sender, RoutedEventArgs e) {
            DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.Now;
            string indicatorType = IndicatorTypeComboBox.SelectedItem.ToString();

            await viewModel.LoadDataAsync(startDate, endDate, indicatorType);
        }

        private void ExportCSVButton_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "IndicadoresFinanceiros";
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV documents (.csv)|*.csv";

            if (dlg.ShowDialog() == true) {
                viewModel.ExportToCSV(dlg.FileName);
                MessageBox.Show("Dados exportados com sucesso!");
            }
        }
    }
}
