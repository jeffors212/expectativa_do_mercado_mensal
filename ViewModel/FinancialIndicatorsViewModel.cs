using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Adicione essa linha no início do arquivo
using Processo_Seletivo.Models;

public class FinancialIndicatorsViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    private ObservableCollection<IndicatorData> _indicatorDataList;
    public ObservableCollection<IndicatorData> IndicatorDataList {
        get { return _indicatorDataList; }
        set {
            _indicatorDataList = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IndicatorDataList)));
        }
    }

    public FinancialIndicatorsViewModel() {
        IndicatorDataList = new ObservableCollection<IndicatorData>();
    }

    public async Task LoadDataAsync(DateTime startDate, DateTime endDate, string indicatorType) {
        try {
            var startDateFormat = startDate.ToString("yyyy-MM-dd");
            var endDateFormat = endDate.ToString("yyyy-MM-dd");

            var url = $"https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativaMercadoMensais?$filter=Indicador eq '{indicatorType}' and DataIndicador ge {startDateFormat} and DataIndicador le {endDateFormat}";

            using (var client = new HttpClient()) {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Rootobject>(responseString);
                IndicatorDataList = new ObservableCollection<IndicatorData>(data.value);
            }
        } catch (Exception ex) {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
        }
    }

    public void ExportToCSV(string filePath) {
        try {
            using (var writer = new StreamWriter(filePath)) {
                writer.WriteLine("Date,IndicatorType,Value");
                foreach (var data in IndicatorDataList) {
                    writer.WriteLine($"{data.Date:yyyy-MM-dd},{data.IndicatorType},{data.Value}");
                }
            }
        } catch (Exception ex) {
            Console.WriteLine($"Ocorreu um erro ao exportar para CSV: {ex.Message}");
        }
    }
}
