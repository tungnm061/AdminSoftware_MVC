using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Kpi;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class ManagerDepartmentController : BaseController
    {
        private readonly WorkPlanBll _workPlanBll;
        private readonly DepartmentBll _departmentBll;
        public ManagerDepartmentController()
        {
            _workPlanBll = SingletonIpl.GetInstance<WorkPlanBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult WorkPlans(DateTime? fromDate, DateTime? toDate, int action)
        {
            try
            {
                if (action == 1)
                {
                    fromDate = null;
                    toDate = null;
                }
                var department = _departmentBll.GetDepartment(UserLogin.DepartmentId ?? 0);
                if (department == null || string.IsNullOrEmpty(department.Path))
                {
                    return
                    Json(_workPlanBll.GetWorkPlansByDepartmentId( action, "",fromDate, toDate),
                        JsonRequestBehavior.AllowGet);
                }
                var path = department.Path;
                String[] elements = Regex.Split(path, "/");
                var newPath = elements[0] + "/" + elements[1] + "/" + elements[2] + "/";
                return
                    Json(_workPlanBll.GetWorkPlansByDepartmentId(action, newPath, fromDate, toDate),
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return
                    Json(new List<WorkPlan>(),
                        JsonRequestBehavior.AllowGet);
            }          
        }

        public ActionResult WorkPlan(string id)
        {
            var workPlan = _workPlanBll.GetWorkPlan(id);
            ViewBag.WorkPlanDetails = workPlan.WorkPlanDetails;
            return PartialView(workPlan);
        }

        public JsonResult Save(string id, int action)
        {
            if (id == null)
            {
                return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
            }
            var workPlan = _workPlanBll.GetWorkPlan(id);
            if (workPlan == null)
            {
                return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
            }
            if (action == 1)
            {
                workPlan.ConfirmedDate = DateTime.Now;
                workPlan.ConfirmedBy = UserLogin.UserId;
                if (_workPlanBll.Update(workPlan))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
            }
            workPlan.ConfirmedDate = null;
            workPlan.ConfirmedBy = null;
            if (_workPlanBll.Update(workPlan))
            {
                return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess}, JsonRequestBehavior.AllowGet);
            }
            return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}