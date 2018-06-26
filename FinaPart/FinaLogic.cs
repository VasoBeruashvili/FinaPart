using System;
using System.Collections.Generic;
using System.Linq;
using FinaPart.Models;
using FinaPart.Utils;
using FinaPart.Enums;
using FinaPart.ViewModels;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;

namespace FinaPart
{
    public class FinaLogic : IFinaLogic
    {
        private FinaContext context = new FinaContext();

        public Users GetUser(string identityName)
        {
            return context.Users.FirstOrDefault(u => u.login == identityName);
        }

        public List<Users> GetUsers()
        {
            return context.Users.ToList();
        }

        public List<ProductViewModel> GetProducts(string path)
        {
            var products = string.IsNullOrEmpty(path) ? context.Products.Where(p => p.Path != "0#2#110") : context.Products.Where(p => p.Path.Contains(path));

            List<ProductViewModel> models = products.Select(p => new ProductViewModel
            {
                ID = p.ID,
                Path = p.Path,
                Name = p.Name,
                UnitId = p.UnitId,
                Code = p.Code,
                GroupId = p.GroupId
            }).ToList();

            models.ForEach(p =>
            {
                var ppr = context.ProductPrices.FirstOrDefault(pp => pp.ProductId == p.ID && pp.PriceId == 3); //მხოლოდ საცალო საქონელი

                if (ppr != null)
                {
                    var ppc = context.Currencies.FirstOrDefault(c => c.Id == ppr.ManualCurrencyId);
                    var bc = context.ProductBarCodes.Where(pbc => pbc.product_id == p.ID).FirstOrDefault();
                    var un = context.Units.FirstOrDefault(u => u.Id == p.UnitId);
                    var pg = context.GroupProducts.Where(gp => gp.Id == p.GroupId).FirstOrDefault();

                    p.Price = ppr.ManualVal;
                    p.CurrencyType = ppr.ManualCurrencyId;
                    p.Currency = ppc.Code;
                    p.Barcode = bc == null ? "" : bc.Barcode;
                    p.UnitName = un == null ? "" : un.FullName;
                    p.GroupName = pg == null ? "" : pg.Name;
                }
            });

            return models;
        }

        public IOrderedQueryable GetSubContragents()
        {
            var subContragents = context.SubContragents.Select(sc => new
            {
                id = sc.Id,
                contragentId = sc.ContragentId,
                name = sc.Name,
                address = sc.Address,
                contragent = context.Contragents.FirstOrDefault(c => c.id == sc.ContragentId)
            }).OrderBy(sc => sc.name);

            return subContragents;
        }

        public Contragents GetContragentById(int id)
        {
            return context.Contragents.FirstOrDefault(c => c.id == id);
        }

        public Contragents GetContragentByCode(string code)
        {
            return context.Contragents.Where(c => c.code == code).FirstOrDefault();
        }

        public SubContragent GetSubContragentById(int id)
        {
            return context.SubContragents.FirstOrDefault(sc => sc.Id == id);
        }

        public Users GetUserById(int id)
        {
            return context.Users.FirstOrDefault(u => u.id == id);
        }

        public Companies GetCompany()
        {
            return context.Companies.ToList()[0];
        }

        public Products GetProductById(int id)
        {
            return context.Products.FirstOrDefault(p => p.ID == id);
        }

        public Units GetUnitById(int id)
        {
            return context.Units.FirstOrDefault(u => u.Id == id);
        }

        public Store GetStoreById(int id)
        {
            return context.Stores.FirstOrDefault(s => s.Id == id);
        }

        public Staffs GetStaffById(int id)
        {
            return context.Staffs.Where(s => s.id == id).FirstOrDefault();
        }

        public List<Staffs> GetStaffsList()
        {
            return context.Staffs.ToList();
        }

        public List<Store> GetStoresList()
        {
            return context.Stores.ToList();
        }

        public ProductPrices GetProductPrice(int productId, int priceId)
        {
            return context.ProductPrices.FirstOrDefault(pp => pp.ProductId == productId && pp.PriceId == priceId);
        }

        public GroupProduct GetGroupProductById(int id)
        {
            return context.GroupProducts.Where(gp => gp.Id == id).FirstOrDefault();
        }

        public IQueryable<Products> SearchProducts(string name)
        {
            return context.Products.Where(p => p.Name.Contains(name));
        }

        public List<ProductViewModel> SearchProductsWithImages(string name)
        {
            var products = context.Products.Where(p => p.Name.Contains(name)).Select(p => new ProductViewModel
            {
                ID = p.ID,
                Path = p.Path,
                Name = p.Name,
                UnitId = p.UnitId,
                Code = p.Code,
                GroupId = p.GroupId
            }).ToList();
            foreach (var p in products)
            {
                p.ProductImages = context.ProductImages.Where(pi => pi.ProductId == p.ID).ToList();
            }

            return products;
        }

        public ProductViewModel GetProductWithImagesById(int id)
        {
            ProductViewModel model = new ProductViewModel();
            var product = context.Products.Where(p => p.ID == id).FirstOrDefault();
            if (product != null)
            {
                model.ID = product.ID;
                model.Path = product.Path;
                model.Name = product.Name;
                model.UnitId = product.UnitId;
                model.Code = product.Code;
                model.GroupId = product.GroupId;

                model.ProductImages = context.ProductImages.Where(pi => pi.ProductId == id).ToList();
            }

            return model;
        }

        public ProductViewModel GetProductWithImagesByCode(string code)
        {
            ProductViewModel model = new ProductViewModel();
            var product = context.Products.Where(p => p.Code == code).FirstOrDefault();
            if (product != null)
            {
                model.ID = product.ID;
                model.Path = product.Path;
                model.Name = product.Name;
                model.UnitId = product.UnitId;
                model.Code = product.Code;
                model.GroupId = product.GroupId;

                model.ProductImages = context.ProductImages.Where(pi => pi.ProductId == product.ID).ToList();
            }

            return model;
        }

        public ProductShipping GetProductShipping(string bortNumber)
        {
            return context.ProductShippings.Where(ps => ps.TransportNumber == bortNumber && ps.WaybillType == 4 && ps.WaybillStatus.Value == 1).FirstOrDefault();
        }

        public ProductMove GetProductMove(string bortNumber)
        {
            return context.ProductMoves.Where(pm => pm.TransportNumber == bortNumber && pm.WaybillType == 1 && pm.WaybillStatus.Value == 1).FirstOrDefault();
        }

        public GeneralDocs GetGeneralDocById(int id)
        {
            return context.GeneralDocs.FirstOrDefault(gd => gd.Id == id);
        }

        public List<ContragentContact> GetContragentContactsByContragentId(int id)
        {
            return context.ContragentContacts.Where(cc => cc.ContragentId == id).ToList();
        }

        public Currencies GetCurrencyById(int id)
        {
            return context.Currencies.FirstOrDefault(c => c.Id == id);
        }

        public List<Store> GetOrderedStores()
        {
            return context.Stores.OrderBy(s => s.Name).ToList();
        }

