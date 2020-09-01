using System.Data;
using System.Data.SqlClient;

namespace Core.Data.DataAccessProvider
{
	/// <summary>
	/// The SQLDataAccessLayer contains the data access layer for Microsoft SQL Server. 
	/// This class implements the abstract methods in the DataAccessLayerBaseClass class.
	/// </summary>
    public class SqlDataAccessLayer : DataProviderBaseClass
	{
		// Provide class constructors
		public SqlDataAccessLayer() {}
		public SqlDataAccessLayer(string connectionString) { ConnectionString = connectionString;}

		// DataAccessLayerBaseClass Members
		internal override IDbConnection GetDataProviderConnection()
		{
			return new SqlConnection();
		}
		internal override IDbCommand GeDataProviderCommand()
		{
			return new SqlCommand();
		}

		internal override IDbDataAdapter GetDataProviderDataAdapter()
		{
			return new SqlDataAdapter();            
		}        
	}
}
