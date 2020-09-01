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
    public class ShiftWorkController : BaseController
    {
        private readonly ShiftWorkBll _shiftWorkBll;

        public ShiftWorkController()
        {
            _shiftWorkBll = SingletonIpl.GetInstance<ShiftWorkBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ShiftWorks()
        {
            return Json(_shiftWorkBll.GetShiftWorks(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShiftWork(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new ShiftWork());
            return PartialView(_shiftWorkBll.GetShiftWork(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult Save(ShiftWorkModel model)
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
                var shiftWork = _shiftWorkBll.GetShiftWork(model.ShiftWorkCode);
                if (model.ShiftWorkId == 0)
                {
                    if (shiftWork != null)
                    {
                        return Json(new {Status = 0, Message = "Mã ca làm việc đã tồn tại trong hệ thống!"},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_shiftWorkBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (shiftWork != null && shiftWork.ShiftWorkId != model.ShiftWorkId)
                {
                    return Json(new {Status = 0, Message = "Mã ca làm việc đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_shiftWorkBll.Update(model.ToObject()))
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

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}