        public Dictionary<int, double> GetProductRestOriginalWithOrder(List<int> product_ids, int store_id, DateTime toDate, string connString)
        {
            using (var _db = new DBContext(connString))
            {
                string sql = string.Format(@"SELECT  a.product_id, ISNULL(SUM(a.amount * CASE a.is_order WHEN 1 THEN -1 WHEN 0 THEN a.coeff END),0) AS rest
                                         FROM doc.ProductsFlow AS a 
                                         INNER JOIN doc.Generaldocs AS g ON g.id=a.general_id
                                         WHERE g.tdate <='{0}' AND ISNULL(g.is_deleted,0) = 0  AND a.is_expense=0 AND a.is_order IN(0,1) AND a.store_id={1} 
                                               AND a.product_id IN({2}) 
                                         GROUP BY  a.product_id
                                         HAVING ISNULL(SUM(a.amount * CASE a.is_order WHEN 1 THEN -1 WHEN 0 THEN a.coeff END),0) <>0
                                          ", toDate.ToString("yyyy-MM-dd HH:mm:ss.fff"), store_id, string.Join(",", product_ids.ConvertAll(Convert.ToString).ToArray()));
                return _db.GetDictionary<int, double>(sql);
            }
        }


        public bool IsCompanyVat(DateTime date)
        {
            Companies _res = context.Companies.Select(a => a).FirstOrDefault();
            if (!_res.Vat.HasValue || !_res.Vat.Value || date < _res.Tdate)
                return false;
            return true;
        }

        public bool SaveEntriesFast(int general_id)
        {
            if (!DeleteEntries(general_id))
                return false;
            List<Entries> _entries = new List<Entries>();
            bool companyVat = IsCompanyVat(DateTime.Now);
            int contragent_id = context.GeneralDocs.FirstOrDefault(gd => gd.Id == general_id).ParamId1.Value;
            var contragent = context.Contragents.FirstOrDefault(c => c.id == contragent_id);
            var generalDoc = context.GeneralDocs.FirstOrDefault(gd => gd.Id == general_id);
            string contragent_account = contragent.account;
            int contragent_vat_type = contragent.vat_type != null && contragent.vat_type != null ? Convert.ToInt32(contragent.vat_type) : 2;
            int contragentGroupID = contragent.group_id.Value;
            double currency_rate = 1;
            int project_id = 1;
            int currency_id = 1;
            double vat = 0.0;
            double total_excise = 0;

            //end products
            double price_1s = 0.0;
            double price_2s = 0.0;
            double price_3s = 0.0;
            double price_4s = 0.0;
            // service out
            double vvt = 18;
            double service_vat = 0;
            double total_service_excise = 0;
            double product_quantity = 1;
            double price_amount = generalDoc.Amount.Value;
            double excise = 0;
            price_amount *= currency_rate;
            int product_vat_type = 0;
            double vatPercent = vvt;
            double pr = price_amount;
            double vat_val = (pr) - ((pr * 100) / (vatPercent + 100));
            if (vat_val > 0)
                price_amount -= vat_val;
            price_amount -= (product_quantity * excise);
            vat += vat_val;
            service_vat += vat;
            total_excise += (product_quantity * excise);
            total_service_excise += (product_quantity * excise);
            if (contragent_vat_type >= 2 && product_vat_type == 1)
                product_vat_type = contragent_vat_type;

            price_1s = 0;
            price_2s = 0;
            price_3s = 0;
            price_4s = 0;
            double original_price_1s = 0;
            double original_price_2s = 0;
            double original_price_3s = 0;
            double original_price_4s = 0;
            if (!companyVat)
            {
                price_1s += price_amount;
                original_price_1s += price_amount + vat_val;
            }
            else
            {
                if (product_vat_type == 1) //company vat and client vat
                {
                    price_2s += price_amount;
                    original_price_2s += price_amount + vat_val;
                }
                else if (product_vat_type == 2)//ჩათვლის უფლებით
                {
                    price_3s += price_amount;
                    original_price_3s += price_amount + vat_val;
                }
                else if (product_vat_type == 3)//ჩათვლის უფლების გარეშე
                {
                    price_4s += price_amount;
                    original_price_4s += price_amount + vat_val;
                }
            }

            if (vat > 0)
            {
                var generatedAmount = vat / currency_rate;
                // შემოსავალი რეალიზაციიდან
                _entries.Add(new Entries { GeneralId = general_id, DebitAcc = contragent_account, CreditAcc = "6110", Amount = generalDoc.Amount - generatedAmount, Rate = currency_rate, N = product_quantity, N2 = product_quantity, A1 = contragent_id, A3 = contragentGroupID, B1 = 114, B3 = 2, Comment = "შემოსავალი რეალიზაციიდან", ProjectId = project_id, CurrencyId = currency_id });

                // გადასახდელი დღგ რეალიზაციიდან
                _entries.Add(new Entries { GeneralId = general_id, DebitAcc = contragent_account, CreditAcc = "3330", Amount = generatedAmount, Rate = currency_rate, A1 = contragent_id, A3 = contragentGroupID, Comment = "გადასახდელი დღგ რეალიზაციიდან", ProjectId = project_id, CurrencyId = currency_id });
            }

            _entries.ForEach(e =>
            {
                context.Entries.Add(e);
            });

            return context.SaveChanges() >= 0;
        }

        public bool SaveEntriesFastCancel(int general_id)
        {
            if (!DeleteEntries(general_id))
                return false;
            List<Entries> _entries = new List<Entries>();
            bool companyVat = IsCompanyVat(DateTime.Now);
            int contragent_id = context.GeneralDocs.FirstOrDefault(gd => gd.Id == general_id).ParamId1.Value;
            var contragent = context.Contragents.FirstOrDefault(c => c.id == contragent_id);
            var generalDoc = context.GeneralDocs.FirstOrDefault(gd => gd.Id == general_id);
            string contragent_account = contragent.account;
            int contragent_vat_type = contragent.vat_type != null && contragent.vat_type != null ? Convert.ToInt32(contragent.vat_type) : 2;
            int contragentGroupID = contragent.group_id.Value;
            double currency_rate = 1;
            int project_id = 1;
            int currency_id = 1;
            double vat = 0.0;
            double total_excise = 0;

            //end products
            double price_1s = 0.0;
            double price_2s = 0.0;
            double price_3s = 0.0;
            double price_4s = 0.0;
            // service out
            double vvt = 18;
            double service_vat = 0;
            double total_service_excise = 0;
            double product_quantity = 1;
            double price_amount = generalDoc.Amount.Value;
            double excise = 0;
            price_amount *= currency_rate;
            int product_vat_type = 0;
            double vatPercent = vvt;
            double pr = price_amount;
            double vat_val = (pr) - ((pr * 100) / (vatPercent + 100));
            if (vat_val > 0)
                price_amount -= vat_val;
            price_amount -= (product_quantity * excise);
            vat += vat_val;
            service_vat += vat;
            total_excise += (product_quantity * excise);
            total_service_excise += (product_quantity * excise);
            if (contragent_vat_type >= 2 && product_vat_type == 1)
                product_vat_type = contragent_vat_type;

            price_1s = 0;
            price_2s = 0;
            price_3s = 0;
            price_4s = 0;
            double original_price_1s = 0;
            double original_price_2s = 0;
            double original_price_3s = 0;
            double original_price_4s = 0;
            if (!companyVat)
            {
                price_1s += price_amount;
                original_price_1s += price_amount + vat_val;
            }
            else
            {
                if (product_vat_type == 1) //company vat and client vat
                {
                    price_2s += price_amount;
                    original_price_2s += price_amount + vat_val;
                }
                else if (product_vat_type == 2)//ჩათვლის უფლებით
                {
                    price_3s += price_amount;
                    original_price_3s += price_amount + vat_val;
                }
                else if (product_vat_type == 3)//ჩათვლის უფლების გარეშე
                {
                    price_4s += price_amount;
                    original_price_4s += price_amount + vat_val;
                }
            }

            if (vat > 0)
            {
                var generatedAmount = vat / currency_rate;
                // გადასახდელი დღგ რეალიზაციიდან
                _entries.Add(new Entries { GeneralId = general_id, DebitAcc = "7290", CreditAcc = "1610", Amount = generatedAmount, Rate = currency_rate, B1 = 1, B2 = 4, B3 = 11, Comment = "საქონლის ჩამოწერა თვითღირებულებით", ProjectId = project_id, CurrencyId = currency_id });
            }

            _entries.ForEach(e =>
            {
                context.Entries.Add(e);
            });

            return context.SaveChanges() >= 0;
        }

        public Dictionary<int, double> GetProductSelfCostAverage(Dictionary<int, double> IdList, DateTime ActionDate)
        {
            List<int> ids = IdList.Keys.ToList();
            string s = context.Params.Where(p => p.Name == "NegativeSelfCost").Select(p => p.Value).FirstOrDefault();
            var _self_info = (from r in context.Products
                              let _t = context.ProductsFlow
                                  .Join(context.GeneralDocs, pf => pf.GeneralId, gd => gd.Id, (pf, gd) => new { pf, gd })
                                  .Where(_j => _j.pf.ProductId == r.ID && _j.pf.IsOrder == 0 && _j.gd.Tdate < ActionDate)
                                  .Select(_r => _r.pf.Amount.Value * _r.pf.SelfCost.Value * _r.pf.Coeff.Value).DefaultIfEmpty().Sum()
                              let _t2 = context.ProductsFlow
                                  .Join(context.GeneralDocs, pf => pf.GeneralId, gd => gd.Id, (pf, gd) => new { pf, gd })
                                  .Where(_j => _j.pf.ProductId == r.ID && _j.pf.IsOrder == 0 && _j.pf.IsExpense == 0 && _j.gd.Tdate < ActionDate)
                                  .Select(_r => _r.pf.Amount.Value * _r.pf.Coeff.Value).DefaultIfEmpty().Sum()
                              where ids.Contains(r.ID)
                              select new
                              {
                                  Id = r.ID,
                                  Rest = _t2,
                                  RestCost = _t
                              }).ToList();
            return _self_info.Select(a => new KeyValuePair<int, double>(a.Id, s != "2" ? (a.Rest > 0 && a.RestCost > 0 ? a.RestCost / a.Rest : 0) : ((a.Rest > 0 && a.RestCost > 0 && a.Rest >= IdList[a.Id]) ? a.RestCost / a.Rest : (a.RestCost > 0 && IdList[a.Id] > 0 && a.Rest < IdList[a.Id]) ? a.RestCost / IdList[a.Id] : 0))).ToDictionary(a => a.Key, b => b.Value);
        }

        public KeyValuePair<int, string> FinaMove(int general_id, DateTime tdate, int store_from, int store_to, int staff_id, int user_id, bool make_entry, ProductMoveViewModel moveModel, List<ProductsFlowViewModel> flowsModel)
        {
            long doc_num = (context.GeneralDocs.Where(gd => gd.DocType == 20).Max(gd => gd.DocNum) ?? 0) + 1;

            Dictionary<int, double> _moves = flowsModel.GroupBy(p => p.ProductId).Select(p => new KeyValuePair<int, double>(p.Key, p.Select(q => q.Quantity).DefaultIfEmpty().Sum())).ToDictionary(k => k.Key, v => v.Value);
            Dictionary<int, double> idList = new Dictionary<int, double>();
            Dictionary<int, double> _self_costs = GetProductSelfCostAverage(idList, DateTime.Now);


            GeneralDocs GeneralDocsItem;
            ProductMove _move;
            ProductsFlow _flow;
            Entries _entry;
            using (DbContextTransaction tran = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    if (general_id > 0)
                    {
                        GeneralDocsItem = context.GeneralDocs.Where(g => g.Id == general_id).FirstOrDefault();
                        if (GeneralDocsItem != null)
                        {
                            GeneralDocsItem.ParamId1 = store_from;
                            GeneralDocsItem.ParamId2 = store_to;
                            GeneralDocsItem.StaffId = staff_id;
                            GeneralDocsItem.UserId = user_id;
                            GeneralDocsItem.Tdate = tdate;
                            GeneralDocsItem.Amount = flowsModel.Select(c => c.Quantity * (!_self_costs.ContainsKey(c.ProductId) ? 0 : _self_costs[c.ProductId])).Sum();

                            context.Entry(GeneralDocsItem).State = System.Data.Entity.EntityState.Modified;

                            _move = context.ProductMoves.Where(p => p.GeneralId == general_id).FirstOrDefault();
                            if (_move != null)
                            {
                                _move.ActivateDate = moveModel.ActivateDate;
                                _move.Avto = moveModel.Avto;
                                _move.Comment = moveModel.Comment;
                                _move.DeliveryDate = moveModel.DeliveryDate;
                                _move.DiscountPercent = moveModel.DiscountPercent;
                                _move.DriverCardNumber = moveModel.DriverCardNumber;
                                _move.DriverName = moveModel.DriverName;
                                _move.IsWaybill = moveModel.IsWaybill;
                                _move.Other = moveModel.Other;
                                _move.Railway = moveModel.Railway;
                                _move.RecieverIdNum = moveModel.RecieverIdNum;
                                _move.RecieverName = moveModel.RecieverName;
                                _move.ResponsablePerson = moveModel.ResponsablePerson;
                                _move.ResponsablePersonDate = moveModel.ResponsablePersonDate;
                                _move.ResponsablePersonNum = moveModel.ResponsablePersonNum;
                                _move.SenderIdNum = moveModel.SenderIdNum;
                                _move.SenderName = moveModel.SenderName;
                                _move.StaffId = staff_id;
                                _move.TransportBeginDate = moveModel.TransportBeginDate;
                                _move.TransportCostPayer = moveModel.TransportCostPayer;
                                _move.TransportEndPlace = moveModel.TransportEndPlace;
                                _move.TransporterIdNum = moveModel.TransporterIdNum;
                                _move.TransporterName = moveModel.TransporterName;
                                _move.TransportModel = moveModel.TransportModel;
                                _move.TransportNumber = moveModel.TransportNumber;
                                _move.TransportStartPlace = moveModel.TransportStartPlace;
                                _move.TransportText = moveModel.TransportText;
                                _move.TransportTypeId = moveModel.TransportTypeId;
                                _move.WaybillCost = moveModel.WaybillCost;
                                _move.WaybillId = moveModel.WaybillId;
                                _move.WaybillStatus = moveModel.WaybillStatus;
                                _move.WaybillType = moveModel.WaybillType;

                                context.Entry(_move).State = System.Data.Entity.EntityState.Modified;
                            }

                        }
                    }
                    else
                    {
                        GeneralDocsItem = new GeneralDocs
                        {
                            Id = general_id,
                            Tdate = DateTime.Now,
                            DocNum = doc_num,
                            DocNumPrefix = "",
                            DocType = 20,
                            Purpose = "საქონლის გადატანა № " + doc_num,
                            Vat = 0,
                            Amount = flowsModel.Select(c => c.Quantity * (!_self_costs.ContainsKey(c.ProductId) ? 0 : _self_costs[c.ProductId])).Sum(),
                            UserId = user_id,
                            ParamId1 = store_from,
                            ParamId2 = store_to,
                            StaffId = staff_id,
                            MakeEntry = true,
                            Uid = Guid.NewGuid().ToString(),
                        };
                        context.GeneralDocs.Add(GeneralDocsItem);

                        _move = new ProductMove
                        {
                            ActivateDate = moveModel.ActivateDate ?? DateTime.Today.AddSeconds(10),
                            Avto = moveModel.Avto,
                            Comment = moveModel.Comment,
                            DeliveryDate = moveModel.DeliveryDate,
                            CategoryId = 0,
                            CheckStatus = 0,
                            DiscountPercent = moveModel.DiscountPercent ?? 0,
                            DriverCardNumber = moveModel.DriverCardNumber,
                            DriverName = moveModel.DriverName,
                            IsForeign = 0,
                            GeneralDoc = GeneralDocsItem,
                            IsWaybill = moveModel.IsWaybill ?? 1,
                            LocationType = true,
                            Other = moveModel.Other,
                            Railway = moveModel.Railway,
                            RecieverIdNum = moveModel.RecieverIdNum,
                            RecieverName = moveModel.RecieverName,
                            ResponsablePerson = moveModel.ResponsablePerson,
                            ResponsablePersonDate = moveModel.ResponsablePersonDate ?? DateTime.Now,
                            ResponsablePersonNum = moveModel.ResponsablePersonNum,
                            SenderIdNum = moveModel.SenderIdNum,
                            SenderName = moveModel.SenderName,
                            StaffId = staff_id,
                            TransportBeginDate = moveModel.TransportBeginDate,
                            TransportCostPayer = moveModel.TransportCostPayer,
                            TransportEndPlace = moveModel.TransportEndPlace,
                            TransporterIdNum = moveModel.TransporterIdNum,
                            TransporterName = moveModel.TransporterName,
                            TransportModel = moveModel.TransportModel,
                            TransportNumber = moveModel.TransportNumber,
                            TransportStartPlace = moveModel.TransportStartPlace,
                            TransportText = moveModel.TransportText,
                            TransportTypeId = moveModel.TransportTypeId,
                            WaybillCost = moveModel.WaybillCost,
                            WaybillId = moveModel.WaybillId ?? 0,
                            WaybillNum = "",
                            WaybillStatus = moveModel.WaybillStatus ?? -1,
                            WaybillType = moveModel.WaybillType ?? 1
                        };
                        context.ProductMoves.Add(_move);

                    }


                    if (general_id > 0)
                    {
                        var _flows = context.ProductsFlow.Where(f => f.GeneralId == general_id).ToList();
                        foreach (ProductsFlow fl in _flows)
                        {
                            context.ProductsFlow.Remove(fl);
                            context.Entry(fl).State = System.Data.Entity.EntityState.Deleted;
                        }
                        context.SaveChanges();
                    }

                    foreach (ProductsFlowViewModel flModel in flowsModel)
                    {
                        _flow = new ProductsFlow
                        {
                            GeneralDocs = GeneralDocsItem,
                            ProductId = flModel.ProductId,
                            Amount = flModel.Quantity,
                            UnitId = flModel.UnitId,
                            StoreId = flModel.StoreId,
                            Price = !_self_costs.ContainsKey(flModel.ProductId) ? 0 : _self_costs[flModel.ProductId],
                            SelfCost = !_self_costs.ContainsKey(flModel.ProductId) ? 0 : _self_costs[flModel.ProductId],
                            IsMove = 1,
                            Coeff = -1,
                            Uid = flModel.Uid != Guid.Empty ? flModel.Uid : Guid.NewGuid()
                        };
                        context.ProductsFlow.Add(_flow);

                        _flow = new ProductsFlow
                        {
                            GeneralDocs = GeneralDocsItem,
                            ProductId = flModel.ProductId,
                            Amount = flModel.Quantity,
                            UnitId = flModel.UnitId,
                            StoreId = store_to,
                            Price = !_self_costs.ContainsKey(flModel.ProductId) ? 0 : _self_costs[flModel.ProductId],
                            SelfCost = !_self_costs.ContainsKey(flModel.ProductId) ? 0 : _self_costs[flModel.ProductId],
                            IsMove = 1,
                            Visible = 0,
                            Coeff = 1,
                            Uid = flModel.Uid != Guid.Empty ? flModel.Uid : Guid.NewGuid()
                        };
                        context.ProductsFlow.Add(_flow);
                    }

                    if (make_entry)
                    {
                        if (general_id > 0)
                        {
                            var _flows = context.Entries.Where(f => f.GeneralId == general_id).ToList();
                            foreach (Entries fl in _flows)
                            {
                                context.Entries.Remove(fl);
                                context.Entry(fl).State = System.Data.Entity.EntityState.Modified;
                            }
                            context.SaveChanges();
                        }

                        foreach (ProductsFlowViewModel flModel in flowsModel)
                        {
                            var product = context.Products.Where(p => p.ID == flModel.ProductId).Select(c => new { path = c.Path, group_id = c.GroupId }).First();
                            _entry = new Entries
                            {
                                GeneralDocs = GeneralDocsItem,
                                DebitAcc = GetProductAccountByPath(product.path, 0),
                                CreditAcc = GetProductAccountByPath(product.path, 0),
                                Amount = flModel.Quantity * (!_self_costs.ContainsKey(flModel.ProductId) ? 0 : _self_costs[flModel.ProductId]),
                                N = flModel.Quantity,
                                N2 = flModel.Quantity,
                                A1 = flModel.ProductId,
                                A2 = GeneralDocsItem.ParamId2,
                                A3 = product.group_id,
                                B1 = flModel.ProductId,
                                B2 = GeneralDocsItem.ParamId1,
                                B3 = product.group_id,
                                Comment = "საქონლის გადაადგილება",
                                ProjectId = 1,
                                CurrencyId = 1
                            };
                            context.Entries.Add(_entry);

                            _entry = new Entries
                            {
                                GeneralDocs = GeneralDocsItem,
                                DebitAcc = "1696",
                                CreditAcc = GetProductAccountByPath(product.path, 0),
                                Amount = flModel.Quantity * (!_self_costs.ContainsKey(flModel.ProductId) ? 0 : _self_costs[flModel.ProductId]),
                                N = flModel.Quantity,
                                N2 = flModel.Quantity,
                                A1 = flModel.ProductId,
                                A2 = GeneralDocsItem.ParamId2,
                                A3 = product.group_id,
                                B1 = flModel.ProductId,
                                B2 = GeneralDocsItem.ParamId1,
                                B3 = product.group_id,
                                Comment = "საქონლის გადაადგილება",
                                ProjectId = 1,
                                CurrencyId = 1
                            };
                            context.Entries.Add(_entry);

                            _entry = new Entries
                            {
                                GeneralDocs = GeneralDocsItem,
                                DebitAcc = GetProductAccountByPath(product.path, 0),
                                CreditAcc = "1696",
                                Amount = flModel.Quantity * (!_self_costs.ContainsKey(flModel.ProductId) ? 0 : _self_costs[flModel.ProductId]),
                                N = flModel.Quantity,
                                N2 = flModel.Quantity,
                                A1 = flModel.ProductId,
                                A2 = GeneralDocsItem.ParamId2,
                                A3 = product.group_id,
                                B1 = flModel.ProductId,
                                B2 = GeneralDocsItem.ParamId2,
                                B3 = product.group_id,
                                Comment = "საქონლის გადაადგილება",
                                ProjectId = 1,
                                CurrencyId = 1
                            };
                            context.Entries.Add(_entry);
                        }
                    }

                    context.SaveChanges();
                    tran.Commit();

                    general_id = GeneralDocsItem.Id;
                    return new KeyValuePair<int, string>(general_id, string.Empty);
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return new KeyValuePair<int, string>(general_id, ex.Message);
                }
            }
        }

