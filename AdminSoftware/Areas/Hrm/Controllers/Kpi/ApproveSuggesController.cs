using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Kpi;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class ApproveSuggesController : BaseController
    {
        private readonly SuggesWorkBll _suggesWorkBll;
        private readonly WorkDetailBll _workDetailBll;
        private readonly WorkStreamDetailBll _workStreamDetailBll;
        private readonly DepartmentBll _departmentBll;
        public ApproveSuggesController()
        {
            _suggesWorkBll = SingletonIpl.GetInstance<SuggesWorkBll>();
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _workStreamDetailBll = SingletonIpl.GetInstance<WorkStreamDetailBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.StatusWorkDetail = from StatusWorkDetail s in Enum.GetValues(typeof (StatusWorkDetail))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            ViewBag.WorkDetailStatusEnum = from WorkDetailStatusEnum s in Enum.GetValues(typeof (WorkDetailStatusEnum))
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

        public JsonResult SuggesWorks(DateTime? fromDate, DateTime? toDate, int action)
        {
            try
            {
                var department = _departmentBll.GetDepartment(UserLogin.DepartmentId ?? 0);
                if (department == null || string.IsNullOrEmpty(department.Path))
                {
                    return
                    Json(_workDetailBll.GetWorkDetailsByPath("", action, fromDate, toDate),
                        JsonRequestBehavior.AllowGet);
                }
                var path = department.Path;
                return
                    Json(_workDetailBll.GetWorkDetailsByPath(department.Path, action, fromDate, toDate),
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return
                    Json(new List<WorkDetail>(),
                        JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SuggesWork(string id)
        {
            return PartialView(_workDetailBll.GetWorkDetail(id, null));
        }

        public JsonResult Save(string id, int action, int type)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || (action != 7 && action != 8))
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
                }
                var workDetail = _workDetailBll.GetWorkDetail(id, type);
                if (workDetail == null)
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
                }
                if (workDetail.ApprovedFisnishBy != null)
                {
                    return
                        Json(
                            new {Status = 0, Message = "Công việc này đã được xác nhận hoàn thành không thể cập nhật!"},
                            JsonRequestBehavior.AllowGet);
                }
                if (action == 7)
                {
                    if (workDetail.WorkType == (int) StatusWorkDetail.SuggesWork)
                    {
                        var suggesWork = _suggesWorkBll.GetSuggesWork(workDetail.WorkDetailId);
                        suggesWork.VerifiedBy = UserLogin.UserId;
                        suggesWork.VerifiedDate = DateTime.Now;
                        if (_suggesWorkBll.Update(suggesWork))
                        {
                            return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                                JsonRequestBehavior.AllowGet);
                        }
                        return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (workDetail.WorkType == (int) StatusWorkDetail.WorkStreamDetail)
                    {
                        var workStreamDetail = _workStreamDetailBll.GetWorkStreamDetail(workDetail.WorkDetailId);
                        workStreamDetail.VerifiedBy = UserLogin.UserId;
                        workStreamDetail.VerifiedDate = DateTime.Now;
                        if (_workStreamDetailBll.Update(workStreamDetail))
                        {
                            return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                                JsonRequestBehavior.AllowGet);
                        }
                        return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                            JsonRequestBehavior.AllowGet);
                    }
                }
                if (action == 8)
                {
                    if (workDetail.WorkType == (int) StatusWorkDetail.SuggesWork)
                    {
                        var suggesWork = _suggesWorkBll.GetSuggesWork(workDetail.WorkDetailId);
                        suggesWork.VerifiedBy = null;
                        suggesWork.VerifiedDate = null;
                        if (_suggesWorkBll.Update(suggesWork))
                        {
                            return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                                JsonRequestBehavior.AllowGet);
                        }
                        return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (workDetail.WorkType == (int) StatusWorkDetail.WorkStreamDetail)
                    {
                        var workStreamDetail = _workStreamDetailBll.GetWorkStreamDetail(workDetail.WorkDetailId);
                        workStreamDetail.VerifiedBy = null;
                        workStreamDetail.VerifiedDate = null;
                        if (_workStreamDetailBll.Update(workStreamDetail))
                        {
                            return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                                JsonRequestBehavior.AllowGet);
                        }
                        return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                            JsonRequestBehavior.AllowGet);
                    }
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