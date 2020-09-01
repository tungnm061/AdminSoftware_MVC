using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Kpi;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.System.Models.Kpi;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Kpi;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class ComplainController : BaseController
    {
        private readonly ComplainBll _complainBll;
        private readonly UserBll _userBll;

        public ComplainController()
        {
            _complainBll = SingletonIpl.GetInstance<ComplainBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.StatusComplain = from StatusComplain s in Enum.GetValues(typeof (StatusComplain))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            return View();
        }

        public JsonResult Complains(DateTime? fromDate, DateTime? toDate)
        {
            return Json(_complainBll.GetComplains(fromDate, toDate, UserLogin.UserId, 0), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Complain(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Complain
                {
                    ComplainId = Guid.Empty.ToString(),
                    CreateDate = DateTime.Now,
                    CreateBy = UserLogin.UserId,
                    Status = 1
                });
            }
            return PartialView(_complainBll.GetComplain(id));
        }

        public ActionResult Employee()
        {
            return PartialView(_userBll.GetUsers(true));
        }

        public JsonResult Save(ComplainModel model)
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

            if (string.IsNullOrEmpty(model.ComplainId) || model.ComplainId == Guid.Empty.ToString())
            {
                model.ComplainId = Guid.NewGuid().ToString();
                model.CreateDate = DateTime.Now;
                if (_complainBll.Insert(model.ToObject()))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
            }
            if (model.ConfirmedBy != null)
            {
                Json(
                    new
                    {
                        Status = 0,
                        Message = "Phàn nàn đã được trưởng phòng duyệt không được thay đổi!"
                    },
                    JsonRequestBehavior.AllowGet);
            }

            if (_complainBll.Update(model.ToObject()))
            {
                return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess}, JsonRequestBehavior.AllowGet);
            }
            return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                var complain = _complainBll.GetComplain(id);
                if (complain.ConfirmedBy != null)
                {
                    return
                        Json(
                            new
                            {
                                Status = 0,
                                Message = "Phàn nàn đã được trưởng phòng duyệt không được xóa!"
                            },
                            JsonRequestBehavior.AllowGet);
                }
                if (_complainBll.Delete(id))
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