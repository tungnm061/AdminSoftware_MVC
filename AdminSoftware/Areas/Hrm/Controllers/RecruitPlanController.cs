using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class RecruitPlanController : BaseController
    {
        private readonly ApplicantBll _applicantBll;
        private readonly AutoNumberBll _autoNumberBll;
        private readonly DepartmentBll _departmentBll;
        private readonly PositionBll _positionBll;
        private readonly string _prefix = "TD" + DateTime.Now.Year.ToString().Substring(2, 2);
        private readonly RecruitChanelBll _recruitChanelBll;
        private readonly RecruitPlanBll _recruitPlanBll;
        private readonly UserBll _userBll;

        public RecruitPlanController()
        {
            _recruitChanelBll = SingletonIpl.GetInstance<RecruitChanelBll>();
            _recruitPlanBll = SingletonIpl.GetInstance<RecruitPlanBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _positionBll = SingletonIpl.GetInstance<PositionBll>();
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _applicantBll = SingletonIpl.GetInstance<ApplicantBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString())).ToList();
            ViewBag.RecruitChanels =
                _recruitChanelBll.GetRecruitChanels(null)
                    .Select(x => new KendoForeignKeyModel(x.ChanelName, x.RecruitChanelId.ToString()))
                    .ToList();
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()))
                    .ToList();
            ViewBag.Positions =
                _positionBll.GetPositions()
                    .Select(x => new KendoForeignKeyModel(x.PositionName, x.PositionId.ToString()))
                    .ToList();
            return View();
        }

        public JsonResult RecruitPlans(DateTime fromDate, DateTime toDate)
        {
            return Json(_recruitPlanBll.GetRecruitPlans(null, fromDate, toDate), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecruitPlan(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new RecruitPlan
                {
                    IsActive = true,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(15),
                    RecruitPlanId = 0,
                    RecruitPlanCode = _autoNumberBll.GetAutoNumber(_prefix),
                    Quantity = 1,
                    DepartmentId = 0,
                    PositionId = 0
                });
            }
            return PartialView(_recruitPlanBll.GetRecruitPlan(long.Parse(id)));
        }

        [HttpPost]
        public JsonResult Save(RecruitPlanModel model)
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
                model.CreateDate = DateTime.Now;
                model.CreateBy = UserLogin.UserId;
                if (model.RecruitPlanId == 0)
                {
                    model.RecruitPlanCode = _prefix;
                    var recruitPlanCode = "";
                    if (_recruitPlanBll.Insert(model.ToObject(), ref recruitPlanCode) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_recruitPlanBll.Update(model.ToObject()))
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
                if (_applicantBll.GetApplicants(long.Parse(id)).Any())
                {
                    return Json(new {Status = 0, Message = "Đã phát sinh nghiệp vụ liên quan. Bạn không thể xóa!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (_recruitPlanBll.Delete(int.Parse(id)))
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