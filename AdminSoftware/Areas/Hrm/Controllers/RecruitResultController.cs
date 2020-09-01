using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class RecruitResultController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly PositionBll _positionBll;
        private readonly RecruitPlanBll _recruitPlanBll;
        private readonly RecruitResultBll _recruitResultBll;
        private readonly UserBll _userBll;

        public RecruitResultController()
        {
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _positionBll = SingletonIpl.GetInstance<PositionBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _recruitResultBll = SingletonIpl.GetInstance<RecruitResultBll>();
            _recruitPlanBll = SingletonIpl.GetInstance<RecruitPlanBll>();
        }

        private List<RecruitResultDetail> RecruitResultDetailsInMemory
        {
            get
            {
                if (Session["RecruitResultDetailsInMemory"] == null)
                    return new List<RecruitResultDetail>();
                return (List<RecruitResultDetail>) Session["RecruitResultDetailsInMemory"];
            }
            set { Session["RecruitResultDetailsInMemory"] = value; }
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString())).ToList();
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()))
                    .ToList();
            ViewBag.Positions =
                _positionBll.GetPositions()
                    .Select(x => new KendoForeignKeyModel(x.PositionName, x.PositionId.ToString()))
                    .ToList();
            ViewBag.RecruitResults = from RecruitResultEnum s in Enum.GetValues(typeof (RecruitResultEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            ViewBag.Employees =
                _employeeBll.GetEmployees(null)
                    .Select(x => new KendoForeignKeyModel(x.EmployeeCode + " - " + x.FullName, x.EmployeeId.ToString()))
                    .ToList();
            ViewBag.RecruitPlans =
                _recruitPlanBll.GetRecruitPlans(true, DateTime.Now.AddYears(-10), DateTime.Now)
                    .Select(x => new KendoForeignKeyModel(x.RecruitPlanCode, x.RecruitPlanId.ToString()))
                    .ToList();
            ViewBag.Sexs = from SexEnum s in Enum.GetValues(typeof (SexEnum))
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

        public JsonResult RecruitResults(long? recruitPlanId)
        {
            return Json(_recruitResultBll.GetRecruitResults(recruitPlanId ?? 0), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecruitResult(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new RecruitResult
                {
                    RecruitResultDetails = new List<RecruitResultDetail>(),
                    RecruitResultId = Guid.Empty.ToString(),
                    Result = (byte) RecruitResultEnum.Waiting
                });
            }
            var recruitResult = _recruitResultBll.GetRecruitResult(id);
            RecruitResultDetailsInMemory = recruitResult.RecruitResultDetails;
            return PartialView(recruitResult);
        }

        public JsonResult RecruitResultDetails()
        {
            return Json(RecruitResultDetailsInMemory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecruitResultDetail(string id)
        {
            var recruitResultDetail = RecruitResultDetailsInMemory.Find(x => x.RecruitResultDetailId == id);
            return
                PartialView(recruitResultDetail ??
                            new RecruitResultDetail
                            {
                                RecruitResultDetailId = Guid.Empty.ToString(),
                                Result = (byte) RecruitResultEnum.Waiting,
                                InterviewDate = DateTime.Now
                            });
        }

        public JsonResult SaveRecruitResultDetail(RecruitResultDetail model)
        {
            try
            {
                var recruitResultDetailsInMemory = RecruitResultDetailsInMemory;
                var recruitResultDetail =
                    recruitResultDetailsInMemory.Find(x => x.RecruitResultDetailId == model.RecruitResultDetailId);
                if (recruitResultDetail == null)
                {
                    model.RecruitResultDetailId = Guid.NewGuid().ToString();
                    recruitResultDetailsInMemory.Add(model);
                }
                else
                {
                    recruitResultDetail.Result = model.Result;
                    recruitResultDetail.InterviewDate = model.InterviewDate;
                    recruitResultDetail.EmployeeId = model.EmployeeId;
                    recruitResultDetail.Description = model.Description;
                }
                RecruitResultDetailsInMemory = recruitResultDetailsInMemory;
                return Json(new {Status = 1, Message = "Cập nhật kết quả thành công!"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteRecruitResultDetail(string id)
        {
            try
            {
                var recruitResultDetailsInMemory = RecruitResultDetailsInMemory;
                recruitResultDetailsInMemory.Remove(recruitResultDetailsInMemory.Find(x => x.RecruitResultDetailId == id));
                RecruitResultDetailsInMemory = recruitResultDetailsInMemory;
                return Json(new {Status = 1, Message = "Cập nhật kết quả thành công!"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Save(RecruitResultModel model)
        {
            try
            {
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
                if (!RecruitResultDetailsInMemory.Any())
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = "Chưa có đánh giá ứng viên nào!"
                    }, JsonRequestBehavior.AllowGet);
                }
                var recruitResult = model.ToObject();
                recruitResult.CreateBy = UserLogin.UserId;
                recruitResult.CreateDate = DateTime.Now;
                recruitResult.RecruitResultDetails = RecruitResultDetailsInMemory;
                if (string.IsNullOrEmpty(recruitResult.RecruitResultId) ||
                    recruitResult.RecruitResultId == Guid.Empty.ToString())
                {
                    model.RecruitResultId = Guid.NewGuid().ToString();
                    if (_recruitResultBll.Insert(recruitResult))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_recruitResultBll.Update(recruitResult))
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
                if (_recruitResultBll.Delete(id))
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
            RecruitResultDetailsInMemory = null;
            return null;
        }
    }
}