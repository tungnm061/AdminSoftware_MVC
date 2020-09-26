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

namespace AdminSoftware.Areas.Sale.Controllers
{
    public class OrderController : BaseController
    {

        private readonly OrderBll _orderBll;
        private readonly GmailBll _gmailBll;
        private readonly CountryBll _countryBll;
        private readonly ProductBll _productBll;
        private readonly ProducerBll _producerBll;

        public OrderController()
        {
            _orderBll = SingletonIpl.GetInstance<OrderBll>();
            _gmailBll = SingletonIpl.GetInstance<GmailBll>();
            _countryBll = SingletonIpl.GetInstance<CountryBll>();
            _productBll = SingletonIpl.GetInstance<ProductBll >();
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
        public ActionResult Index()
        {
            var countries = _countryBll.GetCountries();
            var gmails = _gmailBll.GetGmails();
            var products = _productBll.GetProducts(true,null);
            var producers = _producerBll.GetProducers();

            ViewBag.Producers =
                producers.Select(x => new KendoForeignKeyModel { value = x.ProducerId.ToString(), text = x.ProducerName });

            ViewBag.Products =
                products.Select(x => new KendoForeignKeyModel { value = x.ProductId.ToString(), text = x.ProductName });

            ViewBag.Countries =
                countries.Select(x => new KendoForeignKeyModel { value = x.CountryId.ToString(), text = x.CountryName });

            ViewBag.Gmails =
                gmails.Select(x => new KendoForeignKeyModel { value = x.Id.ToString(), text = x.FullName });

            ViewBag.Sizes = from SizeEnum s in Enum.GetValues(typeof(SizeEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                    s.GetType()
                        .GetField(s.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };

            ViewBag.Colors = from ColorEnum s in Enum.GetValues(typeof(ColorEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                    s.GetType()
                        .GetField(s.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };

            ViewBag.Status = from StatusOrderEnụm s in Enum.GetValues(typeof(StatusOrderEnụm))
                let singleOrDefault =
                    (DescriptionAttribute)
                    s.GetType()
                        .GetField(s.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };

            ViewBag.KeySearchs = from KeySeachOrderEnụm s in Enum.GetValues(typeof(KeySeachOrderEnụm))
                let singleOrDefault =
                    (DescriptionAttribute)
                    s.GetType()
                        .GetField(s.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            return View();
        }

        public JsonResult Orders(int? keySearch, int? statusSearch, DateTime? fromDate, DateTime? toDate,int ? gmailId)
        {
            return Json(_orderBll.GetOrders(keySearch, statusSearch,null, fromDate, toDate,true, gmailId),
                JsonRequestBehavior.AllowGet);
        }
        public ActionResult Order(string id)
        {
            OrderDetailsInMemory = null;
            Order obj;
            if (!string.IsNullOrEmpty(id))
            {
                obj = _orderBll.GetOrder(long.Parse(id));
                OrderDetailsInMemory = obj.OrderDetails;
            }
            else
            {
                obj = new Order
                {
                    OrderId = 0,
                    StartDate = DateTime.Now,
                    Status = 1,
                    ProducerId = 1
                };
            }
            return PartialView(obj);
        }

        public ActionResult Save(Order model)
        {
            try
            {
                if (!OrderDetailsInMemory.Any() || model == null)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
                if (!ModelState.IsValid)
                {
                    var message = ModelState.Where(modelState => modelState.Value.Errors.Count > 0)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                    return Json(new
                    {
                        Status = 0,
                        Message = message == null ? MessageAction.ModelStateNotValid : message.Errors[0].ErrorMessage
                    }, JsonRequestBehavior.AllowGet);
                }

                model.OrderDetails = OrderDetailsInMemory;
                model.TotalPrince = model.OrderDetails.Sum(x => x.Price) - model.ShipMoney;
                if (model.OrderId == 0)
                {
                    model.CreateBy = UserLogin.UserId;
                    model.CreateDate = DateTime.Now;

                    if (_orderBll.Insert(model))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                model.UpdateBy = UserLogin.UserId;
                model.UpdateDate = DateTime.Now;
                if (!_orderBll.Update(model))
                {
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult OrderDetails()
        {
            return Json(OrderDetailsInMemory, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderDetail(string id)
        {
            ViewBag.Status = from StatusOrderEnụm s in Enum.GetValues(typeof(StatusOrderEnụm))
                let singleOrDefault =
                    (DescriptionAttribute)
                    s.GetType()
                        .GetField(s.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new OrderDetail
                {
                    OrderDetailId = Guid.Empty.ToString(),
                    Quantity = 1,
                    Color = 1,
                    Size = 1
                });
            }
            return PartialView(OrderDetailsInMemory.Find(x => x.OrderDetailId == id));
        }

        public ActionResult SaveDetail(OrderDetail model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
                if (!ModelState.IsValid)
                {
                    var message = ModelState.Where(modelState => modelState.Value.Errors.Count > 0)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                    return Json(new
                    {
                        Status = 0,
                        Message = message == null ? MessageAction.ModelStateNotValid : message.Errors[0].ErrorMessage
                    }, JsonRequestBehavior.AllowGet);
                }

                var orderDetails = OrderDetailsInMemory;
                var orderDetail = orderDetails.Find(x => x.OrderDetailId == model.OrderDetailId);
                var product = _productBll.GetProduct(model.ProductId);
                if (orderDetail == null)
                {
                    if (product == null)
                    {
                        return Json(new { Status = 0, Message = "Sản phẩm không tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    orderDetails.Add(new OrderDetail
                    {
                        OrderDetailId = Guid.NewGuid().ToString(),
                        ProductId = product.ProductId,
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        UnitPrince    = product.Price,
                        Size = model.Size,
                        Color = model.Color,
                        Quantity = model.Quantity,
                        Price = model.Quantity * product.Price
                    });
                }
                else
                {
                    orderDetail.ProductId = model.ProductId;
                    orderDetail.ProductCode = product.ProductCode;
                    orderDetail.ProductName = product.ProductName;
                    orderDetail.UnitPrince = product.Price;
                    orderDetail.Size = model.Size;
                    orderDetail.Color = model.Color;
                    orderDetail.Quantity = model.Quantity;
                    orderDetail.Price = model.Quantity * product.Price;

                }
                OrderDetailsInMemory = orderDetails;
                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Thêm sản phẩm thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);

                if (_orderBll.Delete(long.Parse(id)))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageDeleteSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteDetail(string id)
        {
            try
            {
                var orderDetails = OrderDetailsInMemory;
                var orderDetailDelete = orderDetails.Find(x => x.OrderDetailId == id);

                orderDetails.Remove(orderDetailDelete);
                OrderDetailsInMemory = orderDetails;

                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Xóa sản phẩm thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ClearSession()
        {
            return null;
        }

    }
}