using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Kpi;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Kpi;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class ApprovedComplainController : BaseController
    {
        private readonly ComplainBll _complainBll;
        private readonly UserBll _userBll;

        public ApprovedComplainController()
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

        public JsonResult Complains(DateTime? fromDate, DateTime? toDate, int action)
        {
            return Json(_complainBll.GetComplains(fromDate, toDate, null, action), JsonRequestBehavior.AllowGet);
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

        public JsonResult Save(int action, string complainId)
        {
            var complain = _complainBll.GetComplain(complainId);
            if (complain == null)
            {
                return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
            }
            if (action == 2)
            {
                complain.Status = 2;
                complain.ConfirmedBy = UserLogin.UserId;
                complain.ConfirmedDate = DateTime.Now;
                if (_complainBll.Update(complain))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
            }
            if (action == 3)
            {
                complain.Status = 3;
                complain.ConfirmedBy = UserLogin.UserId;
                complain.ConfirmedDate = DateTime.Now;
                if (_complainBll.Update(complain))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
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