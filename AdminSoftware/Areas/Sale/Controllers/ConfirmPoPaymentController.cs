using System;
using System.Collections.Generic;
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
using Entity.Common;
using Entity.Sale;

namespace AdminSoftware.Areas.Sale.Controllers
{
    public class ConfirmPoPaymentController : BaseController
    {
        private readonly AutoNumberBll _autoNumberBll;
        private readonly PoPaymentBll _poPaymentBll;
        private readonly UserBll _userBll;

        public ConfirmPoPaymentController()
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
                    TradingDate = DateTime.Now
                });
            }
            return PartialView(_poPaymentBll.GetPoPayment(long.Parse(id)));
        }

        public JsonResult Save(List<int> listId, int status)
        {
            try
            {
                if (listId == null || listId.Count == 0)
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);

                List<PoPayment> listObj = new List<PoPayment>();
                listId.ForEach(x =>
                {
                    listObj.Add(new PoPayment
                    {
                        PoPaymentId = x,
                        Status = (byte)status,
                        ConfirmBy = UserLogin.UserId,
                        ConfirmDate = DateTime.Now
                    });
                });
                BizResult rs = _poPaymentBll.UpdateConfirm(listObj);
                if (rs.Status > 0)
                {
                    return Json(new { Status = rs.Status, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = -1, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveDetail(int poPaymentId, int status)
        {
            try
            {
                if (poPaymentId == 0)
                {
                    return Json(new { Status = -1, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                }
                PoPayment model = new PoPayment
                {
                    ConfirmBy = UserLogin.UserId,
                    ConfirmDate = DateTime.Now,
                    Status = (byte)status,
                    PoPaymentId = poPaymentId
                };

                var rs = _poPaymentBll.UpdateConfirmDetail(model);
                if (rs)
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = -1, Message = MessageAction.MessageActionFailed },
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