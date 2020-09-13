using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Sale;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Sale.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Sale;

namespace AdminSoftware.Areas.Sale.Controllers
{
    public class ProductController : BaseController
    {
        private readonly AutoNumberBll _autoNumberBll;
        private readonly ProductBll _productBll;
        private readonly UserBll _userBll;

        public ProductController()
        {
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _productBll = SingletonIpl.GetInstance<ProductBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        [SaleFilter]
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null)
                    .Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString()));
            return View();
        }

        public JsonResult Products()
        {
            return Json(_productBll.GetProducts(null, null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Product(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Product
                {
                    ProductCode = _autoNumberBll.GetAutoNumber(AutoNumberSale.SP.ToString()),
                    CreateBy = UserLogin.UserId,
                    ProductId = 0,
                    IsActive = true
                });
            }
            return PartialView(_productBll.GetProduct(long.Parse(id)));
        }

        public JsonResult Save(ProductModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
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

                if (model.ProductId <= 0)
                {
                    model.CreateDate = DateTime.Now;
                    model.CreateBy = UserLogin.UserId;
                    model.ProductCode = AutoNumberSale.SP.ToString();
                    if (_productBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (_productBll.Update(model.ToObject()))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = 0,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                if (_productBll.Delete(long.Parse(id)))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageDeleteSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}