        public List<ProductMoveListViewModel> GetProductMoves(DateTime start_date, DateTime end_date)
        {
            using (FinaContext _db = new FinaContext())
            {
                return _db.Database.SqlQuery<ProductMoveListViewModel>(@"SELECT GeneralDocs.id, GeneralDocs.tdate, CASE ISNULL(P.waybill_id,0) WHEN 0 THEN GeneralDocs.doc_num_prefix+CONVERT(NVARCHAR(20),GeneralDocs.doc_num ) ELSE P.waybill_num END AS doc_num, GeneralDocs.purpose,  CONVERT(DECIMAL(38,2), ROUND(ROUND(CONVERT(decimal(38,14),GeneralDocs.amount),10),2))as amount ,Currencies.code AS currency_name, st_from.name as store_from, st_to.name as store_to,P.transport_number AS auto, DocStatusType.name AS rs_status, ISNULL(staff.name,'') AS staff_name, CASE (SELECT value FROM config.Params WHERE name='ProductMoveService') WHEN 0 THEN N'' ELSE CASE p.location_type WHEN 1 THEN N'აქტიური' WHEN 0 then N'დადასტურებული' END END AS location_type,waybill_id,waybill_status  

FROM doc.GeneralDocs AS GeneralDocs INNER JOIN config.DocTypes AS DocTypes ON GeneralDocs.doc_type = DocTypes.id

INNER JOIN book.Currencies AS Currencies ON GeneralDocs.currency_id = Currencies.id
INNER JOIN doc.ProductMove AS P ON GeneralDocs.id = P.general_id 
LEFT JOIN book.staff AS staff ON staff.id=P.staff_id  
INNER JOIN book.stores AS st_from ON GeneralDocs.param_id1 = st_from.id
INNER JOIN config.DocStatusTypes DocStatusType on DocStatusType.tag = DocTypes.tag  AND DocStatusType.status_id= CASE ISNULL(P.waybill_status, -1)  when -2 then 4 when -1 then 1  when 0 then 1  when 1 then 2  when 2 then 3 when 8 then 6 else 5 end
INNER JOIN book.stores AS st_to ON GeneralDocs.param_id2 = st_to.id
WHERE GeneralDocs.tdate BETWEEN @start_date AND @end_date
ORDER BY GeneralDocs.tdate", new SqlParameter("@start_date", start_date), new SqlParameter("@end_date", end_date)).ToList();
            }
        }

        public ProductMoveViewModel GetProductMove(int general_id)
        {
            ProductMoveViewModel model = new ProductMoveViewModel()
            {
                WaybillType = 1,
                TransportTypeId = 1,
                TransportCostPayer = 1,
            };

            GeneralDocs _g = context.GeneralDocs.Where(g => g.Id == general_id && g.DocType == 20).First();
            ProductMove _mov = context.ProductMoves.Where(m => m.GeneralId == general_id).First();
            model.SenderIdNum = _mov.SenderIdNum;
            model.SenderName = _mov.SenderName;
            model.TransportStartPlace = _mov.TransportStartPlace;
            model.TransporterName = _mov.TransporterName;
            model.TransporterIdNum = _mov.TransporterIdNum;
            model.RecieverIdNum = _mov.RecieverIdNum;
            model.RecieverName = _mov.RecieverName;
            model.TransportEndPlace = _mov.TransportEndPlace;
            model.ResponsablePerson = _mov.ResponsablePerson;
            model.ResponsablePersonNum = _mov.ResponsablePersonNum;
            model.ResponsablePersonDate = _mov.ResponsablePersonDate;
            model.TransportModel = _mov.TransportModel;
            model.TransportNumber = _mov.TransportNumber;
            model.DriverCardNumber = _mov.DriverCardNumber;
            model.DiscountPercent = _mov.DiscountPercent;
            model.IsWaybill = _mov.IsWaybill;
            model.WaybillId = _mov.WaybillId;
            model.WaybillType = _mov.WaybillType;
            model.WaybillCost = _mov.WaybillCost;
            model.DeliveryDate = _mov.DeliveryDate;
            model.WaybillStatus = _mov.WaybillStatus;
            model.TransportBeginDate = _mov.TransportBeginDate;
            model.TransportCostPayer = _mov.TransportCostPayer;
            model.TransportTypeId = _mov.TransportTypeId;
            model.DriverName = _mov.DriverName;
            model.ActivateDate = _mov.ActivateDate;
            model.Comment = _mov.Comment;
            model.TransportText = _mov.TransportText;

            model.General.Id = _g.Id;
            model.General.Tdate = _g.Tdate.Value;
            model.General.DocNum = _g.DocNum.Value;
            model.General.DocNumPrefix = _g.DocNumPrefix;
            model.General.Purpose = _g.Purpose;
            model.General.Amount = _g.Amount.Value;
            model.General.UserId = _g.UserId.Value;
            model.General.RefId = _g.RefId ?? 0;
            model.General.ParamId1 = _g.ParamId1.Value;
            model.General.ParamId2 = _g.ParamId2.Value;
            model.General.MakeEntry = _g.MakeEntry.Value;
            model.General.ProjectId = _g.ProjectId.Value;
            model.General.Uid = _g.Uid;
            model.General.SyncStatus = _g.SyncStatus.Value;
            model.General.WaybillNum = _g.WaybillNum;
            model.General.StoreId = _g.StoreId.Value;
            model.General.StaffId = _g.StaffId;
            model.General.StaffName = context.Staffs.Where(c => c.id == _g.StaffId).Select(c => c.name).FirstOrDefault();
            model.General.AnalyticCode = _g.AnalyticCode.Value;
            model.General.IsBlocked = _g.IsBlocked.Value;
            model.General.IsDeleted = _g.IsDeleted.Value;
            model.General.DeleteUserId = _g.DeleteUserId;
            model.General.DeleteDate = _g.DeleteDate;
            model.General.CreateDate = _g.CreateDate;
            model.General.HouseId = _g.HouseId;

            model.Products = context.ProductsFlow.Where(f => f.GeneralId == general_id).Where(f => f.Visible == 1).OrderBy(f => f.Id).Select(f => new ProductsFlowViewModel
            {
                Id = f.Id,
                ProductId = f.ProductId,
                ProductName = f.Products.Name,
                ProductCode = f.Products.Code,
                ProductPath = f.Products.Path,
                ProductGroupId = f.Products.GroupId.Value,
                Quantity = f.Amount.Value,
                UnitCost = f.Price.Value,
                SelfCost = f.SelfCost.Value,
                Total = Math.Round(f.Amount.Value * f.Price.Value, 2),
                UnitId = f.UnitId,
                UnitName = f.Unit.FullName,
                Uid = f.Uid,
                Rest = 0
            }).ToList();

            return model;
        }

        public string GetProductAccountByPath(string path, int level)
        {

            string[] seps = path.Split('#');
            if (seps.Length > 4)
                path = seps[0] + "#" + seps[1] + "#" + seps[2] + "#" + seps[3];
            string _acc = context.GroupProducts.Where(gp => gp.Path.StartsWith(path)).Select(gp => gp.Account).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(_acc))
                _acc = "";
            string[] vals = _acc.Split(';');
            string account = vals[0];
            if (level == 1)
            {
                if (vals.Length <= 1)
                    return (account == "1620" || account == "1630") ? "7110" : "7290";
                return vals[1];
            }

            if (path.StartsWith("0#1#10#11#"))
                return "1610";
            else if (path.StartsWith("0#1#10#12#"))
                return "1620";
            else if (path.StartsWith("0#1#10#13#"))
                return "1630";
            else if (path.StartsWith("0#1#10#14#"))
                return "1640";
            else if (path.StartsWith("0#1#10#15#"))
                return "1690";

            else if (path.StartsWith("0#2#120#121#"))
                return "7420";
            else if (path.StartsWith("0#2#120#122#"))
                return "7425";
            else if (path.StartsWith("0#2#120#123#"))
                return "7430";
            else if (path.StartsWith("0#2#120#124#"))
                return "7435";
            else if (path.StartsWith("0#2#120#125#"))
                return "7440";
            else if (path.StartsWith("0#2#120#126#"))
                return "7445";
            else if (path.StartsWith("0#2#120#127#"))
                return "7450";
            else if (path.StartsWith("0#2#120#128#"))
                return "7490";
            else if (path.StartsWith("0#3#40#41#"))
                return "2110";
            else if (path.StartsWith("0#3#40#42#"))
                return "2120";
            else if (path.StartsWith("0#3#40#43#"))
                return "2130";
            else if (path.StartsWith("0#3#40#44#"))
                return "2140";
            else if (path.StartsWith("0#3#40#45#"))
                return "2150";
            else if (path.StartsWith("0#3#40#46#"))
                return "2160";
            else if (path.StartsWith("0#3#40#47#"))
                return "2170";
            else if (path.StartsWith("0#3#40#48#"))
                return "2180";
            else if (path.StartsWith("0#3#40#49#"))
                return "2190";
            else if (path.StartsWith("0#3#60#61#"))
                return "2510";
            else if (path.StartsWith("0#3#60#62#"))
                return "2520";
            else if (path.StartsWith("0#3#60#63#"))
                return "2530";
            else if (path.StartsWith("0#3#60#129#"))
                return "2540";
            else if (path.StartsWith("0#3#60#130#"))
                return "2590";
            else if (account != "")
                return account;

            else return "";
        }

