using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResistorsManager.Models
{
    public class Resistor : IDisposable
    {
        public string Marking { get; set; }
        public string Type { get; set; }
        public double Power { get; set; }
        public double Resistance { get; set; }
        public double Accuracy { get; set; }
        public string PreciousMetal { get; set; }
        public string Element { get; set; }
        public double Content { get; set; }
        public string Technology { get; set; }
        public int ProductionYear { get; set; }
        public string Manufacturer { get; set; }
        public string Status { get; set; }

        // Метод для преобразования строки сопротивления в числовое значение в Омах
        public static double ParseResistance(string resistanceStr)
        {
            if (string.IsNullOrEmpty(resistanceStr))
                return 0;

            resistanceStr = resistanceStr.Trim().ToLower();

            // Убираем пробелы и преобразуем в числовое значение
            double multiplier = 1;

            if (resistanceStr.EndsWith("ком") || resistanceStr.EndsWith("кОм") || resistanceStr.Contains(" к"))
            {
                multiplier = 1000;
                resistanceStr = resistanceStr.Replace("ком", "").Replace("кОм", "").Replace(" к", "").Trim();
            }
            else if (resistanceStr.EndsWith("мом") || resistanceStr.EndsWith("МОм") || resistanceStr.Contains(" М"))
            {
                multiplier = 1000000;
                resistanceStr = resistanceStr.Replace("мом", "").Replace("МОм", "").Replace(" М", "").Trim();
            }
            else if (resistanceStr.EndsWith("ом") || resistanceStr.EndsWith("Ом"))
            {
                resistanceStr = resistanceStr.Replace("ом", "").Replace("Ом", "").Trim();
            }

            // Заменяем точку на запятую для корректного парсинга
            resistanceStr = resistanceStr.Replace(".", ",");

            if (double.TryParse(resistanceStr, out double value))
            {
                return value * multiplier;
            }

            return 0;
        }

        // Метод для преобразования строки точности в числовое значение
        public static double ParseAccuracy(string accuracyStr)
        {
            if (string.IsNullOrEmpty(accuracyStr))
                return 0;

            accuracyStr = accuracyStr.Trim().Replace("±", "").Replace("%", "");

            if (double.TryParse(accuracyStr, out double value))
            {
                return value;
            }

            return 0;
        }

        // Метод для форматирования сопротивления обратно в читаемый вид
        public static string FormatResistance(double resistance)
        {
            if (resistance >= 1000000)
                return $"{resistance / 1000000:0.###} МОм";
            else if (resistance >= 1000)
                return $"{resistance / 1000:0.###} кОм";
            else
                return $"{resistance:0.###} Ом";
        }

        // Метод для форматирования точности обратно в читаемый вид
        public static string FormatAccuracy(double accuracy)
        {
            return $"±{accuracy}%";
        }

        public void Dispose()
        {

        }
    }
}
