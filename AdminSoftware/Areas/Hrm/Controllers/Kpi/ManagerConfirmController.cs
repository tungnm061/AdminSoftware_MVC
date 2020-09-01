using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Kpi;
using Newtonsoft.Json;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class ManagerConfirmController : BaseController
    {
        private readonly AssignWorkBll _assignWorkBll;
        private readonly DepartmentBll _departmentBll;
        private readonly SuggesWorkBll _suggesWorkBll;
        private readonly WorkDetailBll _workDetailBll;
        private readonly WorkPlanDetailBll _workPlanDetailBll;
        private readonly WorkPointConfigBll _workPointConfigBll;
        private readonly WorkStreamDetailBll _workStreamDetailBll;

        public ManagerConfirmController()
        {
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _workPlanDetailBll = SingletonIpl.GetInstance<WorkPlanDetailBll>();
            _workStreamDetailBll = SingletonIpl.GetInstance<WorkStreamDetailBll>();
            _assignWorkBll = SingletonIpl.GetInstance<AssignWorkBll>();
            _suggesWorkBll = SingletonIpl.GetInstance<SuggesWorkBll>();
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _workPointConfigBll = SingletonIpl.GetInstance<WorkPointConfigBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.WorkPointType = from WorkPointType s in Enum.GetValues(typeof (WorkPointType))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select
                    new KendoForeignKeyModel {value = singleOrDefault.Description, text = singleOrDefault.Description};
            ViewBag.WorkDetailStatusEnum = from WorkDetailStatusEnum s in Enum.GetValues(typeof (WorkDetailStatusEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};

            ViewBag.StatusWorkDetail = from StatusWorkDetail s in Enum.GetValues(typeof (StatusWorkDetail))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            return View();
        }

        public JsonResult WorkDetails(int action, DateTime? fromDate, DateTime? toDate)
        {
            return Json(_workDetailBll.GetWorkDetails(null, action, fromDate, toDate), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAll(List<WorkDetail> selectedDataItems)
        {
            if (selectedDataItems == null)
            {
                return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
            }
            foreach (var item in selectedDataItems)
            {
                item.ApprovedFisnishBy = UserLogin.UserId;
                item.ApprovedFisnishDate = DateTime.Now;
                item.Status = 4;
                if (item.WorkType == (int)StatusWorkDetail.WorkPlanDetail)
                {
                    var workPlanDetail = _workPlanDetailBll.GetWorkPlanDetail(item.WorkDetailId);
                    workPlanDetail.Status = item.Status;
                    workPlanDetail.ApprovedFisnishBy = item.ApprovedFisnishBy;
                    workPlanDetail.ApprovedFisnishDate = item.ApprovedFisnishDate;
                    if (!_workPlanDetailBll.Update(workPlanDetail))
                    {
                        return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);

                    }
                }
                if (item.WorkType == (int)StatusWorkDetail.AssignWork)
                {
                    var assignWork = _assignWorkBll.GetAssignWork(item.WorkDetailId);
                    assignWork.Status = (byte)item.Status;
                    assignWork.ApprovedFisnishBy = item.ApprovedFisnishBy;
                    assignWork.ApprovedFisnishDate = item.ApprovedFisnishDate;
                    if (!_assignWorkBll.Update(assignWork))
                    {
                        return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);

                    }
                }
                if (item.WorkType == (int)StatusWorkDetail.WorkStreamDetail)
                {
                    var workStreamDetail = _workStreamDetailBll.GetWorkStreamDetail(item.WorkDetailId);
                    workStreamDetail.Status = item.Status;
                    workStreamDetail.ApprovedFisnishBy = item.ApprovedFisnishBy;
                    workStreamDetail.ApprovedFisnishDate = item.ApprovedFisnishDate;
                    if (!_workStreamDetailBll.Update(workStreamDetail))
                    {
                        return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);

                    }
                }
                if (item.WorkType == (int)StatusWorkDetail.SuggesWork)
                {
                    var suggesWork = _suggesWorkBll.GetSuggesWork(item.WorkDetailId);
                    suggesWork.Status = (byte)item.Status;
                    suggesWork.ApprovedFisnishBy = item.ApprovedFisnishBy;
                    suggesWork.ApprovedFisnishDate = item.ApprovedFisnishDate;
                    if (!_suggesWorkBll.Update(suggesWork))
                    {
                        return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);

                    }
                }

            }
            return Json(new { Status =1, Message = MessageAction.MessageUpdateSuccess }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Save(string id, int action, int workType)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
            }
            var workDetail = _workDetailBll.GetWorkDetail(id, workType);

            if (workDetail == null)
            {
                return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
            }
          

            if (action == 1)
            {
                workDetail.Status = 4;
                workDetail.ApprovedFisnishBy = UserLogin.UserId;
                workDetail.ApprovedFisnishDate = DateTime.Now;
            }
            if (action == 2)
            {
                workDetail.Status = 5;
                workDetail.ApprovedFisnishBy = UserLogin.UserId;
                workDetail.ApprovedFisnishDate = DateTime.Now;
            }
            if (action != 1 && action != 2)
            {
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.WorkPlanDetail)
            {
                var workPlanDetail = _workPlanDetailBll.GetWorkPlanDetail(workDetail.WorkDetailId);
                workPlanDetail.Status = workDetail.Status;
                workPlanDetail.ApprovedFisnishBy = workDetail.ApprovedFisnishBy;
                workPlanDetail.ApprovedFisnishDate = workDetail.ApprovedFisnishDate;
                if (_workPlanDetailBll.Update(workPlanDetail))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.AssignWork)
            {
                var assignWork = _assignWorkBll.GetAssignWork(workDetail.WorkDetailId);
                assignWork.Status = (byte)workDetail.Status;
                assignWork.ApprovedFisnishBy = workDetail.ApprovedFisnishBy;
                assignWork.ApprovedFisnishDate = workDetail.ApprovedFisnishDate;
                if (_assignWorkBll.Update(assignWork))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.WorkStreamDetail)
            {
                var workStreamDetail = _workStreamDetailBll.GetWorkStreamDetail(workDetail.WorkDetailId);
                workStreamDetail.Status = workDetail.Status;
                workStreamDetail.ApprovedFisnishBy = workDetail.ApprovedFisnishBy;
                workStreamDetail.ApprovedFisnishDate = workDetail.ApprovedFisnishDate;
                if (_workStreamDetailBll.Update(workStreamDetail))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.SuggesWork)
            {
                var suggesWork = _suggesWorkBll.GetSuggesWork(workDetail.WorkDetailId);
                suggesWork.Status = (byte)workDetail.Status;
                suggesWork.ApprovedFisnishBy = workDetail.ApprovedFisnishBy;
                suggesWork.ApprovedFisnishDate = workDetail.ApprovedFisnishDate;
                if (_suggesWorkBll.Update(suggesWork))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmFinish(string id, int workType)
        {
            var workingNotes = new List<WorkingNote>();
            var workDetail = _workDetailBll.GetWorkDetail(id, workType);

            if (workDetail.WorkingNote != null)
            {
                workingNotes = JsonConvert.DeserializeObject<List<WorkingNote>>(workDetail.WorkingNote);
            }
            ViewBag.WorkingNotes = workingNotes;
            return PartialView(workDetail);
        }

        public ActionResult CancelFinish(string id, int workType)
        {
            var workingNotes = new List<WorkingNote>();
            var explanations = new List<Explanation>();

            var workDetail = _workDetailBll.GetWorkDetail(id, workType);
            if (!string.IsNullOrEmpty(workDetail.WorkingNote))
            {
                workingNotes = JsonConvert.DeserializeObject<List<WorkingNote>>(workDetail.WorkingNote);
            }
            if (!string.IsNullOrEmpty(workDetail.Explanation))
            {
                 explanations = JsonConvert.DeserializeObject<List<Explanation>>(workDetail.Explanation);
            }
            ViewBag.WorkingNotes = workingNotes;
            ViewBag.Explanations = explanations;
            return PartialView(workDetail);
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}