        public int FinaRealization(int general_id, int? contragentId, double? amount, int? storeId, int staffId, double? quantity, int productId, int? unitId, int? rsSaleType, string userIdentityName)
        {
            //selfcost calculation
            Dictionary<int, double> idList = new Dictionary<int, double>();
            idList.Add(productId, quantity.Value);
            var productSelfCost = GetProductSelfCostAverage(idList, DateTime.Now);
            //---


            var gdc = context.GeneralDocs.FirstOrDefault(gd => gd.Id == general_id);
            var pdot = context.ProductOut.FirstOrDefault(po => po.GeneralId == general_id);
            var pdfl = context.ProductsFlow.FirstOrDefault(pf => pf.GeneralId == general_id);

            if (gdc != null && pdot != null && pdfl != null)
            {
                context.GeneralDocs.Remove(gdc);
                context.ProductOut.Remove(pdot);
                context.ProductsFlow.Remove(pdfl);

                DeleteEntries(gdc.Id);

                context.SaveChanges();
            }

            GeneralDocs GeneralDocsItem = new GeneralDocs();
            ProductOut ProductOutItem = new ProductOut();
            ProductsFlow ProductsFlowList = new ProductsFlow();

            //take authenticated user user
            var currentUser = context.Users.FirstOrDefault(u => u.login == userIdentityName);

            //configure doc.GeneralDocs
            GeneralDocsItem.ParamId1 = contragentId;
            GeneralDocsItem.Amount = (rsSaleType.HasValue && rsSaleType.Value == (int)EnumSaleTypes.Spend ? 0.01 : amount) * quantity; //თუ ჩამოწერაა FINA ში გატარდეს 0.01 თეთრი
            GeneralDocsItem.StaffId = staffId;

            GeneralDocsItem.Tdate = DateTime.Now; //For tdate and tdate1 balance reports
            GeneralDocsItem.Purpose = "საქონლის გაყიდვა"; //set purpose to GeneralDocs (as default)
            GeneralDocsItem.DocType = 21; //set doc_type to GeneralDocs (საცალო გაყიდვა)
            GeneralDocsItem.UserId = currentUser.id; //set user_id to GeneralDocs
            GeneralDocsItem.CreateDate = DateTime.Now;
            GeneralDocsItem.Vat = 18; //set vat to GeneralDocs
            GeneralDocsItem.DocNum = context.GeneralDocs.Where(gd => gd.DocType == 21).Max(gd => gd.DocNum) + 1; //set doc_num + 1 to GeneralDocs
            GeneralDocsItem.ParamId2 = storeId; //set param_id2 to GeneralDocs (საწყობი)
            GeneralDocsItem.StatusId = 1; //set status_id to GeneralDocs (added from web interface)
            GeneralDocsItem.MakeEntry = true; //set make_entry to GeneralDocs (გატარებები)

            //configure doc.ProductOut
            ProductOutItem.StaffId = staffId;

            ProductOutItem.ResponsablePersonDate = DateTime.Now;
            ProductOutItem.Other = true;
            ProductOutItem.TransportCostPayer = 1;
            ProductOutItem.InvoiceBankId = 0;
            ProductOutItem.PayType = "1";
            //docItem.ProductOutItem[0].CheckStatus = 0; //TODO add?

            //configure doc.ProductsFlow
            ProductsFlowList.Amount = quantity;//docItem.GeneralDocsItem.Amount; //the same Amount as doc.GeneralDocs
            ProductsFlowList.Price = rsSaleType.HasValue && rsSaleType.Value == (int)EnumSaleTypes.Spend ? 0.01 : amount; //the same Amount as doc.GeneralDocs... თუ ჩამოწერაა FINA ში გატარდეს 0.01 თეთრი
            ProductsFlowList.ProductId = productId;
            ProductsFlowList.StoreId = storeId;
            ProductsFlowList.VatPercent = 18;
            ProductsFlowList.IsOrder = 0;
            ProductsFlowList.IsExpense = 0;
            ProductsFlowList.IsMove = 0;
            ProductsFlowList.UnitId = 17;
            ProductsFlowList.InId = unitId;
            ProductsFlowList.VendorId = 0;
            ProductsFlowList.OutId = 0;
            ProductsFlowList.ServiceStaffId = 0;
            ProductsFlowList.Coeff = -1;
            ProductsFlowList.OriginalPrice = ProductsFlowList.Price;

            //add entities
            context.GeneralDocs.Add(GeneralDocsItem);
            context.ProductOut.Add(ProductOutItem);
            context.ProductsFlow.Add(ProductsFlowList);

            var saveResult = context.SaveChanges() >= 0;

            SaveEntriesFast(GeneralDocsItem.Id);

            return GeneralDocsItem.Id;
        }

