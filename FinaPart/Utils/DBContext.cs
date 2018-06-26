using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace FinaPart.Utils
{
    public  class DBContext : IDisposable
    {
        private bool _disposed = false;

        public string ConnectionString { get; set; }
        public int Timeout { get; set; }
        public string ErrorEx { get; private set; }


        public DBContext(string connection_string)
        {
            this.ConnectionString = connection_string;
            this.Timeout = 1200;
        }

        public bool IsConnectionValid()
        {
            ErrorEx = null;
            try
            {
                using (SqlConnection _connection = new SqlConnection(this.ConnectionString))
                {
                    _connection.Open();
                    _connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.ErrorEx = ex.Message;
                return false;
            }
        }

        public Nullable<int> ExecuteSql(string sql, params SqlParameter[] sql_parameters)
        {
            ErrorEx = null;
            int? _result = null;
            sql_parameters.ToList().ForEach(a => a.Value = a.Value ?? DBNull.Value);
            try
            {
                using (SqlConnection _connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand _command = new SqlCommand(sql, _connection))
                    {
                        _command.CommandTimeout = this.Timeout;
                        _command.Parameters.AddRange(sql_parameters);
                        _connection.Open();
                        _result = _command.ExecuteNonQuery();
                        _connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _result = null;
                this.ErrorEx = ex.Message;
            }
            return _result;
        }

        public List<T> GetList<T>(string sql, params SqlParameter[] sql_parameters)
        {
            ErrorEx = string.Empty;
            List<T> _result = new List<T>();
            sql_parameters.ToList().ForEach(a => a.Value = a.Value ?? DBNull.Value);
            try
            {
                using (SqlConnection _connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand _command = new SqlCommand(sql, _connection))
                    {
                        _command.CommandTimeout = this.Timeout;
                        _command.Parameters.AddRange(sql_parameters);
                        _connection.Open();
                        using (SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (typeof(T).IsValueType || typeof(T) == typeof(string))
                            {
                                while (_reader.Read())
                                    _result.Add((T)_reader[0]);
                            }
                            else
                            {
                                T obj = default(T);
                                var _columns = _reader.GetSchemaTable().AsEnumerable().Select(r => r.Field<string>("ColumnName")).ToList();
                                var _existing = Activator.CreateInstance<T>().GetType().GetProperties().Where(a => _columns.Contains(a.Name, StringComparer.InvariantCultureIgnoreCase));
                                while (_reader.Read())
                                {
                                    obj = Activator.CreateInstance<T>();
                                    foreach (PropertyInfo prop in _existing)
                                    {
                                        if (!object.Equals(_reader[prop.Name], DBNull.Value))
                                            prop.SetValue(obj, _reader[prop.Name], null);
                                    }
                                    _result.Add(obj);
                                }
                            }
                        }
                        _connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _result = null;
                this.ErrorEx = ex.Message;
            }
            return _result;
        }

        public List<Dictionary<string, object>> GetTableDictionary(string sql, params SqlParameter[] sql_parameters)
        {
            ErrorEx = string.Empty;
            List<Dictionary<string, object>> _result = new List<Dictionary<string, object>>();
            sql_parameters.ToList().ForEach(a => a.Value = a.Value ?? DBNull.Value);
            try
            {
                using (SqlConnection _connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand _command = new SqlCommand(sql, _connection))
                    {
                        _command.CommandTimeout = this.Timeout;
                        _command.Parameters.AddRange(sql_parameters);
                        _connection.Open();
                        using (SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            var cols = _reader.GetSchemaTable().AsEnumerable().Select(r => r["ColumnName"]);
                            while (_reader.Read())
                            {
                                Dictionary<string, object> item = new Dictionary<string, object>();
                                foreach (var _c in cols)
                                    item.Add(_c.ToString(), _reader[_c.ToString()]);
                                _result.Add(item);
                            }
                        }
                        _connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _result = null;
                ErrorEx = ex.Message;
            }
            return _result;
        }

        public Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(string sql, params SqlParameter[] sql_parameters)
        {
            ErrorEx = string.Empty;
            Dictionary<TKey, TValue> _result = new Dictionary<TKey, TValue>();
            sql_parameters.ToList().ForEach(a => a.Value = a.Value ?? DBNull.Value);
            try
            {
                using (SqlConnection _connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand _command = new SqlCommand(sql, _connection))
                    {
                        _command.CommandTimeout = this.Timeout;
                        _command.Parameters.AddRange(sql_parameters);
                        _connection.Open();
                        using (SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (_reader.Read())
                            {
                                if (!_result.ContainsKey((TKey)_reader[0]))
                                    _result.Add((TKey)_reader[0], (TValue)_reader[1]);
                            }
                        }
                        _connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _result = null;
                ErrorEx = ex.Message;
            }
            return _result;
        }

        public Nullable<KeyValuePair<TKey, TValue>> GetKeyValuePair<TKey, TValue>(string sql, params SqlParameter[] sql_parameters)
        {
            ErrorEx = string.Empty;
            Nullable<KeyValuePair<TKey, TValue>> _result = new Nullable<KeyValuePair<TKey, TValue>>();
            sql_parameters.ToList().ForEach(a => a.Value = a.Value ?? DBNull.Value);
            try
            {
                using (SqlConnection _connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand _command = new SqlCommand(sql, _connection))
                    {
                        _command.CommandTimeout = this.Timeout;
                        _command.Parameters.AddRange(sql_parameters);
                        _connection.Open();
                        using (SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (_reader.Read())
                            {
                                _result = new KeyValuePair<TKey, TValue>((TKey)_reader[0], (TValue)_reader[1]);
                                break;
                            }
                        }
                        _connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _result = null;
                ErrorEx = ex.Message;
            }
            return _result;
        }

        public Nullable<T> GetScalar<T>(string sql, params SqlParameter[] sql_parameters) where T : struct
        {
            ErrorEx = null;
            Nullable<T> _result = null;
            sql_parameters.ToList().ForEach(a => a.Value = a.Value ?? DBNull.Value);
            try
            {
                using (SqlConnection _connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand _command = new SqlCommand(sql, _connection))
                    {
                        _command.CommandTimeout = this.Timeout;
                        _command.Parameters.AddRange(sql_parameters);
                        _connection.Open();
                        object _res = _command.ExecuteScalar();
                        _connection.Close();
                        if (_res != null)
                            _result = (T)Convert.ChangeType(_res, typeof(T));
                    }
                }
            }
            catch (Exception ex)
            {
                _result = null;
                this.ErrorEx = ex.Message;
            }
            return _result;
        }

        public string GetString(string sql, params SqlParameter[] sql_parameters)
        {
            ErrorEx = string.Empty;
            string _result = null;
            sql_parameters.ToList().ForEach(a => a.Value = a.Value ?? DBNull.Value);
            try
            {
                using (SqlConnection _connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand _command = new SqlCommand(sql, _connection))
                    {
                        _command.CommandTimeout = this.Timeout;
                        _command.Parameters.AddRange(sql_parameters);
                        _connection.Open();
                        object _res = _command.ExecuteScalar();
                        _connection.Close();
                        if (_res != null)
                            _result = Convert.ToString(_res);
                    }
                }
            }
            catch (Exception ex)
            {
                _result = null;
                this.ErrorEx = ex.Message;
            }
            return _result;
        }

        public DataTable GetTableData(string sql, params SqlParameter[] sql_parameters)
        {
            ErrorEx = string.Empty;
            DataTable _result = new DataTable("temp");
            sql_parameters.ToList().ForEach(a => a.Value = a.Value ?? DBNull.Value);
            try
            {
                using (SqlConnection _connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand _command = new SqlCommand(sql, _connection))
                    {
                        _command.CommandTimeout = this.Timeout;
                        _command.Parameters.AddRange(sql_parameters);
                        _connection.Open();
                        using (SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            _result.Load(_reader);
                        }
                        _connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _result = null;
                this.ErrorEx = ex.Message;
            }
            return _result;
        }


        public bool isDistributionStore(int storeID)
        {

            int res = GetScalar<int>("SELECT COUNT(id) FROM book.Stores WHERE id= " + storeID + " AND path LIKE '0#1#4%'").Value;
            return res > 0 ? true : false;
        }

        public bool IsCompanyVAT()
        {
            bool isCompanyVat = false;
            bool.TryParse(GetString("SELECT top 1 ISNULL(vat,0) FROM book.Companies"), out isCompanyVat);
            return isCompanyVat;
        }

        public bool IsOperationVAT(int general_id)
        {
            return Convert.ToBoolean(GetScalar<double>("SELECT vat FROM doc.GeneralDocs WHERE id=" + general_id));
        }

        public double GetProductPriceByID(string productID, string priceID)
        {
            double price = 0;

            if (priceID != "1")
                double.TryParse(GetString(@"SELECT (CASE WHEN (SELECT  value FROM config.params WHERE name='DiscountPrice')='1'  AND getdate() BETWEEN  ISNULL(d.discount_start_date,0) AND ISNULL(discount_end_date,999999) AND ISNULL(d.discount_manual_val,0)>0 THEN  d.discount_manual_val ELSE   d.manual_val END)
                                                FROM book.ProductPrices as d WHERE d.price_id = " + priceID + " AND d.product_id = " + productID), out price);
            else
            {
                try
                {
                    price = GetProductInCostGEL(int.Parse(productID), 0, true);
                }
                catch
                {
                    price = 0;
                }

            }
            return price;
        }

        private double GetProductInCostGEL(int product_id, int store_id, bool include_vat)
        {
            string str = "self_cost";
            if (include_vat)
                str = "price";
            string sql = "SELECT TOP 1 a." + str + " FROM   doc.ProductsFlow a " +
                        " INNER JOIN doc.GeneralDocs c ON a.general_id = c.id" +
                        " WHERE c.doc_type = 16 AND ISNULL(c.is_deleted,0) = 0 AND  a.coeff>0 AND a.is_order = 0 AND a.is_expense=0" +
                            " AND a.product_id = " + product_id +
                            "    AND ( (" + store_id + " =0 AND a.is_move=0)  OR a.store_id = " + store_id + "  ) ORDER BY c.tdate DESC";
            return GetScalar<double>(sql).Value;
        }

        public int? GetDistributorParentWaybillID(int store_id)
        {
            int? res = GetScalar<int>("SELECT TOP 1 ISNULL(m.waybill_id,0) AS id FROM doc.ProductShipping m INNER JOIN doc.GeneralDocs gd ON gd.id= m.general_id WHERE m.waybill_status=1 AND ISNULL(m.waybill_id,0)>0 AND gd.param_id1=" + store_id + " order by gd.tdate DESC");
            return res;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
                return;
            if (disposing)
            {
                ConnectionString = null;
                SqlConnection.ClearAllPools();
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DBContext()
        {
            Dispose(false);
        }

    }


}



