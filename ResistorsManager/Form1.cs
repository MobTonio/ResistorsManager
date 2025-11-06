using ResistorsManager.Managers;
using ResistorsManager.Models;
using ResistorsManager.Services;
using System.Windows.Forms;

namespace ResistorsManager
{
    public partial class MainForm : Form
    {
        private List<Resistor> _resistors = new List<Resistor>();
        private DataGridViewManager _dataGridViewManager;
        private FileService _fileService;
        private DatabaseService _databaseService;

        public MainForm()
        {
            InitializeComponent();

            // Инициализация сервисов
            _dataGridViewManager = new DataGridViewManager(dataGridView);
            _fileService = new FileService();
            _databaseService = new DatabaseService(
                "Server=localhost,51241;Database=master;Integrated Security=true;Encrypt=true;TrustServerCertificate=true;Connection Timeout=30");

            _dataGridViewManager.InitializeDataGridView();
        }

        private void btnLoadTxt_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt",
                    Title = "Выберите TXT файл"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _resistors = _fileService.LoadDataFromTxt(openFileDialog.FileName);
                    _fileService.SaveToExcel(_resistors);
                    _dataGridViewManager.DisplayData(_resistors);

                    MessageBox.Show("Данные успешно загружены из TXT и сохранены в CSV!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке TXT файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportToDb_Click(object sender, EventArgs e)
        {
            try
            {
                var cachedResistors = _fileService.GetCachedResistors();
                if (cachedResistors == null || !cachedResistors.Any())
                {
                    MessageBox.Show("Нет данных для выгрузки. Сначала загрузите данные из TXT.", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _databaseService.ExportToDatabase(cachedResistors);
                MessageBox.Show("Данные успешно выгружены в базу данных!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выгрузке в БД: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}