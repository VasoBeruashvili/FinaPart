using FinaPart.Models;
using FinaPart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaPart
{
    public interface IFinaLogic
    {
        Users GetUser(string identityName);

        List<Users> GetUsers();

        List<ProductViewModel> GetProducts(string path);

        IOrderedQueryable GetSubContragents();

        Contragents GetContragentById(int id);

        Contragents GetContragentByCode(string code);

        SubContragent GetSubContragentById(int id);

        Users GetUserById(int id);

        Companies GetCompany();

        Products GetProductById(int id);

        Units GetUnitById(int id);

        Store GetStoreById(int id);

        Staffs GetStaffById(int id);

        List<Staffs> GetStaffsList();

        List<Store> GetStoresList();

        ProductPrices GetProductPrice(int productId, int priceId);

        GroupProduct GetGroupProductById(int id);

        IQueryable<Products> SearchProducts(string name);

        List<ProductViewModel> SearchProductsWithImages(string name);

        ProductViewModel GetProductWithImagesById(int id);

        ProductViewModel GetProductWithImagesByCode(string code);

        ProductMove GetProductMove(string bortNumber);

        GeneralDocs GetGeneralDocById(int id);

        List<ContragentContact> GetContragentContactsByContragentId(int id);

        Currencies GetCurrencyById(int id);

        List<Store> GetOrderedStores();

        Dictionary<int, double> GetProductRestOriginalWithOrder(List<int> product_ids, int store_id, DateTime toDate, string connString);

        List<ProductRestModel> GetProductRest(int store_id, string connString);

        List<ProductRestModel> GetProductFullRest(int product_id, string connString);

        bool IsCompanyVat(DateTime date);

        bool SaveEntriesFast(int general_id);

        bool SaveEntriesFastCancel(int general_id);

        Dictionary<int, double> GetProductSelfCostAverage(Dictionary<int, double> IdList, DateTime ActionDate);

        KeyValuePair<int, string> FinaMove(int general_id, DateTime tdate, int store_from, int store_to, int staff_id, int user_id, bool make_entry, ProductMoveViewModel moveModel, List<ProductsFlowViewModel> flowsModel);

        List<ProductMoveListViewModel> GetProductMoves(DateTime start_date, DateTime end_date);

        ProductMoveViewModel GetProductMove(int general_id);

        string GetProductAccountByPath(string path, int level);

        int FinaRealization(int general_id, int? contragentId, double? amount, int? storeId, int staffId, double? quantity, int productId, int? unitId, int? rsSaleType, string userIdentityName);

        int FinaProductCancel(int general_id, int? storeId, int staffId, double? quantity, int productId, int? unitId, string userIdentityName);

        bool DeleteEntries(int general_id);

        int FinaProvideService(int general_id, int? contragentId, double? amount, int? storeId, int staffId, double? quantity, int productId, int? unitId, string userIdentityName);

        List<Car> GetCars(int staff_id);

        int FinaProductShipping(string conn_string, DateTime tdate, int bort_id, int staff_id, int user_id, ProductShippingViewModel model);

        List<ProductShippingListViewModel> GetProductShippings(DateTime start_date, DateTime end_date);

        List<ProductShippingFlowListView> GetProductShippingFlow(int general_id);

        ProductShipping GetProductShipping(string bortNumber);

        bool DeleteOut(int general_id);
    }
}