        public int FinaProductCancel(int general_id, int? storeId, int staffId, double? quantity, int productId, int? unitId, string userIdentityName)
        {
            var gdc = context.GeneralDocs.FirstOrDefault(gd => gd.Id == general_id);
            var pdcl = context.ProductCancels.FirstOrDefault(pc => pc.GeneralId == general_id);
            var pdfl = context.ProductsFlow.FirstOrDefault(pf => pf.GeneralId == general_id);

            if (gdc != null && pdcl != null && pdfl != null)
            {
                context.GeneralDocs.Remove(gdc);
                context.ProductCancels.Remove(pdcl);
                context.ProductsFlow.Remove(pdfl);

                DeleteEntries(gdc.Id);

                context.SaveChanges();
            }

            GeneralDocs GeneralDocsItem = new GeneralDocs();
            ProductCancel ProductCancelItem = new ProductCancel();
            ProductsFlow ProductsFlowList = new ProductsFlow();

            //take authenticated user user
            var currentUser = context.Users.FirstOrDefault(u => u.login == userIdentityName);

            //configure doc.GeneralDocs
            GeneralDocsItem.ParamId1 = 0;
            GeneralDocsItem.Amount = 0;
            GeneralDocsItem.StaffId = staffId;

            GeneralDocsItem.Tdate = DateTime.Now; //For tdate and tdate1 balance reports
            GeneralDocsItem.Purpose = "საქონლის ჩამოწერა"; //set purpose to GeneralDocs (as default)
            GeneralDocsItem.DocType = 15; //set doc_type to GeneralDocs (საცალო გაყიდვა)
            GeneralDocsItem.UserId = currentUser.id; //set user_id to GeneralDocs
            GeneralDocsItem.CreateDate = DateTime.Now;
            GeneralDocsItem.Vat = 18; //set vat to GeneralDocs
            GeneralDocsItem.DocNum = context.GeneralDocs.Max(gd => gd.DocNum) + 1; //set doc_num + 1 to GeneralDocs
            GeneralDocsItem.ParamId2 = storeId; //set param_id2 to GeneralDocs (საწყობი)
            GeneralDocsItem.StatusId = 1; //set status_id to GeneralDocs (added from web interface)
            GeneralDocsItem.MakeEntry = true; //set make_entry to GeneralDocs (გატარებები)

            //configure doc.ProductCancel
            ProductCancelItem.AccountId = 0;
            ProductCancelItem.StaffId = 0;

            //configure doc.ProductsFlow
            ProductsFlowList.Amount = quantity;//docItem.GeneralDocsItem.Amount; //the same Amount as doc.GeneralDocs
            ProductsFlowList.Price = 0; //the same Amount as doc.GeneralDocs
            ProductsFlowList.ProductId = productId;
            ProductsFlowList.StoreId = storeId;
            ProductsFlowList.VatPercent = 18;
            ProductsFlowList.IsOrder = 0;
            ProductsFlowList.IsExpense = 0;
            ProductsFlowList.IsMove = 0;
            ProductsFlowList.UnitId = 1;
            ProductsFlowList.InId = unitId;
            ProductsFlowList.VendorId = 0;
            ProductsFlowList.OutId = 0;
            ProductsFlowList.ServiceStaffId = 0;
            ProductsFlowList.Coeff = -1;

            //add entities
            context.GeneralDocs.Add(GeneralDocsItem);
            context.ProductCancels.Add(ProductCancelItem);
            context.ProductsFlow.Add(ProductsFlowList);

            var saveResult = context.SaveChanges() >= 0;

            SaveEntriesFastCancel(GeneralDocsItem.Id);

            return GeneralDocsItem.Id;
        }

