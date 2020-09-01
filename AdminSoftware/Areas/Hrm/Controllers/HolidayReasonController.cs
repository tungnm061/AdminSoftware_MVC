using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class HolidayReasonController : BaseController
    {
        private readonly HolidayReasonBll _holidayReasonBll;

        public HolidayReasonController()
        {
            _holidayReasonBll = SingletonIpl.GetInstance<HolidayReasonBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult HolidayReasons()
        {
            return Json(_holidayReasonBll.GetHolidayReasons(null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult HolidayReason(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new HolidayReason {IsActive = true});
            }
            return PartialView(_holidayReasonBll.GetHolidayReason(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult Save(HolidayReasonModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message =
                        ModelState.Where(modelState => modelState.Value.Errors.Count > 0)
                            .Select(x => x.Value)
                            .FirstOrDefault();
                    return
                        Json(
                            new
                            {
                                Status = 0,
                                Message =
                                    message == null ? MessageAction.ModelStateNotValid : message.Errors[0].ErrorMessage
                            },
                            JsonRequestBehavior.AllowGet);
                }
                var holidayReason = _holidayReasonBll.GetHolidayReason(model.ReasonCode);
                if (model.HolidayReasonId <= 0)
                {
                    if (holidayReason != null)
                    {
                        return Json(new {Status = 0, Message = "Mã lý do đã tồn tại trong hệ thống!"},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_holidayReasonBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (holidayReason != null && holidayReason.HolidayReasonId != model.HolidayReasonId)
                {
                    return Json(new {Status = 0, Message = "Mã lý do đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_holidayReasonBll.Update(model.ToObject()))
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
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
                if (_holidayReasonBll.Delete(int.Parse(id)))
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