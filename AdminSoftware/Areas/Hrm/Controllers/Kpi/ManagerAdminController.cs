using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class ManagerAdminController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly WorkPlanBll _workPlanBll;

        public ManagerAdminController()
        {
            _workPlanBll = SingletonIpl.GetInstance<WorkPlanBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            return View();
        }

        public JsonResult WorkPlans(DateTime? fromDate, DateTime? toDate, int action)
        {
            if (action == 3)
            {
                fromDate = null;
                toDate = null;
            }
            return
                Json(_workPlanBll.GetWorkPlansByDepartmentId(action, null, fromDate, toDate),
                    JsonRequestBehavior.AllowGet);
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
            if (action == 3)
            {
                workPlan.ApprovedDate = DateTime.Now;
                workPlan.ApprovedBy = UserLogin.UserId;
                if (_workPlanBll.Update(workPlan))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
            }
            workPlan.ApprovedDate = null;
            workPlan.ApprovedBy = null;
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