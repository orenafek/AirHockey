using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using System.Data.SqlClient;
using System.Web;

namespace AirHockeyMobileService
{
    public class ADO_Data_Service
    {
        private static SqlConnection _conn;
        private static SqlTransaction _trans;
        private static SqlDataAdapter _adapter;
        private static SqlCommand _cmd;
        private static SqlParameterCollection _paramColl;
        private static SqlParameter _param;
        private static DataSet _ds;

        static ADO_Data_Service()
        {
            _conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _ds = new DataSet();

        }
        public static DataView getView(string viewName)
        {
            try
            {
                _cmd = _conn.CreateCommand();
                _cmd.CommandText = "View" + viewName;
                _cmd.CommandType = CommandType.StoredProcedure;
                _adapter = new SqlDataAdapter(_cmd);
                _adapter.Fill(_ds);
                return new DataView(_ds.Tables[0]);
            }

            catch (SqlException e)
            {
                return null;
            }

            return null;

        }
    

    public static void updateUser(int userID)
    {
            _cmd = _conn.CreateCommand();
            _cmd.CommandText = "UPDATE Users SET id='a' AND Pass='999' WHERE id='77'";

            try
            {
                _conn.Open();
                _cmd.ExecuteNonQuery();
                
            }

            catch(SqlException e)
            {

            }

            finally
            {
                if(_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
    }
}