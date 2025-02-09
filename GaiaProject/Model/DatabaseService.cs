using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiaProject.Model
{
    public class DatabaseService
    {
        private  string connectionString;

        public DatabaseService()
        {
            connectionString = @"Data Source=DESKTOP-5HI22AR\SQLEXPRESS;Initial Catalog=GaiaCalculatorDB;Integrated Security=True";
        }

        public void SaveOperation(OperationModel operation)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO OperationHistory (ValueA, ValueB, Operation, Result, Timestamp) 
                               VALUES (@ValueA, @ValueB, @Operation, @Result, @Timestamp)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ValueA", operation.ValueA);
                cmd.Parameters.AddWithValue("@ValueB", operation.ValueB);
                cmd.Parameters.AddWithValue("@Operation", operation.Operation);
                cmd.Parameters.AddWithValue("@Result", operation.Result);
                cmd.Parameters.AddWithValue("@Timestamp", operation.Timestamp);

                 
                cmd.ExecuteNonQuery();
            }
        }

        public List<OperationModel> GetHistory()
        {
            List<OperationModel> history = new List<OperationModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM OperationHistory ORDER BY Timestamp DESC";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        history.Add(new OperationModel
                        {
                            ValueA = Convert.ToDouble(reader["ValueA"]),
                            ValueB = Convert.ToDouble(reader["ValueB"]),
                            Operation = reader["Operation"].ToString(),
                            Result = Convert.ToDouble(reader["Result"]),
                            Timestamp = Convert.ToDateTime(reader["Timestamp"])
                        });
                    }
                }
            }

            return history;
        }

    }
}
