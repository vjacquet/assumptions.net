using System.Data;
using System.Data.SQLite;
using Xunit;

namespace NetCore.Assumptions.Data
{
    public class AboutDataSets
    {
        static SQLiteConnection CreateDatase()
        {
            var connection = new SQLiteConnection("DataSource=:memory:");
            connection.Open();

            SQLiteCommand create = connection.CreateCommand();
            create.CommandText = @"CREATE TABLE Company(
Id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
Name    TEXT    NOT NULL,
Age     INT     NOT NULL,
Address CHAR(50),
Salary  REAL);

INSERT INTO Company (Name,Age,Address,Salary)
VALUES ('Paul', 32, 'California', 20000.00 )
     , ('Allen', 25, 'Texas', 15000.00 )
     , ('Teddy', 23, 'Norway', 20000.00 )
     , ('Mark', 25, 'Rich-Mond ', 65000.00 )
     , ('David', 27, 'Texas', 85000.00 )
     , ('Kim', 22, 'South-Hall', 45000.00 );";

            create.ExecuteNonQuery();

            return connection;
        }

        [Fact]
        public void CanLoad()
        {
            using (SQLiteConnection connection = CreateDatase())
            {
                var adapter = new SQLiteDataAdapter("SELECT * FROM Company", connection)
                {
                    MissingSchemaAction = MissingSchemaAction.AddWithKey
                };

                var dataSet = new DataSet();
                adapter.FillSchema(dataSet.Tables.Add("Company"), SchemaType.Source);

                DataTable table = dataSet.Tables["Company"];
                adapter.Fill(table);

                Assert.Equal(6, table.Rows.Count);
            }
        }

        [Fact]
        public void CanFind()
        {
            using (SQLiteConnection connection = CreateDatase())
            {
                var adapter = new SQLiteDataAdapter("SELECT * FROM Company", connection)
                {
                    MissingSchemaAction = MissingSchemaAction.AddWithKey
                };

                var dataSet = new DataSet();
                adapter.FillSchema(dataSet.Tables.Add("Company"), SchemaType.Source);

                DataTable table = dataSet.Tables["Company"];
                adapter.Fill(table);

                DataRow found = table.Rows.Find(2);
                Assert.NotNull(found);
                Assert.Equal("Allen", found["Name"]);
            }
        }

        [Fact]
        public void CanUpdate()
        {
            using (SQLiteConnection connection = CreateDatase())
            {
                var adapter = new SQLiteDataAdapter("SELECT * FROM Company", connection)
                {
                    MissingSchemaAction = MissingSchemaAction.AddWithKey
                };
                _ = new SQLiteCommandBuilder(adapter); // required to setup Insert/Update/Delete commands.

                var dataSet = new DataSet();
                adapter.FillSchema(dataSet.Tables.Add("Company"), SchemaType.Source);

                var table = dataSet.Tables["Company"];
                adapter.Fill(table);

                table.Rows.Add(0, "Patricia", 25, "Norway", 20500.00);

                var found = table.Rows.Find(2);
                found["Salary"] = 16000.00m;

                table.Rows.Find(5).Delete();

                var changes = table.GetChanges();
                int affected = adapter.Update(changes);
                dataSet.AcceptChanges();

                Assert.Equal(3, affected);
            }
        }
    }
}
