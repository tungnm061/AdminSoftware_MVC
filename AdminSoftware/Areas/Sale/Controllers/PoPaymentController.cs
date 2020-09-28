using System;
using System.ComponentModel;
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
    public class PoPaymentController : BaseController
    {
        private readonly AutoNumberBll _autoNumberBll;
        private readonly PoPaymentBll _poPaymentBll;
        private readonly UserBll _userBll;

        public PoPaymentController()
        {
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _poPaymentBll = SingletonIpl.GetInstance<PoPaymentBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        [SaleFilter]
        public ActionResult Index()
        {
            ViewBag.Status = from StatusPoEnụm s in Enum.GetValues(typeof(StatusPoEnụm))
                             let singleOrDefault =
                                 (DescriptionAttribute)
                                 s.GetType()
                                     .GetField(s.ToString())
                                     .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                     .SingleOrDefault()
                             where singleOrDefault != null
                             select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            ViewBag.Users =
                _userBll.GetUsers(null)
                    .Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString()));
            return View();
        }

        public JsonResult PoPayments(int? status, DateTime? fromDate, DateTime? toDate)
        {
            var listObj = _poPaymentBll.GetPoPayments(status, fromDate, toDate);
            return Json(listObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PoPayment(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new PoPayment
                {
                    PoPaymentId = 0,
                    RateMoney = 1,
                    TradingDate = DateTime.Now,
                    TradingBy = UserLogin.UserId,
                    Status = 1
                });
            }
            return PartialView(_poPaymentBll.GetPoPayment(long.Parse(id)));
        }

        public JsonResult Save(PoPayment model)
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

                if (model.Status == 3)
                {
                    return Json(new { Status = -1, Message = MessageAction.DuLieuKhongTheUpdate },
                        JsonRequestBehavior.AllowGet);
                }

                model.TypeMoney = 1;
                if (model.PoPaymentId <= 0)
                {
                    model.Status = 1;
                    //model.Path = UserLogin.Path;
                    //if (string.IsNullOrEmpty(model.Path))
                    //{
                    //    return Json(new { Status = -1, Message = MessageAction.TaiKhoanKhongCoQuyenTao },
                    //        JsonRequestBehavior.AllowGet);
                    //}
                    model.CreateDate = DateTime.Now;
                    model.CreateBy = UserLogin.UserId;
                    if (_poPaymentBll.Insert(model) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }


                if (_poPaymentBll.Update(model))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
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
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_poPaymentBll.Delete(long.Parse(id)))
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

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}