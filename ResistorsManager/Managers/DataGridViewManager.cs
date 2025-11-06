// DataGridViewManager.cs
using ResistorsManager.Models;
using System.Windows.Forms;

namespace ResistorsManager.Managers
{
    public class DataGridViewManager
    {
        private DataGridView _dataGridView;

        public DataGridViewManager(DataGridView dataGridView)
        {
            _dataGridView = dataGridView;
        }

        public void InitializeDataGridView()
        {
            _dataGridView.AutoGenerateColumns = false;
            _dataGridView.Columns.Clear();

            AddColumn("Marking", "Маркировка", "Marking");
            AddColumn("Type", "Тип", "Type");
            AddColumn("Power", "Мощность (Вт)", "Power");
            AddColumn("Resistance", "Сопротивление", "ResistanceFormatted");
            AddColumn("Accuracy", "Точность", "AccuracyFormatted");
            AddColumn("PreciousMetal", "Драгоценный металл", "PreciousMetal");
            AddColumn("Element", "Элемент", "Element");
            AddColumn("Content", "Содержание (г/шт)", "Content");
            AddColumn("Technology", "Технология", "Technology");
            AddColumn("ProductionYear", "Год выпуска", "ProductionYear");
            AddColumn("Manufacturer", "Производитель", "Manufacturer");
            AddColumn("Status", "Статус", "Status");
        }

        private void AddColumn(string name, string headerText, string dataPropertyName)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = dataPropertyName
            };
            _dataGridView.Columns.Add(column);
        }

        public void DisplayData(List<Resistor> resistors)
        {
            var sortedResistors = resistors
                .OrderBy(c => c.Status)
                .ThenBy(c => c.Marking)
                .ToList();

            _dataGridView.DataSource = null;
            _dataGridView.DataSource = sortedResistors.Select(c => new
            {
                c.Marking,
                c.Type,
                c.Power,
                ResistanceFormatted = Resistor.FormatResistance(c.Resistance),
                AccuracyFormatted = Resistor.FormatAccuracy(c.Accuracy),
                c.PreciousMetal,
                c.Element,
                c.Content,
                c.Technology,
                c.ProductionYear,
                c.Manufacturer,
                c.Status
            }).ToList();
        }
    }
}