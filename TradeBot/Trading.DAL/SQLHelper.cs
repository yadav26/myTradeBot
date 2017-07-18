using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Trading.DAL
{
    public class SQLHelper : IDisposable
    {
        private SqlConnection connection = null;
        //private String connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        private String connectionString = Properties.Settings.Default.ConnectionString;

        public String ConnectionString
        {
            set { connectionString = value; }
        }
        #region Constructors
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SQLHelper() { }
        /// <summary>
        /// Parameteraize constructor. It takes connection string as parameter.
        /// </summary>
        /// <remarks>If connection string is not specified then Default connection string is "dbConnection"</remarks>
        /// <param name="p_connectionString"></param>
        public SQLHelper(string p_connectionString) { if (!string.IsNullOrEmpty(p_connectionString.Trim()))connectionString = p_connectionString; }
        #endregion
        #region AddParameter Methods
        private void AddParameter(SqlCommand command, string parameterName, SqlDbType dbType, int size, ParameterDirection direction,
            byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            SqlParameter p = new SqlParameter(parameterName, dbType, size, direction, precision, scale, sourceColumn,
                sourceVersion, true, value, null, null, null);
            command.Parameters.Add(p);
        }
        public void AddParameter(SqlCommand command, string parameterName, SqlDbType dbType, int size, ParameterDirection direction, object value)
        {
            AddParameter(command, parameterName, dbType, size, direction, 0, 0, null, DataRowVersion.Current, value);
        }
        public void AddInParameter(SqlCommand command, string parameterName, SqlDbType dbType, object value)
        {
            AddParameter(command, parameterName, dbType, 0, ParameterDirection.Input, value);
        }
        public void AddOutParameter(SqlCommand command, string parameterName, SqlDbType dbType, int size)
        {
            AddParameter(command, parameterName, dbType, size, ParameterDirection.Output, null);
        }
        public object GetParameterValue(SqlCommand command, string parameterName)
        {
            return command.Parameters[parameterName].Value;
        }
        #endregion
        #region Methods for Generating SqlCommand
        private SqlCommand PrepareCommand(CommandType commandType, string commandText)
        {
            if (connection == null)
            {
                //string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                string _connectionString = Properties.Settings.Default.ConnectionString;
                connection = new SqlConnection(_connectionString);
            }
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection.Open();
            }
            SqlCommand command = new SqlCommand(commandText, connection);
            command.CommandType = commandType;
            return command;
        }
        public SqlCommand GetStoreProcedureCommand(string spname)
        {
            return PrepareCommand(CommandType.StoredProcedure, spname);
        }
        public SqlCommand GetSqlQueryCommand(string query)
        {
            return PrepareCommand(CommandType.Text, query);
        }
        #endregion
        #region Database Related Command Methods
        public int ExecuteNonQuery(SqlCommand command)
        {
            return command.ExecuteNonQuery();
        }
        public object ExecuteScalar(SqlCommand command)
        {
            return command.ExecuteScalar();
        }
        public SqlDataReader ExecuteReader(SqlCommand command)
        {
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public SqlDataReader ExecuteReader(SqlCommand command, CommandBehavior commandBehavior)
        {
            return command.ExecuteReader(commandBehavior);
        }
        public DataTable LoadDataTable(SqlCommand command, string tableName)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(command))
            {
                DataTable dt = new DataTable(tableName);
                da.Fill(dt);
                return dt;
            }
        }
        public DataSet LoadDataSet(SqlCommand command, string[] tableNames)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(command))
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    ds.Tables[i].TableName = tableNames[i];
                }
                return ds;
            }
        }
        public DataSet LoadDataSet(SqlCommand command)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(command))
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
        #endregion
        #region Methods for Transaction Handling
        private SqlTransaction PrepareTransaction(IsolationLevel isolationLevel)
        {
            if (connection == null)
            {
                string _connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
                connection = new SqlConnection(_connectionString);
            }
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection.Open();
            }
            return connection.BeginTransaction(isolationLevel);
        }
        public SqlTransaction BeginTransaction()
        {
            return PrepareTransaction(IsolationLevel.ReadCommitted);
        }
        public SqlTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return PrepareTransaction(isolationLevel);
        }
        public void Commit(SqlTransaction transaction)
        {
            if (transaction != null)
                transaction.Commit();
        }
        public void RollBack(SqlTransaction transaction)
        {
            if (transaction != null)
                transaction.Rollback();
        }
        #endregion
        #region IDisposable Members
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
        ~SQLHelper()
        {
            Dispose();
        }
    }

}
