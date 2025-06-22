using ClosedXML.Excel;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ProjectTgBot
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<OrderEntry> Orders { get; set; } = new();
        public ObservableCollection<OrderEntry> FilteredOrders => new(Orders.Where(o => o.Date.Month == GetSelectedMonthNumber()));

        public ObservableCollection<string> Months { get; set; } = new ObservableCollection<string>(CultureInfo.GetCultureInfo("ru-RU")
     .DateTimeFormat.MonthNames
     .Where(m => !string.IsNullOrEmpty(m))
     .ToList());

        private string _selectedMonthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(DateTime.Now.Month);
        public string SelectedMonthName
        {
            get => _selectedMonthName;
            set
            {
                _selectedMonthName = value;
                OnPropertyChanged(nameof(SelectedMonthName));
                OnPropertyChanged(nameof(FilteredOrders));
                OnPropertyChanged(nameof(TotalIncome));
                OnPropertyChanged(nameof(TotalExpense));
                OnPropertyChanged(nameof(TotalProfit));
            }
        }


        public decimal TotalIncome => FilteredOrders.Sum(o => o.Income);
        public decimal TotalExpense => FilteredOrders.Sum(o => o.Expense);
        public decimal TotalProfit => FilteredOrders.Sum(o => o.Profit);

        public ICommand AddOrderCommand => new RelayCommand(_ => AddOrder());
        public ICommand ExportExcelCommand => new RelayCommand(_ => ExportToExcel());

        public MainViewModel()
        {
            Orders.CollectionChanged += (_, __) => OnPropertyChanged(nameof(FilteredOrders));
        }

        private int GetSelectedMonthNumber()
        {
            var ru = CultureInfo.GetCultureInfo("ru-RU");
            for (int i = 1; i <= 12; i++)
            {
                if (ru.DateTimeFormat.GetMonthName(i).Equals(SelectedMonthName, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return DateTime.Now.Month;
        }


        private void AddOrder()
        {
            var month = GetSelectedMonthNumber();
            var year = DateTime.Now.Year;

            // дата по умолчанию — 1 число выбранного месяца
            var orderDate = new DateTime(year, month, 1);

            Orders.Add(new OrderEntry
            {
                IsCompleted = false,
                Date = orderDate,
                OrderName = "Новый заказ",
                Income = 0,
                Expense = 0
            });

            OnPropertyChanged(nameof(FilteredOrders));
            OnPropertyChanged(nameof(TotalIncome));
            OnPropertyChanged(nameof(TotalExpense));
            OnPropertyChanged(nameof(TotalProfit));
        }

        public ICommand ImportExcelCommand => new RelayCommand(_ => ImportFromExcel());

        private void ImportFromExcel()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Выберите Excel-файл"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            try
            {
                using var workbook = new XLWorkbook(openFileDialog.FileName);
                var worksheet = workbook.Worksheet(1); // первый лист

                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // пропустить заголовки
                foreach (var row in rows)
                {
                    var dateCell = row.Cell(1).GetString();
                    var name = row.Cell(2).GetString();
                    var incomeCell = row.Cell(3).GetString();
                    var expenseCell = row.Cell(4).GetString();
                    var isCompletedCell = row.Cell(6).GetString();

                    if (!DateTime.TryParseExact(dateCell, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                        continue;
                    if (!decimal.TryParse(incomeCell, out var income)) continue;
                    if (!decimal.TryParse(expenseCell, out var expense)) continue;
                    var isCompleted = isCompletedCell.Trim().ToLower() == "да" || isCompletedCell.Trim().ToLower() == "true";

                    Orders.Add(new OrderEntry
                    {
                        Date = date,
                        OrderName = name,
                        Income = income,
                        Expense = expense,
                        IsCompleted = isCompleted
                    });
                }

                OnPropertyChanged(nameof(FilteredOrders));
                OnPropertyChanged(nameof(TotalIncome));
                OnPropertyChanged(nameof(TotalExpense));
                OnPropertyChanged(nameof(TotalProfit));

                MessageBox.Show("Импорт из Excel выполнен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте Excel: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToExcel()
        {
            var saveDialog = new SaveFileDialog { Filter = "Excel Files|*.xlsx" };
            if (saveDialog.ShowDialog() != true) return;

            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Отчёт");

            sheet.Cell(1, 1).Value = "Дата";
            sheet.Cell(1, 2).Value = "Название";
            sheet.Cell(1, 3).Value = "Доход";
            sheet.Cell(1, 4).Value = "Расход";
            sheet.Cell(1, 5).Value = "Прибыль";
            sheet.Cell(1, 6).Value = "Выполнен";

            int row = 2;
            foreach (var order in Orders)
            {
                sheet.Cell(row, 1).Value = order.Date.ToShortDateString();
                sheet.Cell(row, 2).Value = order.OrderName;
                sheet.Cell(row, 3).Value = order.Income;
                sheet.Cell(row, 4).Value = order.Expense;
                sheet.Cell(row, 5).Value = order.Profit;
                sheet.Cell(row, 6).Value = order.IsCompleted ? "Да" : "Нет";
                row++;
            }

            workbook.SaveAs(saveDialog.FileName);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}