        public bool DeleteEntries(int general_id)
        {
            var entries = context.Entries.Where(e => e.GeneralId == general_id).ToList();

            entries.ForEach(e =>
            {
                context.Entries.Remove(e);
            });

            return context.SaveChanges() >= 0;
        }


        public List<Car> GetCars(int staff_id)
        {
            return context.Cars.Where(c => c.StaffId == staff_id).ToList();
        }

        public int FinaProvideService(int general_id, int? contragentId, double? amount, int? storeId, int staffId, double? quantity, int productId, int? unitId, string userIdentityName)
        {
            var gdc = context.GeneralDocs.FirstOrDefault(gd => gd.Id == general_id);
            var pdot = context.ProductOut.FirstOrDefault(po => po.GeneralId == general_id);
            var pdfl = context.ProductsFlow.FirstOrDefault(pf => pf.GeneralId == general_id);

            if (gdc != null && pdot != null && pdfl != null)
            {
                context.GeneralDocs.Remove(gdc);
                context.ProductOut.Remove(pdot);
                context.ProductsFlow.Remove(pdfl);

                DeleteEntries(gdc.Id);

                context.SaveChanges();
            }

            GeneralDocs GeneralDocsItem = new GeneralDocs();
            ProductOut ProductOutItem = new ProductOut();
            ProductsFlow ProductsFlowList = new ProductsFlow();

            //take authenticated user user
            var currentUser = context.Users.FirstOrDefault(u => u.login == userIdentityName);

            //configure doc.GeneralDocs
            GeneralDocsItem.ParamId1 = contragentId;
            GeneralDocsItem.Amount = amount;
            GeneralDocsItem.StaffId = staffId;

            GeneralDocsItem.Tdate = DateTime.Now; //For tdate and tdate1 balance reports
            GeneralDocsItem.Purpose = "მომსახურების გაწევა"; //set purpose to GeneralDocs (as default)
            GeneralDocsItem.DocType = 29; //set doc_type to GeneralDocs (მომსახურების გაწევა)
            GeneralDocsItem.UserId = currentUser.id; //set user_id to GeneralDocs
            GeneralDocsItem.CreateDate = DateTime.Now;
            GeneralDocsItem.Vat = 18; //set vat to GeneralDocs
            GeneralDocsItem.DocNum = context.GeneralDocs.Where(gd => gd.DocType == 29).Max(gd => gd.DocNum) + 1; //set doc_num + 1 to GeneralDocs
            GeneralDocsItem.ParamId2 = storeId; //set param_id2 to GeneralDocs (საწყობი)
            GeneralDocsItem.StatusId = 1; //set status_id to GeneralDocs (added from web interface)
            GeneralDocsItem.MakeEntry = true; //set make_entry to GeneralDocs (გატარებები)

            //configure doc.ProductOut
            ProductOutItem.StaffId = staffId;

            ProductOutItem.ResponsablePersonDate = DateTime.Now;
            ProductOutItem.Other = true;
            ProductOutItem.TransportCostPayer = 1;
            ProductOutItem.InvoiceBankId = 0;
            ProductOutItem.PayType = "1";
            //docItem.ProductOutItem[0].CheckStatus = 0; //TODO add?

            //configure doc.ProductsFlow
            ProductsFlowList.Amount = quantity;//docItem.GeneralDocsItem.Amount; //the same Amount as doc.GeneralDocs
            ProductsFlowList.Price = amount; //the same Amount as doc.GeneralDocs
            ProductsFlowList.ProductId = productId;
            ProductsFlowList.StoreId = storeId;
            ProductsFlowList.VatPercent = 18;
            ProductsFlowList.IsOrder = null;
            ProductsFlowList.IsExpense = null;
            ProductsFlowList.IsMove = null;
            ProductsFlowList.UnitId = 1;
            ProductsFlowList.InId = null;
            ProductsFlowList.VendorId = null;
            ProductsFlowList.OutId = null;
            ProductsFlowList.ServiceStaffId = 0;
            ProductsFlowList.Coeff = null;

            //add entities
            context.GeneralDocs.Add(GeneralDocsItem);
            context.ProductOut.Add(ProductOutItem);
            context.ProductsFlow.Add(ProductsFlowList);

            var saveResult = context.SaveChanges() >= 0;

            SaveEntriesFast(GeneralDocsItem.Id);

            return GeneralDocsItem.Id;
        }

