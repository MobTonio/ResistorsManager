using Microsoft.Data.SqlClient;
using ResistorsManager.Models;

namespace ResistorsManager.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ExportToDatabase(List<Resistor> resistors)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                CreateTableIfNotExists(connection);
                ClearTable(connection);
                InsertResistors(connection, resistors);
            }
        }

        private void CreateTableIfNotExists(SqlConnection connection)
        {
            string createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Resistors' AND xtype='U')
                CREATE TABLE Resistors (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Marking NVARCHAR(100),
                    Type NVARCHAR(50),
                    Power DECIMAL(10,3),
                    Resistance DECIMAL(18,6),
                    Accuracy DECIMAL(5,2),
                    PreciousMetal NVARCHAR(50),
                    Element NVARCHAR(10),
                    Content DECIMAL(10,6),
                    Technology NVARCHAR(50),
                    ProductionYear INT,
                    Manufacturer NVARCHAR(100),
                    Status NVARCHAR(50)
                )";

            using (SqlCommand command = new SqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void ClearTable(SqlConnection connection)
        {
            string clearTableQuery = "DELETE FROM Resistors";
            using (SqlCommand command = new SqlCommand(clearTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void InsertResistors(SqlConnection connection, List<Resistor> resistors)
        {
            string insertQuery = @"
                INSERT INTO Resistors (Marking, Type, Power, Resistance, Accuracy, PreciousMetal, 
                                      Element, Content, Technology, ProductionYear, Manufacturer, Status)
                VALUES (@Marking, @Type, @Power, @Resistance, @Accuracy, @PreciousMetal, 
                       @Element, @Content, @Technology, @ProductionYear, @Manufacturer, @Status)";

            foreach (var resistor in resistors)
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Marking", resistor.Marking);
                    command.Parameters.AddWithValue("@Type", resistor.Type);
                    command.Parameters.AddWithValue("@Power", resistor.Power);
                    command.Parameters.AddWithValue("@Resistance", resistor.Resistance);
                    command.Parameters.AddWithValue("@Accuracy", resistor.Accuracy);
                    command.Parameters.AddWithValue("@PreciousMetal", resistor.PreciousMetal);
                    command.Parameters.AddWithValue("@Element", resistor.Element);
                    command.Parameters.AddWithValue("@Content", resistor.Content);
                    command.Parameters.AddWithValue("@Technology", resistor.Technology);
                    command.Parameters.AddWithValue("@ProductionYear", resistor.ProductionYear);
                    command.Parameters.AddWithValue("@Manufacturer", resistor.Manufacturer);
                    command.Parameters.AddWithValue("@Status", resistor.Status);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}