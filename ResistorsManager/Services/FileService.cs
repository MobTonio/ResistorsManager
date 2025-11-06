using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ResistorsManager.Models;
using System.Runtime.Caching;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace ResistorsManager.Services
{
    public class FileService
    {
        private ObjectCache _cache = MemoryCache.Default;
        private const string CacheKey = "ResistorsData";

        public List<Resistor> LoadDataFromTxt(string filePath)
        {
            var resistors = new List<Resistor>();
            var lines = File.ReadAllLines(filePath);

            // Пропускаем заголовок
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = lines[i].Split(',');
                if (fields.Length >= 12)
                {
                    try
                    {
                        var resistor = new Resistor
                        {
                            Marking = fields[0],
                            Type = fields[1],
                            Power = double.Parse(fields[2].Replace(".", ",")),
                            Resistance = Resistor.ParseResistance(fields[3]),
                            Accuracy = Resistor.ParseAccuracy(fields[4]),
                            PreciousMetal = fields[5],
                            Element = fields[6],
                            Content = double.Parse(fields[7].Replace(".", ",")),
                            Technology = fields[8],
                            ProductionYear = int.Parse(fields[9]),
                            Manufacturer = fields[10],
                            Status = fields[11]
                        };
                        resistors.Add(resistor);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Ошибка при обработке строки {i + 1}: {ex.Message}", ex);
                    }
                }
            }

            // Кешируем данные
            _cache.Set(CacheKey, resistors, DateTimeOffset.Now.AddMinutes(30));
            return resistors;
        }


        public void SaveToExcel(List<Resistor> resistors)
        {
            Application excelApp = null;
            Workbook workbook = null;
            Worksheet worksheet = null;

            try
            {
                excelApp = new Application();
                excelApp.Visible = false; // Скрыть Excel
                excelApp.DisplayAlerts = false; // Отключить предупреждения

                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.ActiveSheet;

                // Заголовки
                string[] headers = {
            "Маркировка", "Тип", "Мощность (Вт)", "Сопротивление (Ом)",
            "Точность (%)", "Драгоценный металл", "Элемент", "Содержание (г/шт)",
            "Технология", "Год выпуска", "Производитель", "Статус"
        };

                for (int i = 0; i < headers.Length; i++)
                {
                    Range headerCell = worksheet.Cells[1, i + 1] as Range;
                    headerCell.Value = headers[i];
                    headerCell.Font.Bold = true;
                    Marshal.ReleaseComObject(headerCell);
                }

                // Данные
                int row = 2;
                foreach (var comp in resistors)
                {
                    worksheet.Cells[row, 1] = comp.Marking;
                    worksheet.Cells[row, 2] = comp.Type;
                    worksheet.Cells[row, 3] = comp.Power;
                    worksheet.Cells[row, 4] = comp.Resistance;
                    worksheet.Cells[row, 5] = comp.Accuracy;
                    worksheet.Cells[row, 6] = comp.PreciousMetal;
                    worksheet.Cells[row, 7] = comp.Element;
                    worksheet.Cells[row, 8] = comp.Content;
                    worksheet.Cells[row, 9] = comp.Technology;
                    worksheet.Cells[row, 10] = comp.ProductionYear;
                    worksheet.Cells[row, 11] = comp.Manufacturer;
                    worksheet.Cells[row, 12] = comp.Status;
                    row++;
                }

                // Автоподбор ширины столбцов
                worksheet.Columns.AutoFit();

                // Сохранение
                string excelPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "ResistorsData.xlsx");

                workbook.SaveAs(excelPath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении в Excel: {ex.Message}");
            }
            finally
            {
                // Правильное освобождение ресурсов в обратном порядке создания
                if (worksheet != null)
                {
                    Marshal.ReleaseComObject(worksheet);
                    worksheet = null;
                }

                if (workbook != null)
                {
                    workbook.Close(false);
                    Marshal.ReleaseComObject(workbook);
                    workbook = null;
                }

                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                    excelApp = null;
                }

                // Принудительная сборка мусора
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        public List<Resistor> GetCachedResistors()
        {
            return _cache.Get(CacheKey) as List<Resistor>;
        }
    }
}