        public List<ProductRestModel> GetProductRest(int store_id, string connString)
        {
            using (var _db = new DBContext(connString))
            {
                string sql = string.Format(@"SELECT  a.product_id AS Id, p.name AS Name, p.code AS Code, ISNULL(SUM(a.amount * CASE a.is_order WHEN 1 THEN -1 WHEN 0 THEN a.coeff END),0) AS Rest
                                         FROM doc.ProductsFlow AS a 
                                         INNER JOIN book.Products AS p ON p.id=a.product_id
                                         INNER JOIN doc.Generaldocs AS g ON g.id=a.general_id
                                         WHERE g.tdate <='{0}' AND ISNULL(g.is_deleted,0) = 0  AND a.is_expense=0 AND a.is_order IN(0,1) AND a.store_id={1} 
                                         GROUP BY  a.product_id,p.name,p.code
                                         HAVING ISNULL(SUM(a.amount * CASE a.is_order WHEN 1 THEN -1 WHEN 0 THEN a.coeff END),0) <>0
                                          ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), store_id);
                return _db.GetList<ProductRestModel>(sql);
            }
        }

        public List<ProductRestModel> GetProductFullRest(int product_id, string connString)
        {
            using (var _db = new DBContext(connString))
            {
                string sql = string.Format(@"SELECT TOP(1) a.product_id AS Id, p.name AS Name, p.code AS Code, ISNULL(SUM(a.amount * CASE a.is_order WHEN 1 THEN -1 WHEN 0 THEN a.coeff END),0) AS Rest
                                         FROM doc.ProductsFlow AS a 
                                         INNER JOIN book.Products AS p ON p.id=a.product_id
                                         INNER JOIN doc.Generaldocs AS g ON g.id=a.general_id
                                         WHERE g.tdate <='{0}' AND ISNULL(g.is_deleted,0) = 0  AND a.is_expense=0 AND a.is_order IN(0,1) AND a.product_id={1} 
                                         GROUP BY  a.product_id,p.name,p.code
                                         HAVING ISNULL(SUM(a.amount * CASE a.is_order WHEN 1 THEN -1 WHEN 0 THEN a.coeff END),0) <>0
                                          ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), product_id);
                return _db.GetList<ProductRestModel>(sql);
            }
        }

        public int FinaProductShipping(string conn_string,DateTime tdate, int bort_id, int staff_id, int user_id, ProductShippingViewModel model)
        {
            long doc_num = (context.GeneralDocs.Where(gd => gd.DocType == 77).Max(gd => gd.DocNum) ?? 0) + 1;

            using (DbContextTransaction tran = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {

                    string sql = @"SELECT  a.id, a.code, a.name,b.id AS unitID, b.name AS unitName,           
              CAST(ISNULL((SELECT SUM(pf.amount*pf.coeff)  FROM   doc.ProductsFlow pf  INNER JOIN doc.GeneralDocs g ON g.id=pf.general_id   WHERE  pf.is_order = 0 AND pf.is_expense=0 
              AND g.tdate<='" + tdate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" +
              " AND pf.product_id = a.id AND pf.store_id = " + bort_id + "), 0)as DEC(21,3)) AS restQuantity, " +
              " CAST(ROUND(ISNULL(( SELECT TOP 1 d.manual_val*curr.rate /1.0 " +
                      @" FROM  book.ProductPrices d INNER JOIN book.Currencies curr ON d.manual_currency_id= curr.id 
                       WHERE    d.price_id =3 " +
                      @" AND d.product_id = a.id), 0), 2" +
                      @" ) AS DECIMAL(21,2" +
                      @" ))AS productPrice, 
                               CAST(ROUND(
                     CAST(ROUND(ISNULL(( SELECT TOP 1 d.manual_val*curr.rate /1.0 " +
                      @" FROM  book.ProductPrices d INNER JOIN book.Currencies curr ON d.manual_currency_id= curr.id 
                       WHERE    d.price_id =3 " +
                      @" AND d.product_id = a.id), 0), 2" +
                      @" ) AS DECIMAL(21,2" +
                      @" ))
                      *
                     CAST(ISNULL((SELECT SUM(pf.amount*pf.coeff)  FROM   doc.ProductsFlow pf  INNER JOIN doc.GeneralDocs g ON g.id=pf.general_id   WHERE  pf.is_order = 0 AND pf.is_expense=0 
              AND g.tdate<='" + tdate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" +
            " AND pf.product_id = a.id AND pf.store_id = " + bort_id + "), 0)as DEC(21,3)), 2)" +
            " AS DECIMAL(21,2 )) AS SumPrice" +
            @" FROM    book.Products a 
             INNER JOIN book.Units b ON a.unit_id = b.id 

           WHERE a.is_hidden=0
             AND (CAST(ISNULL((SELECT SUM(pf.amount*pf.coeff)  FROM   doc.ProductsFlow pf  INNER JOIN doc.GeneralDocs g ON g.id=pf.general_id   WHERE  pf.is_order = 0 AND pf.is_expense=0 
             AND g.tdate<='" + tdate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" +
             " AND pf.product_id = a.id AND pf.store_id = " + bort_id + "), 0)as DEC(21,3)))>0";
                    DataTable productsData = new DBContext(conn_string).GetTableData(sql);
                    if (productsData.Rows.Count == 0)
                        return 0;
                    double full_amount = productsData.Rows.Cast<DataRow>().Sum(d => double.Parse(d["SumPrice"].ToString()));


                    GeneralDocs GeneralDocsItem = new GeneralDocs
                    {
                        Tdate = tdate,
                        DocNum = doc_num,
                        DocNumPrefix = "",
                        DocType = 77,
                        Purpose = "დისტრიბუციის ზედნადები № " + doc_num,
                        Vat = 0,
                        Amount = full_amount,
                        UserId = user_id,
                        ParamId1 = bort_id,
                        ParamId2 = 0,
                        StaffId = staff_id,
                        MakeEntry = false,
                        Uid = Guid.NewGuid().ToString(),
                    };
                    context.GeneralDocs.Add(GeneralDocsItem);

                    ProductShipping shipping = new ProductShipping
                    {
                        GeneralDoc = GeneralDocsItem,
                        DriverName = model.DriverName,
                        TransportEndPlace = model.TransportEndPlace,
                        TransporterIdNum = model.TransporterIdNum,
                        TransportModel = model.TransportModel,
                        TransportNumber = model.TransportNumber,
                        TransportStartPlace = model.TransportStartPlace
                    };
                    context.ProductShippings.Add(shipping);


                    foreach(DataRow row in productsData.Rows)
                    {
                        context.ProductShippingFlows.Add(new ProductShippingFlow
                        {
                            GeneralDoc = GeneralDocsItem,
                            Price = Convert.ToDouble(row["productPrice"]),
                            ProductId = Convert.ToInt32(row["id"]),
                            UnitId = Convert.ToInt32(row["unitID"]),
                            Rest = Convert.ToDouble(row["restQuantity"])
                        });
                    }

                    context.SaveChanges();
                    tran.Commit();

                    return GeneralDocsItem.Id;

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0;
                }
            }
        }

        public List<ProductShippingListViewModel> GetProductShippings(DateTime start_date, DateTime end_date)
        {
            using (FinaContext _db = new FinaContext())
            {
                return _db.Database.SqlQuery<ProductShippingListViewModel>(@"SELECT GeneralDocs.id, GeneralDocs.tdate,  ISNULL(NULLIF(GeneralDocs.waybill_num, ''), GeneralDocs.doc_num_prefix+CONVERT(NVARCHAR(20),GeneralDocs.doc_num))AS doc_num , 
GeneralDocs.purpose,  CONVERT(DECIMAL(38,2), ROUND(ROUND(CONVERT(decimal(38,14),GeneralDocs.amount),10),2))as amount, 
Currencies.code AS currency_name,psh.waybill_id,psh.waybill_status, DocStatusType.name AS rs_status, s.name AS bort_name, st.name AS staff_name, psh.transp_start_place FROM 
doc.GeneralDocs as GeneralDocs
INNER JOIN book.Currencies AS Currencies ON GeneralDocs.currency_id = Currencies.id 
LEFT JOIN book.Staff AS st ON st.id=GeneralDocs.staff_id
INNER JOIN book.Stores AS s ON s.id=GeneralDocs.param_id1
INNER JOIN doc.ProductShipping AS psh ON psh.general_id=GeneralDocs.id
INNER JOIN config.DocTypes AS DocTypes ON GeneralDocs.doc_type = DocTypes.id
INNER JOIN config.DocStatusTypes DocStatusType on DocStatusType.tag = DocTypes.tag  AND DocStatusType.status_id= CASE ISNULL(psh.waybill_status, -1)  when -2 then 4 when -1 then 1  when 0 then 1  when 1 then 2  when 2 then 3 when 8 then 6 else 5 end
WHERE GeneralDocs.tdate BETWEEN @start_date AND @end_date ORDER BY GeneralDocs.tdate", new SqlParameter("@start_date", start_date), new SqlParameter("@end_date", end_date)).ToList();
            }
        }

        public List<ProductShippingFlowListView> GetProductShippingFlow(int general_id)
        {
            return context.ProductShippingFlows.Where(c => c.GeneralId == general_id)
                .Select(c => new ProductShippingFlowListView
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    GeneralId = c.GeneralId.Value,
                    Price = c.Price,
                    ProductName = c.Product.Name,
                    UnitName = c.Unit.Name,
                    Rest = c.Rest,
                    ProductCode = c.Product.Code
                }).ToList();
        }

        public bool DeleteOut(int general_id)
        {
            using (var tran = context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    context.Database.ExecuteSqlCommand("DELETE FROM doc.Entries WHERE general_id=" + general_id);
                    context.Database.ExecuteSqlCommand("DELETE FROM doc.ProductsFlow WHERE general_id=" + general_id);
                    context.Database.ExecuteSqlCommand("DELETE FROM doc.ProductOut WHERE general_id=" + general_id);
                    context.Database.ExecuteSqlCommand("DELETE FROM doc.ServiceOutStaffs WHERE general_id=" + general_id);
                    context.Database.ExecuteSqlCommand("DELETE FROM doc.GeneralDocs WHERE id=" + general_id);

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
            }
        }
    }


}
