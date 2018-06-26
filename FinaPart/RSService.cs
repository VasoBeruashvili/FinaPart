using System;
using System.Data;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using FinaPart.Utils;
using System.Xml.Linq;

namespace FinaPart
{
    public enum WaybillMode
    { CreateActivate, Close, Cancel }

    public class RSService
    {
        public string ErrorEx { get; private set; }
        private RSServiceReference.NtosServiceSoapClient m_Service = new RSServiceReference.NtosServiceSoapClient();
        private WaybillServiceReference.WayBillsSoapClient m_WaybillService = new WaybillServiceReference.WayBillsSoapClient();
        string m_su = "", m_sp = "";
        public int m_UserID = 0;
        public int m_UnID = 0;
        bool m_bEnabled = false;
        string m_Code = "";
        Hashtable m_SqlParams = new Hashtable();
        WaybillMode waybill_mode;



        public bool Initialize(string code)
        {
            using (FinaContext context = new FinaContext())
            {
                m_Code = code;
                string value = context.Params.Where(p => p.Name == "RSUserID").Select(c => c.Value).First();

                m_UserID = Convert.ToInt32(context.Params.Where(p => p.Name == "RSUserID").Select(c => c.Value).First());
                m_UnID = Convert.ToInt32(context.Params.Where(p => p.Name == "RSUnId").Select(c => c.Value).First());

                UpdateServiceData();
                return true;
            }
        }

        public bool UpdateServiceData()
        {
            using (FinaContext context = new FinaContext())
            {
                m_bEnabled = false;
                m_su = context.Params.Where(p => p.Name == "RSServiceUsername").Select(c => c.Value).First();
                m_sp = context.Params.Where(p => p.Name == "RSServicePassword").Select(c => c.Value).First();

                if (m_UnID <= 0)
                {
                    m_su = "";
                    m_sp = "";
                    return false;
                }
                m_bEnabled = true;
                return true;
            }
        }
        
        public RSServiceReference.NtosServiceSoapClient GetService()
        {
            return m_Service;
        }
        
        public WaybillServiceReference.WayBillsSoapClient GetWaybillService()
        {
            return m_WaybillService;
        }
        
        public string GetSU()
        {
            return m_su;
        }
        
        public string GetSP()
        {
            return m_sp;
        }

        public bool IsEnabled()
        {
            return m_bEnabled && m_UnID > 0;
        }

        public bool isPayerVat(string code)
        {
            string name = "";
            int un_id = GetContragentUserID(code, out name);
            return GetWaybillService().is_vat_payer(m_su, m_sp, un_id);
        }

        public string GetContragentNameByCode(string code)
        {
            return GetWaybillService().get_name_from_tin(m_su, m_sp,code);
        }

        public int GetContragentUserID(string code, out string name)
        {
            name = "";
            if (!IsEnabled())
                return 0;

            return GetService().get_un_id_from_tin(m_UserID, code, m_su, m_sp, out name);
        }

        public DataTable GetSellerInvoices(DateTime s_date, DateTime s_op_date, DateTime e_date, DateTime e_op_date, string invoice_num, string sa_ident_no)
        {
            return GetService().get_seller_invoices(m_UserID, m_UnID, s_date, e_date, s_op_date, e_op_date, invoice_num, sa_ident_no, string.Empty, string.Empty, m_su, m_sp);

        }

        public DataTable GetBuyerInvoices(DateTime s_date, DateTime e_date, DateTime s_op_date, DateTime e_op_date, string invoice_num, string sa_ident_no)
        {

            return GetService().get_buyer_invoices(m_UserID, m_UnID, s_date, e_date, s_op_date, e_op_date, invoice_num, sa_ident_no, string.Empty, string.Empty, m_su, m_sp);

        }

        public DataTable GetFakturaItems(int id)
          {

              return GetService().get_invoice_desc(m_UserID, id, m_su, m_sp);

          }

        public DataTable GetFakturaWaybills(int id)
          {
             
              return GetService().get_ntos_invoices_inv_nos(m_UserID, id, m_su, m_sp);

          }

        public int AddInvoice(DateTime tdate, string code)
        {
            if (!IsEnabled())
                return 0;
            string name;
            int buyer_id = GetContragentUserID(code, out name);
            if (buyer_id <= 0)
                return 0;
            int invoice_id = 0;
            bool res = GetService().save_invoice(m_UserID, ref invoice_id, tdate, m_UnID, buyer_id, "", tdate, 0, m_su, m_sp);
            if (!res)
                return invoice_id;
            return invoice_id;
        }

