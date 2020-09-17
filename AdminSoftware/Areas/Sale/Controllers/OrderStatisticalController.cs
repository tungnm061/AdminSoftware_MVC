using System;
using System.Web.Mvc;
using BusinessLogic.Sale;
using Core.Singleton;
using AdminSoftware.Controllers;
using Entity.Sale;
using System.Collections.Generic;
using System.Linq;
using AdminSoftware.Models;
using BusinessLogic.System;
using Core.Enum;
using System.ComponentModel;
using Core.Helper.Logging;
using Entity.Kpi;
using AdminSoftware.Areas.Sale.Models;
using Entity.System;

namespace AdminSoftware.Areas.Sale.Controllers
{
    public class OrderStatisticalController : BaseController
    {

        private readonly OrderBll _orderBll;
        private readonly GmailBll _gmailBll;
        private readonly CountryBll _countryBll;
        private readonly ProductBll _productBll;
        private readonly ProducerBll _producerBll;

        public OrderStatisticalController()
        {
            _orderBll = SingletonIpl.GetInstance<OrderBll>();
            _gmailBll = SingletonIpl.GetInstance<GmailBll>();
            _countryBll = SingletonIpl.GetInstance<CountryBll>();
            _productBll = SingletonIpl.GetInstance<ProductBll>();
            _producerBll = SingletonIpl.GetInstance<ProducerBll>();

        }

        private List<OrderDetail> OrderDetailsInMemory
        {
            get
            {
                if (Session["OrderDetailsInMemory"] == null)
                    return new List<OrderDetail>();
                return (List<OrderDetail>)Session["OrderDetailsInMemory"];
            }
            set { Session["OrderDetailsInMemory"] = value; }
        }

        [SaleFilter]
        public ActionResult Index(DateTime? fromDate, DateTime? toDate)
        {
            var gmails = _gmailBll.GetGmails();
            var producers = _producerBll.GetProducers();

            ViewBag.Producers =
                producers.Select(x => new KendoForeignKeyModel { value = x.ProducerId.ToString(), text = x.ProducerName });


            ViewBag.Gmails =
                gmails.Select(x => new KendoForeignKeyModel { value = x.Id.ToString(), text = x.FullName });

            var listDay = new List<String>();
            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (toDate == null)
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);
            //var listDay = new List<String>();
            //if (fromDate == null)
            //    fromDate =  DateTime.Now.AddDays(-7);
            //if (toDate == null)
            //    toDate = DateTime.Now;
            ViewBag.FromDate = fromDate.Value.ToString("dd/MM/yyyy");
            ViewBag.ToDate = toDate.Value.ToString("dd/MM/yyyy");
            for (var start = fromDate; start <= toDate; start = start.Value.AddDays(1))
            {
                listDay.Add(start.Value.ToString("dd/MM/yyyy"));
            }
            ViewBag.Days = listDay;

            return View();
        }

        public JsonResult Orders(DateTime fromDate , DateTime toDate )
        {
            List<Gmail> gmails = _gmailBll.GetGmails();
            var listOrderStatistical = new List<OrderStatistical>();
            foreach (var item in gmails)
            {
                var obj = new OrderStatistical();
                obj.GmailId = item.Id;
                var listDetail = new List<StatisticalDetail>();
                for (var start = fromDate; start <= toDate; start = start.AddDays(1))
                {
                    var orders = _orderBll.GetOrders(1, null, null, start, start, true, item.Id);
                    var objDetail = new StatisticalDetail();

                    objDetail.TotalOrder = orders.Count();
                    objDetail.DateDay = start;
                    objDetail.FinishOrder = orders.Where(x => x.Status == 2).ToList().Count();
                    objDetail.CancelOrder = orders.Where(x => x.Status == 3).ToList().Count();
                    listDetail.Add(objDetail);
                    obj.TotalOrderPrice += orders.Where(x => x.Status != 3).ToList().Sum(x => x.TotalPrince);
                    obj.FinishOrderPrice += orders.Where(x => x.Status == 2).ToList().Sum(x => x.TotalPrince);
                    obj.TotalOrder += objDetail.TotalOrder;
                    obj.TotalFinish += objDetail.FinishOrder;
                    obj.TotalCancel += objDetail.CancelOrder;

                }

                obj.StatisticalDetails = listDetail;
                listOrderStatistical.Add(obj);

            }
            return Json(listOrderStatistical,
                JsonRequestBehavior.AllowGet);
        }
        

    }
}