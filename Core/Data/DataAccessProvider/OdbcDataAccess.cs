using System.Data;
using System.Data.Odbc;

namespace Core.Data.DataAccessProvider
{
	/// <summary>
	/// The SQLDataAccessLayer contains the data access layer for Odbc data provider. 
	/// This class implements the abstract methods in the DataAccessLayerBaseClass class.
	/// </summary>
    public class OdbcDataAccessLayer : DataProviderBaseClass
	{
		// Provide class constructors
		public OdbcDataAccessLayer() {}
		public OdbcDataAccessLayer(string connectionString) { ConnectionString = connectionString;}

		// DataAccessLayerBaseClass Members
		internal override IDbConnection GetDataProviderConnection()
		{
			return new OdbcConnection();
		}
		internal override IDbCommand GeDataProviderCommand()
		{
			return new OdbcCommand();
		}

		internal override IDbDataAdapter GetDataProviderDataAdapter()
		{
			return new OdbcDataAdapter();
		}
	}
}