        public bool ActivateWaybill(int id)
        {
            if (!IsEnabled())
                return false;
            string sql = "";

            int waybill_id = GetWaybillIDByGeneralID(id);
            if (waybill_id <= 0)
                return false;

            string doc_num = GetWaybillService().send_waybill(m_su, m_sp, waybill_id);
            if (doc_num == "-1")
                return false;

            using (FinaContext _db = new FinaContext())
            {
                sql = "UPDATE doc.GeneralDocs SET doc_num = " + doc_num + " WHERE id=" + id;
                _db.Database.ExecuteSqlCommand(sql);
            }

            if (!SetWaybillStatus(id))
                return false;
            //update status

            return true;

        }

        public int CloseWaybill(int id)
        {
            if (!IsEnabled())
                return -1;
            waybill_mode = WaybillMode.Close;
            int waybill_id = GetWaybillIDByGeneralID(id);
            if (waybill_id <= 0)
                return -1;
            int res = GetWaybillService().close_waybill(m_su, m_sp,waybill_id);
            if (res == 1)
            {
                if (!SetWaybillStatus(id))
                    return -1;
                return res;
            }
            return res;
          
        }

        private string GetTableNameByGeneralID(int id)
        {
            using (FinaContext _db = new FinaContext())
            {
                int? doc_type = _db.GeneralDocs.Where(g => g.Id == id).Select(g => g.DocType).FirstOrDefault();
                string res = "doc.ProductMove";
                if (doc_type == 21 || doc_type == 13)
                    res = "doc.ProductOut";
                else if (doc_type == 32)
                    res = "doc.VendorReturns";
                else if (doc_type == 77)
                    res = "doc.ProductShipping";
                else if (doc_type == 9)
                    res = "doc.CustomerReturns";
                else if (doc_type == 107)
                    res = "doc.AutoServiceRequest";
                else if (doc_type == 97)
                    res = "doc.InventoryRent";
                else if (doc_type == 99)
                    res = "doc.InventoryRentReturn";

                return res;
            }
        }
       
