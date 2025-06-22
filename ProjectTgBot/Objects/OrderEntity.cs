using System.ComponentModel;

namespace ProjectTgBot.Objects
{


    public class OrderEntry : INotifyPropertyChanged
    {
        private bool _isCompleted;
        private DateTime _date;
        private string _orderName = string.Empty;
        private decimal _income;
        private decimal _expense;

        public bool IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnPropertyChanged(nameof(IsCompleted)); }
        }

        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }

        public string OrderName
        {
            get => _orderName;
            set { _orderName = value; OnPropertyChanged(nameof(OrderName)); }
        }

        public decimal Income
        {
            get => _income;
            set
            {
                _income = value;
                OnPropertyChanged(nameof(Income));
                OnPropertyChanged(nameof(Profit));
            }
        }

        public decimal Expense
        {
            get => _expense;
            set
            {
                _expense = value;
                OnPropertyChanged(nameof(Expense));
                OnPropertyChanged(nameof(Profit));
            }
        }

        public decimal Profit => Income - Expense;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}