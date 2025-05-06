using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SQLScriptGenerator
{
    public partial class MainForm : Form
    {
        private AppSettings _appSettings;
        private DatabaseService _databaseService;
        private ScriptGeneratorService _scriptGeneratorService;
        private List<Database> _databases;
        private Database _selectedDatabase;
        private List<Table> _selectedTables = new List<Table>();

        public MainForm()
        {
            InitializeComponent();
            LoadAppSettings();
            InitializeServices();
            SetupComboBox();
            ConnectToServer();
        }

        private void LoadAppSettings()
        {
            try
            {
                string json = File.ReadAllText("appsettings.json");
                _appSettings = JsonSerializer.Deserialize<AppSettings>(json);
                lblConnectionStatus.Text = "Configuration loaded.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblConnectionStatus.Text = "Failed to load configuration.";
            }
        }

        private void InitializeServices()
        {
            _databaseService = new DatabaseService();
            _scriptGeneratorService = new ScriptGeneratorService();
        }

        private void SetupComboBox()
        {
            cboScriptType.Items.Add("Create Table (Empty)");
            cboScriptType.Items.Add("Create Table with Data");
            cboScriptType.Items.Add("Create Table with Modified Company Code");
            cboScriptType.SelectedIndex = 0;

            // Hide company code fields initially
            lblSourceCompanyCode.Visible = false;
            txtSourceCompanyCode.Visible = false;
            lblTargetCompanyCode.Visible = false;
            txtTargetCompanyCode.Visible = false;
        }

        private async void ConnectToServer()
        {
            try
            {
                lblConnectionStatus.Text = "Connecting to server...";
                _databases = await _databaseService.GetDatabasesAsync(_appSettings.ConnectionString);

                // Update the UI with database names
                foreach (var db in _databases)
                {
                    clbDatabases.Items.Add(db.Name, false);
                }

                lblConnectionStatus.Text = $"Connected to {_appSettings.Server}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to the server: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblConnectionStatus.Text = "Failed to connect.";
            }
        }

        private async void clbDatabases_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Get the selected database
            string dbName = clbDatabases.Items[e.Index].ToString();

            // If the item is being checked
            if (e.NewValue == CheckState.Checked)
            {
                _selectedDatabase = _databases.FirstOrDefault(d => d.Name == dbName);

                // Clear previous tables
                clbTables.Items.Clear();

                try
                {
                    // Load tables for the selected database
                    var tables = await _databaseService.GetTablesAsync(_appSettings.ConnectionString, dbName);
                    _selectedDatabase.Tables = tables;

                    // Add tables to the checklist
                    foreach (var table in tables)
                    {
                        clbTables.Items.Add(table.Name, false);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading tables: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // If the database is being unchecked, clear the tables list
                clbTables.Items.Clear();
                _selectedDatabase = null;
            }
        }

        private void clbTables_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_selectedDatabase == null) return;

            string tableName = clbTables.Items[e.Index].ToString();
            Table table = _selectedDatabase.Tables.FirstOrDefault(t => t.Name == tableName);

            if (e.NewValue == CheckState.Checked)
            {
                if (!_selectedTables.Contains(table))
                {
                    _selectedTables.Add(table);
                }
            }
            else
            {
                _selectedTables.Remove(table);
            }
        }

        private void cboScriptType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Show company code fields only when the third option is selected
            bool showCompanyCodeFields = cboScriptType.SelectedIndex == 2;
            lblSourceCompanyCode.Visible = showCompanyCodeFields;
            txtSourceCompanyCode.Visible = showCompanyCodeFields;
            lblTargetCompanyCode.Visible = showCompanyCodeFields;
            txtTargetCompanyCode.Visible = showCompanyCodeFields;
        }

        private async void btnGenerateScript_Click(object sender, EventArgs e)
        {
            if (_selectedDatabase == null || _selectedTables.Count == 0)
            {
                MessageBox.Show("Please select at least one database and table.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                ScriptOptions options = new ScriptOptions
                {
                    ScriptType = (ScriptType)cboScriptType.SelectedIndex,
                    SourceCompanyCode = txtSourceCompanyCode.Text,
                    TargetCompanyCode = txtTargetCompanyCode.Text
                };

                txtScript.Text = "Generating script...";
                string script = await _scriptGeneratorService.GenerateScriptAsync(
                    _appSettings.ConnectionString,
                    _selectedDatabase.Name,
                    _selectedTables,
                    options);

                txtScript.Text = script;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating script: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtScript.Text = $"/* Error generating script: {ex.Message} */";
            }
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtScript.Text))
            {
                Clipboard.SetText(txtScript.Text);
                MessageBox.Show("Script copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSaveScript_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtScript.Text))
            {
                MessageBox.Show("No script to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
                DefaultExt = "sql",
                Title = "Save SQL Script"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, txtScript.Text);
                    MessageBox.Show("Script saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    // Model classes
    public class AppSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool IntegratedSecurity { get; set; }

        public string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = Server,
                    InitialCatalog = Database,
                    IntegratedSecurity = IntegratedSecurity
                };

                if (!IntegratedSecurity)
                {
                    builder.UserID = UserId;
                    builder.Password = Password;
                }

                return builder.ConnectionString;
            }
        }
    }

    public class Database
    {
        public string Name { get; set; }
        public List<Table> Tables { get; set; } = new List<Table>();
    }

    public class Table
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public List<Column> Columns { get; set; } = new List<Column>();
    }

    public class Column
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
    }

    public enum ScriptType
    {
        CreateTableEmpty,
        CreateTableWithData,
        CreateTableWithModifiedCompanyCode
    }

    public class ScriptOptions
    {
        public ScriptType ScriptType { get; set; }
        public string SourceCompanyCode { get; set; }
        public string TargetCompanyCode { get; set; }
    }

    // Database service for interacting with SQL Server
    public class DatabaseService
    {
        public async Task<List<Database>> GetDatabasesAsync(string connectionString)
        {
            List<Database> databases = new List<Database>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Query to get all user databases
                string query = @"
                    SELECT name
                    FROM sys.databases
                    WHERE database_id > 4  -- Skip system databases
                    ORDER BY name";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            databases.Add(new Database
                            {
                                Name = reader["name"].ToString()
                            });
                        }
                    }
                }
            }

            return databases;
        }

        public async Task<List<Table>> GetTablesAsync(string connectionString, string databaseName)
        {
            List<Table> tables = new List<Table>();

            // Create a new connection string with the specific database
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = databaseName
            };

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                await connection.OpenAsync();

                // Query to get all tables
                string query = @"
                    SELECT 
                        s.name AS SchemaName,
                        t.name AS TableName
                    FROM 
                        sys.tables t
                    INNER JOIN 
                        sys.schemas s ON t.schema_id = s.schema_id
                    ORDER BY 
                        s.name, t.name";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tables.Add(new Table
                            {
                                Schema = reader["SchemaName"].ToString(),
                                Name = reader["TableName"].ToString()
                            });
                        }
                    }
                }

                // For each table, get its columns
                foreach (var table in tables)
                {
                    await GetTableColumnsAsync(connection, table);
                }
            }

            return tables;
        }

        private async Task GetTableColumnsAsync(SqlConnection connection, Table table)
        {
            string query = @"
                SELECT 
                    c.name AS ColumnName,
                    t.name AS DataType,
                    c.is_nullable AS IsNullable,
                    CASE WHEN pk.column_id IS NOT NULL THEN 1 ELSE 0 END AS IsPrimaryKey
                FROM 
                    sys.columns c
                INNER JOIN 
                    sys.types t ON c.user_type_id = t.user_type_id
                LEFT JOIN 
                    (SELECT ic.object_id, ic.column_id
                     FROM sys.index_columns ic
                     INNER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
                     WHERE i.is_primary_key = 1) pk 
                    ON c.object_id = pk.object_id AND c.column_id = pk.column_id
                WHERE 
                    OBJECT_SCHEMA_NAME(c.object_id) = @Schema AND
                    OBJECT_NAME(c.object_id) = @TableName
                ORDER BY 
                    c.column_id";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Schema", table.Schema);
                command.Parameters.AddWithValue("@TableName", table.Name);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        table.Columns.Add(new Column
                        {
                            Name = reader["ColumnName"].ToString(),
                            DataType = reader["DataType"].ToString(),
                            IsNullable = Convert.ToBoolean(reader["IsNullable"]),
                            IsPrimaryKey = Convert.ToBoolean(reader["IsPrimaryKey"])
                        });
                    }
                }
            }
        }
    }

    // Service for generating SQL scripts
    public class ScriptGeneratorService
    {
        public async Task<string> GenerateScriptAsync(string connectionString, string databaseName, List<Table> tables, ScriptOptions options)
        {
            StringBuilder script = new StringBuilder();

            // Create a new connection string with the specific database
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = databaseName
            };

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                await connection.OpenAsync();

                foreach (var table in tables)
                {
                    // Add table creation script
                    script.AppendLine($"-- Script for {table.Schema}.{table.Name}");
                    script.AppendLine(await GenerateTableCreationScript(connection, table));

                    // Add data insertion scripts if needed
                    if (options.ScriptType != ScriptType.CreateTableEmpty)
                    {
                        script.AppendLine();
                        script.AppendLine($"-- Data for {table.Schema}.{table.Name}");
                        script.AppendLine(await GenerateDataInsertionScript(connection, table, options));
                    }

                    script.AppendLine();
                    script.AppendLine("GO");
                    script.AppendLine();
                }
            }

            return script.ToString();
        }

        private async Task<string> GenerateTableCreationScript(SqlConnection connection, Table table)
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine($"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{table.Name}' AND schema_id = SCHEMA_ID('{table.Schema}'))");
            script.AppendLine("BEGIN");
            script.AppendLine($"    CREATE TABLE [{table.Schema}].[{table.Name}] (");

            // Get primary key columns
            var primaryKeyColumns = table.Columns.Where(c => c.IsPrimaryKey).ToList();

            // Add column definitions
            for (int i = 0; i < table.Columns.Count; i++)
            {
                var column = table.Columns[i];
                string columnDefinition = $"        [{column.Name}] [{column.DataType}]";

                // For simplicity, we're not handling detailed data type specifications here
                // You might want to add length/precision/scale for certain data types

                if (!column.IsNullable)
                    columnDefinition += " NOT NULL";
                else
                    columnDefinition += " NULL";

                // Add comma if it's not the last column or if there will be a PK constraint
                if (i < table.Columns.Count - 1 || primaryKeyColumns.Count > 0)
                    columnDefinition += ",";

                script.AppendLine(columnDefinition);
            }

            // Add primary key constraint if any
            if (primaryKeyColumns.Count > 0)
            {
                script.Append($"        CONSTRAINT [PK_{table.Name}] PRIMARY KEY (");
                script.Append(string.Join(", ", primaryKeyColumns.Select(c => $"[{c.Name}]")));
                script.AppendLine(")");
            }

            script.AppendLine("    )");
            script.AppendLine("END");

            return script.ToString();
        }

        private async Task<string> GenerateDataInsertionScript(SqlConnection connection, Table table, ScriptOptions options)
        {
            StringBuilder script = new StringBuilder();

            // Build query to get data
            string query = $"SELECT * FROM [{table.Schema}].[{table.Name}]";

            // Add filter for company code if needed
            if (options.ScriptType == ScriptType.CreateTableWithModifiedCompanyCode &&
                !string.IsNullOrEmpty(options.SourceCompanyCode))
            {
                // Check if the table has the company code column
                bool hasCompanyCodeColumn = table.Columns.Any(c => c.Name.Equals("Sirkod", StringComparison.OrdinalIgnoreCase));

                if (hasCompanyCodeColumn)
                {
                    query += $" WHERE Sirkod = '{options.SourceCompanyCode}'";
                }
            }

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // If no data, return empty script
                        if (!reader.HasRows)
                        {
                            return "-- No data to script";
                        }

                        while (await reader.ReadAsync())
                        {
                            script.Append($"INSERT INTO [{table.Schema}].[{table.Name}] (");
                            script.Append(string.Join(", ", table.Columns.Select(c => $"[{c.Name}]")));
                            script.AppendLine(") VALUES (");

                            List<string> values = new List<string>();

                            for (int i = 0; i < table.Columns.Count; i++)
                            {
                                string columnName = table.Columns[i].Name;
                                object value = reader[columnName];
                                string formattedValue;

                                // Transform company code if needed
                                if (options.ScriptType == ScriptType.CreateTableWithModifiedCompanyCode &&
                                    !string.IsNullOrEmpty(options.TargetCompanyCode) &&
                                    columnName.Equals("Sirkod", StringComparison.OrdinalIgnoreCase))
                                {
                                    formattedValue = $"'{options.TargetCompanyCode}'";
                                }
                                else
                                {
                                    // Format value according to its type
                                    formattedValue = FormatValueForSql(value);
                                }

                                values.Add(formattedValue);
                            }

                            script.AppendLine($"    {string.Join(", ", values)}");
                            script.AppendLine(");");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"-- Error generating data script: {ex.Message}";
            }

            return script.ToString();
        }

        private string FormatValueForSql(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return "NULL";
            }

            switch (value)
            {
                case string strValue:
                    // Escape single quotes in string
                    return $"'{strValue.Replace("'", "''")}'";
                case DateTime dateValue:
                    return $"'{dateValue:yyyy-MM-dd HH:mm:ss.fff}'";
                case bool boolValue:
                    return boolValue ? "1" : "0";
                default:
                    return value.ToString();
            }
        }
    }
}