        public int GetWaybillIDByGeneralID(int id)
        {
            string str = GetTableNameByGeneralID(id);
            
            using (FinaContext _db = new FinaContext())
            {
                string sql = "SELECT ISNULL(waybill_id,0) AS waybill_id FROM " + str + " WHERE general_id=" + id;
                return _db.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
        }
       
        public int CancelWaybill(int id)
        {
            if (!IsEnabled())
                return -1;
            waybill_mode = WaybillMode.Cancel;
            int waybill_id = GetWaybillIDByGeneralID(id);
            if (waybill_id <= 0)
                return -1;

            int res = GetWaybillService().ref_waybill(m_su, m_sp, waybill_id);
            if (res == 1)
            {
                if (!SetWaybillStatus(id))
                    return -1;
                return res;
            }

            return res;

        }
        
        public int GetWayBillStatus(int general_id)
        {
            string doc_str = GetTableNameByGeneralID(general_id);

            using (FinaContext _db = new FinaContext())
            {
                string sql = "SELECT ISNULL(waybill_status,-1) AS waybill_status FROM " + doc_str + " WHERE general_id=" + general_id;
                return _db.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
        }
      
        public bool SetWaybillStatus(int general_id)
        {
            using (FinaContext _db = new FinaContext())
            {
                string doc_str = GetTableNameByGeneralID(general_id);

                string wayBillnum = "";
                int status_id = 0;
                if (!GetWaybillInfo(GetWaybillIDByGeneralID(general_id), out wayBillnum, out status_id))
                    return false;

                string sql = "UPDATE " + doc_str + " SET waybill_status = " + status_id + ", waybill_num='" + wayBillnum + "' WHERE general_id=" + general_id;
                _db.Database.ExecuteSqlCommand(sql);
                //update new number
                sql = "UPDATE doc.Generaldocs SET waybill_num=@WAYBILLNUM WHERE id=@ID";
                m_SqlParams.Clear();
                m_SqlParams.Add("@ID", general_id);
                m_SqlParams.Add("@WAYBILLNUM", wayBillnum);

                _db.Database.ExecuteSqlCommand(sql, new SqlParameter("@ID", general_id), new SqlParameter("@WAYBILLNUM", wayBillnum));

                return true;
            }
        }

        private XmlNode AddElement(string tagName, string textContent, XmlNode parent)
        {
            XmlNode node = parent.OwnerDocument.CreateElement(tagName);
            parent.AppendChild(node);

            if (textContent != null)
            {
                XmlNode content = parent.OwnerDocument.CreateTextNode(textContent);
                node.AppendChild(content);
            }

            return node;
        }

        public string GetMyIP()
        {
            string str = "";
            try
            {
             str=   GetWaybillService().what_is_my_ip();
            }
            catch
            {
                str = "";
            }
            return str;
        }

        public int CreateWaybill(string connString, int id, out string doc_num, out string error_str)
        {
            using (DBContext _db = new DBContext(connString))
            {
                error_str = "ERROR";
                doc_num = "";
                if (!IsEnabled())
                    return -1;
                int waybill_id = GetWaybillIDByGeneralID(id);
                if (waybill_id <= 0)
                    waybill_id = 0;
                if (waybill_id > 0)
                {
                    error_str = "ზედნადები უკვე აქტივირებულია";
                    return -1;
                }
                waybill_mode = WaybillMode.CreateActivate;
                DataTable doc_data = null;
                DataRow doc_row = null;
                string doc_str = GetTableNameByGeneralID(id);
                bool is_distributionove = false;
                bool is_move = false;
                bool is_return = false;
                string flow_name = "doc.productsflow";
                bool is_free = false;
                if (doc_str == "doc.ProductOut")
                {
                    if (_db.GetScalar<int>("SELECT ISNULL(pay_type,0) FROM doc.ProductOut WHERE general_id=" + id).Value == 5)
                        is_free = true;
                }
                else
                    if (doc_str == "doc.AutoServiceRequest")
                {
                    if (_db.GetScalar<int>("SELECT ISNULL(pay_type,0) FROM doc.AutoserviceRequest WHERE general_id=" + id).Value == 5)
                        is_free = true;
                }

                if (doc_str == "doc.ProductShipping")
                {
                    is_distributionove = true;
                    is_return = false;
                    is_move = false;
                    flow_name = "doc.ProductShippingFlow";
                }
                if (doc_str == "doc.ProductMove" || doc_str == "doc.InventoryRent" || doc_str == "doc.InventoryRentReturn")
                {
                    is_distributionove = false;
                    is_return = false;
                    is_move = true;
                }
                if (doc_str == "doc.CustomerReturns")
                {
                    is_distributionove = false;
                    is_return = true;
                    is_move = false;
                }
                string sql = "SELECT ISNULL(p.waybill_status,0) AS waybill_status, gd.param_id1, gd.param_id2 FROM " + doc_str + " p INNER JOIN doc.GeneralDocs gd ON gd.id=p.general_id WHERE p.general_id=" + id;
                doc_data = _db.GetTableData(sql);
                doc_row = doc_data.Rows[0];
                string parent_id = "";
                if (!is_distributionove)
                {
                    int store_id = int.Parse(doc_row["param_id2"].ToString());
                    if (_db.isDistributionStore(store_id))
                    {
                        if (doc_str == "doc.CustomerReturns")
                        {
                            error_str = "ბორტზე დაბრუნება შეუძლებელია";
                            return -1;
                        }
                        int? id2 = _db.GetDistributorParentWaybillID(store_id);
                        if (id2 > 0)
                            parent_id = id2.ToString();
                        else
                        {
                            error_str = "აქტიური მშობელი ზედნადები ვერ მოიძებნა";
                            return -1;
                        }
                    }
                }
                if (doc_str == "doc.ProductMove" || doc_str == "doc.InventoryRent" || doc_str == "doc.InventoryRentReturn")
                {
                    int store_id1 = int.Parse(doc_row["param_id1"].ToString());
                    int store_id2 = int.Parse(doc_row["param_id2"].ToString());
                    if (_db.isDistributionStore(store_id1) ||
                        _db.isDistributionStore(store_id2))
                        return -12345678;
                }

                int waybill_status = 0;//
                string price_id = _db.GetString("SELECT value FROM config.Params WHERE name='CheckPrices'");
                sql = @"
                    SELECT g.rate, (g.amount*g.rate) AS full_amount, p.id as product_id, 
                    CASE p.rs_code_type WHEN '1' THEN p.partnumber WHEN '2' THEN p.code WHEN '3' THEN (SELECT top 1 barcode FROM book.ProductBarCodes WHERE product_id=p.id) END  AS product_code, 
                    p.vat, p.vat_type, p.name, ISNULL(p.excise,0) AS excise,  
                    ISNULL(p.process_type,0) AS process_type, u.full_name AS unit_name, 
                           w.waybill_type, w.waybill_cost, w.delivery_date, w.driver_name, w.transport_begin_date, w.activate_date, w.transport_cost_payer, w.transport_type_id, w.sender_name, w.reciever_name, w.comment,
                           w.transp_start_place, w.transp_end_place,  w.transporter_IdNum, ISNULL(w.is_foreign,0) AS is_foreign,  w.transporter_name as driver_name, w.transport_number, w.transport_text,
                           c.name AS contragentName, c.code AS contragentCode, c.address AS contragentAddress, ISNULL(c.is_resident,0) AS is_resident, w.category_id, 
                    ISNULL(u.rs_id,99) AS rs_id,
                    pf.price * (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) AS price, 
                    pf.amount / (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) AS amount 
                    FROM doc.ProductsFlow pf 
                    INNER JOIN doc.generaldocs g ON g.id= pf.general_id
                    INNER JOIN ";
                sql = sql + doc_str;
                sql = sql + @" w on w.general_id=pf.general_id
                    INNER JOIN book.Products p ON p.id= pf.product_id 
                    INNER JOIN book.contragents c on c.id=g.param_id1
                    INNER JOIN book.Units u ON u.id = p.special_unit_id  --CASE  @unit_type WHEN '1' THEN pf.unit_id WHEN '2' THEN  p.unit_id WHEN '3' THEN p.special_unit_id END
                    WHERE 
                    (p.path LIKE '0#1%' OR p.path LIKE '0#3%') AND g.status_id=CASE g.doc_type WHEN 107 THEN 5 ELSE g.status_id END AND
                    pf.visible=1  AND pf.general_id=" + id + " ORDER BY pf.id";


                if (is_distributionove)
                {
                    sql = @"
                        SELECT g.rate, (g.amount*g.rate) AS full_amount, p.id as product_id,  
                        CASE  p.rs_code_type WHEN '1' THEN p.partnumber WHEN '2' THEN p.code WHEN '3' THEN (SELECT top 1 barcode FROM book.ProductBarCodes WHERE product_id=p.id) END  AS product_code,  
                        p.vat, p.vat_type, p.name,  ISNULL(p.excise,0) AS excise,   ISNULL(p.process_type,0) AS process_type, u.full_name AS unit_name, 
                           w.waybill_type, w.waybill_cost, w.delivery_date, w.driver_name,
                           w.transport_begin_date, w.activate_date, w.transport_cost_payer, w.transport_type_id,
                           w.transp_start_place, w.transp_end_place,    w.transport_number,
                           w.transporter_IdNum,ISNULL(w.is_foreign,0) AS is_foreign,w.sender_name, w.reciever_name, w.comment, w.transport_text, 0 AS category_id, 
                    ISNULL(u.rs_id,99) AS rs_id, 
                    pf.price * (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) AS price,
                    pf.rest /(SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) AS amount 
                    FROM doc.ProductShippingFlow pf 
                    INNER JOIN doc.generaldocs g ON g.id= pf.general_id
                    INNER JOIN doc.ProductShipping w on w.general_id=pf.general_id
                    INNER JOIN book.Products p ON p.id= pf.product_id 
                    INNER JOIN book.Units u ON u.id=p.special_unit_id
                    WHERE (p.path LIKE '0#1%' OR p.path LIKE '0#3%') AND  pf.general_id=" + id + " ORDER BY pf.id";
                }

                if (is_move)
                {
                    sql = @"
                        SELECT g.rate, (g.amount*g.rate) AS full_amount, p.id as product_id, 
                        CASE p.rs_code_type WHEN '1' THEN p.partnumber WHEN '2' THEN p.code WHEN '3' THEN (SELECT top 1 barcode FROM book.ProductBarCodes WHERE product_id=p.id) END  AS product_code, 
                        p.vat, p.vat_type, p.name,  ISNULL(p.excise,0) AS excise,   ISNULL(p.process_type,0) AS process_type, u.full_name AS unit_name, 
                        w.waybill_type, w.waybill_cost, w.delivery_date, w.driver_name,
                        w.transport_begin_date, w.activate_date, w.transport_cost_payer, w.transport_type_id,
                        w.transp_start_place, w.transp_end_place,    w.transport_number,
                        w.transporter_IdNum,ISNULL(w.is_foreign,0) AS is_foreign, w.sender_name, w.reciever_name, w.comment, w.transport_text, w.category_id, 
                    ISNULL(u.rs_id,99) AS rs_id,
                    pf.price * (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) AS price, --CASE  @unit_type WHEN '1' THEN (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=pf.unit_id) WHEN '2' THEN 1 WHEN '3' THEN (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) END AS price,
                    pf.amount / (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) AS amount -- CASE @unit_type WHEN '1' THEN (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=pf.unit_id) WHEN '2' THEN 1 WHEN '3' THEN (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) END AS amount
                    FROM " + flow_name + " pf " +
                         @" INNER JOIN doc.generaldocs g ON g.id= pf.general_id
                    INNER JOIN ";
                    sql = sql + doc_str;
                    sql = sql + @" w on w.general_id=pf.general_id
                    INNER JOIN book.Products p ON p.id= pf.product_id 
                    INNER JOIN book.Units u ON u.id =  p.special_unit_id --CASE  @unit_type WHEN '1' THEN pf.unit_id WHEN '2' THEN  p.unit_id WHEN '3' THEN p.special_unit_id END
                    WHERE (p.path LIKE '0#1%' OR p.path LIKE '0#3%') AND pf.visible=1 AND pf.general_id=" + id + " ORDER BY pf.id";
                }
                if (is_return)
                {
                    sql = @"DECLARE @code_type AS VARCHAR = (SELECT value FROM config.params WHERE name='WaybillCodeMode')
                        SELECT g.rate, (g.amount*g.rate) AS full_amount, p.id as product_id, 
                        CASE p.rs_code_type WHEN '1' THEN p.partnumber WHEN '2' THEN p.code WHEN '3' THEN (SELECT top 1 barcode FROM book.ProductBarCodes WHERE product_id=p.id) END  AS product_code, 
                        p.vat, p.vat_type, p.name,  ISNULL(p.excise,0) AS excise,   ISNULL(p.process_type,0) AS process_type, u.full_name AS unit_name, 
                        w.waybill_type, w.waybill_cost, w.delivery_date, w.driver_name,
                        w.transport_begin_date, w.activate_date, w.transport_cost_payer, w.transport_type_id,
                        w.transp_start_place, w.transp_end_place,    w.transport_number,ISNULL(w.is_foreign,0) AS is_foreign,
                        w.transporter_IdNum, w.sender_name, w.reciever_name, w.comment, w.transport_text,  0 AS category_id, 
                        c.name AS contragentName, c.code AS contragentCode, c.address AS contragentAddress,  ISNULL(c.is_resident,0) AS is_resident,
                    ISNULL(u.rs_id,99) AS rs_id, pf.amount / (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) AS amount, 
                    pf.price * (SELECT ISNULL(MIN(amount),1)  FROM book.ProductUnits WHERE product_id=pf.product_id AND unit_id=p.special_unit_id) AS price FROM " + flow_name + " pf " +
                         @" INNER JOIN doc.generaldocs g ON g.id= pf.general_id
                    INNER JOIN ";
                    sql = sql + doc_str;
                    sql = sql + @" w on w.general_id=pf.general_id
                    INNER JOIN book.Products p ON p.id= pf.product_id 
                    INNER JOIN book.contragents c on c.id=g.param_id1
                    INNER JOIN book.Units u ON u.id=p.special_unit_id WHERE 
                    (p.path LIKE '0#1%' OR p.path LIKE '0#3%') AND 
                    pf.visible=1 AND pf.general_id=" + id + " ORDER BY pf.id";
                }

                DataTable data = _db.GetTableData(sql);
                if (data == null || data.Rows.Count <= 0)
                    return -1;
                DateTime dd = Convert.ToDateTime(data.Rows[0]["delivery_date"].ToString());
                string delivery_date = dd.Year + "-" + dd.Month + "-" + dd.Day + "T" + dd.Hour + ":" + dd.Minute + ":" + dd.Second;
                dd = Convert.ToDateTime(data.Rows[0]["transport_begin_date"]);
                double rate = Convert.ToDouble(data.Rows[0]["rate"]);
                if (doc_str == "doc.CustomerReturns")
                    rate = 1;

                List<Dictionary<string, object>> woods = new List<Dictionary<string, object>>();
                byte category = Convert.ToByte(data.Rows[0]["category_id"]);
                if (category == 1)
                    woods = _db.GetTableDictionary("SELECT name, tdate, describ FROM doc.WoodFlow WHERE general_id = " + id);

                string transport_begin_date = dd.Year + "-" + dd.Month + "-" + dd.Day + "T" + dd.Hour + ":" + dd.Minute + ":" + dd.Second;
                string driver_name = data.Rows[0]["driver_name"].ToString();
                string sender = data.Rows[0]["sender_name"].ToString();
                string reciever = data.Rows[0]["reciever_name"].ToString();
                string comment = data.Rows[0]["comment"].ToString();
                double price = 0.00;
                string trans_id = Convert.ToString(data.Rows[0]["transport_type_id"]);
                string transport_text = Convert.ToString(data.Rows[0]["transport_text"]);
                string transporter_tin = "";
                bool is_excise = _db.GetString("SELECT value FROM config.Params WHERE name='IsExcise'") == "1";

                if (trans_id == "7")
                {
                    transporter_tin = transport_text;
                    transport_text = "";
                }

                XmlDocument doc = new XmlDocument();
                XmlNode rot = doc.CreateElement("root");
                doc.AppendChild(rot);
                XmlNode WAYBILLNode = AddElement("WAYBILL", null, rot);
                AddElement("SUB_WAYBILLS", string.Empty, WAYBILLNode);
                XmlNode GOODS_LISTNode = AddElement("GOODS_LIST", null, WAYBILLNode);
                foreach (DataRow row in data.Rows)
                {
                    string vat = row["vat"].ToString().ToLower();
                    string vat_type = row["vat_type"].ToString();
                    string res_vat = "0";
                    if (vat == "false")
                    {
                        if (vat_type == "0")//nulovani
                            res_vat = "1";
                        else if (vat_type == "1")//daubegravi
                            res_vat = "2";
                    }
                    if (doc_str != "doc.ProductMove" && doc_str != "doc.InventoryRent")//nika to fix in service
                        if (res_vat == "0" && _db.IsCompanyVAT() && !_db.IsOperationVAT(id))
                            res_vat = "1";
                    price = !is_move ? Convert.ToDouble(row["price"]) : (int.Parse(price_id) <= 2 ? Convert.ToDouble(row["price"]) : _db.GetProductPriceByID((row["product_id"].ToString()), price_id));
                    if (doc_str == "doc.ProductMove" || doc_str == "doc.InventoryRent")
                        price = 0.0;
                    double amount = !is_free ? (row.Field<int>("process_type") == 0 ? (price * rate) * Convert.ToDouble(row["amount"]) : 0) : 0;
                    string _code = row["product_code"].ToString();
                    string _a_id = "0";

                    if (is_excise)
                    {
                        double _excise = Convert.ToDouble(row["excise"]);
                        if (_excise > 0)
                            _a_id = _code.TrimStart('ა');
                    }


                    XmlNode GOODS = AddElement("GOODS", null, GOODS_LISTNode);
                    AddElement("ID", "0", GOODS);
                    AddElement("W_NAME", row["name"].ToString(), GOODS);
                    AddElement("UNIT_ID", row["rs_id"].ToString(), GOODS);
                    AddElement("UNIT_TXT", row["rs_id"].ToString() == "99" ? row["unit_name"].ToString() : string.Empty, GOODS);
                    AddElement("QUANTITY", row["amount"].ToString().Replace(",", "."), GOODS);
                    AddElement("PRICE", (rate * price).ToString().Replace(",", "."), GOODS);
                    AddElement("STATUS", "1", GOODS);
                    AddElement("AMOUNT", amount.ToString().Replace(",", "."), GOODS);
                    AddElement("BAR_CODE", _code, GOODS);
                    AddElement("A_ID", _a_id, GOODS);
                    AddElement("VAT_TYPE", res_vat, GOODS);
                }

                if (category == 1 && woods != null && woods.Any())
                {
                    XmlNode Woods_LISTNode = AddElement("WOOD_DOCS_LIST", null, WAYBILLNode);
                    woods.ForEach(w =>
                    {
                        XmlNode WOODS = AddElement("WOODDOCUMENT", null, Woods_LISTNode);
                        AddElement("ID", "0", WOODS);
                        AddElement("DOC_N", w["name"].ToString(), WOODS);
                        DateTime dddt = Convert.ToDateTime(w["tdate"]);
                        string ddt = dddt.Year + "-" + dddt.Month + "-" + dddt.Day + "T" + dddt.Hour + ":" + dddt.Minute + ":" + dddt.Second;
                        AddElement("DOC_DATE", Convert.ToDateTime(w["tdate"]).ToString("yyyy-MM-ddTHH:mm:ss"), WOODS);
                        AddElement("DOC_DESC", w["describ"].ToString(), WOODS);
                        AddElement("STATUS", "1", WOODS);
                    });

                }


                string contragent_code = "";
                string contragent_name = "";
                string check_buyer = "0";
                string type = data.Rows[0]["waybill_type"].ToString();
                if (!is_distributionove && !is_move)
                {
                    contragent_code = data.Rows[0]["contragentCode"].ToString();
                    contragent_name = data.Rows[0]["contragentName"].ToString();
                    check_buyer = data.Rows[0]["is_resident"].ToString() == "0" ? "1" : "0";
                    if (parent_id != "")
                        type = "6";
                }
                AddElement("ID", waybill_id.ToString(), WAYBILLNode);
                AddElement("TYPE", type, WAYBILLNode);
                if (type != "4")
                {
                    AddElement("BUYER_TIN", contragent_code, WAYBILLNode);
                    AddElement("CHEK_BUYER_TIN", check_buyer, WAYBILLNode);
                    AddElement("BUYER_NAME", contragent_name, WAYBILLNode);
                }
                AddElement("START_ADDRESS", Convert.ToString(data.Rows[0]["transp_start_place"]), WAYBILLNode);
                if (type != "4")
                    AddElement("END_ADDRESS", Convert.ToString(data.Rows[0]["transp_end_place"]), WAYBILLNode);
                AddElement("DRIVER_TIN", Convert.ToString(data.Rows[0]["transporter_IdNum"]), WAYBILLNode);
                AddElement("CHEK_DRIVER_TIN", data.Rows[0]["is_foreign"].ToString() == "0" ? "1" : "0", WAYBILLNode);
                AddElement("DRIVER_NAME", driver_name, WAYBILLNode);
                AddElement("TRANSPORT_COAST", Convert.ToString(data.Rows[0]["waybill_cost"]), WAYBILLNode);
                AddElement("RECEPTION_INFO", sender, WAYBILLNode);
                AddElement("RECEIVER_INFO", reciever, WAYBILLNode);
                if (type != "4")
                    AddElement("DELIVERY_DATE", string.Empty, WAYBILLNode);
                string status = trans_id != "7" ? "1" : "8";
                AddElement("STATUS", status.ToString(), WAYBILLNode);
                AddElement("SELER_UN_ID", m_UnID.ToString(), WAYBILLNode);
                AddElement("PAR_ID", parent_id, WAYBILLNode);
                AddElement("FULL_AMOUNT", Convert.ToString(data.Rows[0]["full_amount"]), WAYBILLNode);
                AddElement("CAR_NUMBER", Convert.ToString(data.Rows[0]["transport_number"]), WAYBILLNode);
                AddElement("WAYBILL_NUMBER", string.Empty, WAYBILLNode);
                AddElement("S_USER_ID", m_UserID.ToString(), WAYBILLNode);
                AddElement("BEGIN_DATE", transport_begin_date, WAYBILLNode);
                AddElement("TRAN_COST_PAYER", Convert.ToString(data.Rows[0]["transport_cost_payer"]), WAYBILLNode);
                AddElement("TRANS_ID", trans_id, WAYBILLNode);
                AddElement("TRANS_TXT", transport_text, WAYBILLNode);
                AddElement("COMMENT", comment, WAYBILLNode);
                AddElement("TRANSPORTER_TIN", transporter_tin, WAYBILLNode);
                AddElement("CATEGORY", category.ToString(), WAYBILLNode);


                XElement node = XElement.Load(new XmlNodeReader(doc));
                XElement res_node = GetWaybillService().save_waybill(m_su, m_sp, node);
                int status_v = int.Parse(res_node.Element("STATUS").Value);
                error_str = GetStatusMessage(status_v);
                IEnumerable<XElement> groups = res_node.Element("GOODS_LIST").Elements();
                if (groups != null)
                {
                    foreach (XElement node2 in groups)
                    {
                        int error = int.Parse(node2.Element("ERROR").Value);
                        string er = GetStatusMessage(error);
                        string nm = node2.Element("W_NAME").Value;
                        if (error < 0)
                            error_str += ";" + nm + "(" + er + ")";
                    }
                }

                if (status_v >= 0)
                {
                    waybill_id = int.Parse(res_node.Element("ID").Value);
                    if (waybill_id <= 0)
                        return -12345678;
                    waybill_status = 0;
                    doc_num = "";
                    bool re = GetWaybillInfo(waybill_id, out doc_num, out waybill_status);
                    sql = "UPDATE " + doc_str + " SET waybill_id=" + waybill_id + ", waybill_status=" + waybill_status + ", waybill_num= '" + doc_num + "' WHERE general_id=" + id;
                    if (!_db.ExecuteSql(sql).HasValue)
                        return -1;
                    sql = "UPDATE doc.Generaldocs SET waybill_num=@WAYBILLNUM WHERE id=@ID";
                    m_SqlParams.Clear();
                    m_SqlParams.Add("@ID", id);
                    m_SqlParams.Add("@WAYBILLNUM", doc_num);
                    if (!_db.ExecuteSql(sql, new SqlParameter("@ID", id), new SqlParameter("@WAYBILLNUM", doc_num)).HasValue)
                        return -1;
                    return status_v;
                }
                return status_v;
            }
        }

        public bool GetWaybillInfo(int waybill_id, out string waybill_num, out int status)
        {
            waybill_num = "";
            status = 0;
            if (!IsEnabled())
                return false;
            XElement res_node= GetWaybillService().get_waybill(m_su, m_sp, waybill_id);
            status = int.Parse(res_node.Element("STATUS").Value);
            if (waybill_mode == WaybillMode.Cancel)
            {
                if (status != -2)
                    return false;
            }
            else
            {
                if (status < 0)
                    return false;
            }
            waybill_num = res_node.Element("WAYBILL_NUMBER").Value;
           
            return true;

        }

        public string GetWaybillStatusNameByID(int status_id)
        {
            string str = "";

            switch (status_id)
            {
                case -1:
                    str = "არ არის აქტივირებული RS.ge-ზე";
                    break;
                case 1:
                    str = "აქტივირებულია RS.ge-ზე";
                    break;
                case 2:
                    str = "დასრულებულია RS.ge-ზე";
                    break;
                case -2:
                    str = "გაუქმებულია RS.ge-ზე";
                    break;
                default: break;
            }

            return str;
        }

        public string GetStatusMessage(int status)
        {
           
            if (status >= 0)
                return "ოპერაცია წარმატებით შერსრულდა";
                    

            string str="უცნობი შეცდომა";
            XElement res_node = GetWaybillService().get_error_codes(m_su, m_sp);
            if (res_node == null)
                return str;
            foreach (XElement node in res_node.Elements())
            {
                try
                {
                    if (int.Parse(node.Element("ID").Value) == status)
                    {
                        str = node.Element("TEXT").Value;
                        break;
                    }
                }
                catch
                {


                }

            }

            
            return str;
        }

        public string GetWaybilDocNum(int general_id)
        {
            using (FinaContext _db = new FinaContext())
            {
                string res = string.Empty;
                string sql = @"SELECT ISNULL(Generaldocs.waybill_num, Generaldocs.doc_num_prefix+CONVERT(NVARCHAR(20),Generaldocs.doc_num )) 
                            FROM doc.generaldocs as Generaldocs
                            WHERE GeneralDocs.id=" + general_id;
                res = _db.Database.SqlQuery<string>(sql).FirstOrDefault();
                return res;
            }
        }

        public string GetWaybillType(XmlElement el, string type)
        {
            string res = "";
            if (el != null)
            {
                XmlNode t = el.SelectSingleNode("/WAYBILL_TYPE/ID[text()=" + type + "]").ParentNode;
                if (t != null)
                    res = t.SelectSingleNode("NAME").InnerText;
            }
            return res;
        }


       
    }
}
