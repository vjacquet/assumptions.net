using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
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

        [Fact]
        public void CanGetSchema()
        {
            using (SQLiteConnection connection = CreateDatase())
            {
                var schema = connection.GetSchema("DataSourceInformation");
                var information = new Dictionary<string, object>();
                for (int i = 0; i < schema.Columns.Count; i++)
                {
                    information.Add(schema.Columns[i].ColumnName, schema.Rows[0][i]);
                }
                Assert.NotEmpty(information);
            }
        }

        [Fact]
        public void CanStoreCustomTypes()
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete

            var dataSet = new DataSet { RemotingFormat = SerializationFormat.Binary };
            var table = dataSet.Tables.Add("Test");
            var column = table.Columns.Add("custom");
            column.DataType = typeof(object);

            var row = table.NewRow();
            row["custom"] = new Custom("test");
            table.Rows.Add(row);

            var formatter = new BinaryFormatter();
            var store = new MemoryStream();
            formatter.Serialize(store, dataSet);

            store.Seek(0, SeekOrigin.Begin); // rewind
            var stored = (DataSet)formatter.Deserialize(store);
            Assert.IsType<Custom>(stored.Tables[0].Rows[0][0]);

#pragma warning restore SYSLIB0011 // Type or member is obsolete
        }

        [Fact]
        public void CannotQueryCustomTypesWithoutConvert()
        {
            var dataSet = new DataSet();
            var table = dataSet.Tables.Add("Test");
            var column = table.Columns.Add("custom");
            column.DataType = typeof(object);

            var row = table.NewRow();
            row["custom"] = new Custom("test");
            table.Rows.Add(row);

            Assert.Throws<EvaluateException>(() => _ = table.Select("[custom]='test'"));
        }

        [Fact]
        public void CanQueryCustomTypesWithConvert()
        {
            var dataSet = new DataSet();
            var table = dataSet.Tables.Add("Test");
            var column = table.Columns.Add("custom");
            column.DataType = typeof(object);

            var row = table.NewRow();
            row["custom"] = new Custom("test");
            table.Rows.Add(row);

            var result = table.Select("CONVERT([custom], 'System.String')='test'");
            Assert.NotEmpty(result);
        }
    }

    [Serializable]
    [TypeConverter(typeof(CustomConverter))]
    public class Custom
    {
        #region CustomConverter

        private class CustomConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null) throw GetConvertFromException(value);

                string s = value as string;
                if (s == null) throw new ArgumentException(nameof(value));

                return new Custom(s);
            }

            [SecurityCritical]
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType != null && value is Custom)
                {
                    var id = (Custom)value;
                    var s = id.ToString();
                    if (destinationType == typeof(InstanceDescriptor))
                    {
                        var ctor = typeof(Custom).GetConstructor(new Type[] { typeof(string) });
                        return new InstanceDescriptor(ctor, new object[] { s });
                    }
                    if (destinationType == typeof(string))
                    {
                        return s;
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return false;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }
        }

        #endregion

        private readonly string _value;

        public Custom(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
