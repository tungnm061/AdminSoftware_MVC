    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Helper;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly DepartmentBll _departmentBll;

        public DepartmentController()
        {
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Departments()
        {
            var departments = _departmentBll.GetDepartments(null);
            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Department(string id)
        {
            ViewBag.DepartmentTrees = BuildDropDownTreeModels(string.IsNullOrEmpty(id) ? 0 : int.Parse(id));
            if (string.IsNullOrEmpty(id))
            {
                return
                    PartialView(new Department
                    {
                        IsActive = true
                    });
            }
            return PartialView(_departmentBll.GetDepartment(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult Save(DepartmentModel model)
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
                model.DepartmentCode = EncodeString.ReplaceUnicode(model.DepartmentCode);
                string path;
                if (model.ParentId != null)
                {
                    var departmentParent = _departmentBll.GetDepartment(model.ParentId ?? 0);
                    path = departmentParent.Path + model.DepartmentCode + "/";
                }
                else
                {
                    path = "Root/" + model.DepartmentCode + "/";
                }
                model.Path = path.ToUpper();
                var department = _departmentBll.GetDepartmentByCode(model.DepartmentCode);
                if (model.DepartmentId == 0)
                {
                    if (department != null)
                    {
                        return Json(new {Status = 0, Message = "Mã phòng ban đã tồn tại trong hệ thống!"},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_departmentBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }

                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (department != null && department.DepartmentId != model.DepartmentId)
                {
                    return Json(new {Status = 0, Message = "Mã phòng ban đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_departmentBll.Update(model.ToObject()))
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
                if (_departmentBll.GetDepartments(long.Parse(id)).Any())
                {
                    return
                        Json(
                            new {Status = 0, Message = "Phòng ban đã phát sinh nghiệp vụ liên quan. Bạn không thể xóa!"},
                            JsonRequestBehavior.AllowGet);
                }
                if (_departmentBll.Delete(long.Parse(id)))
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

        public List<KendoDropDownTreeModel> BuildDropDownTreeModels(int departmentId)
        {
            var kendoDropDownTreeModels = new List<KendoDropDownTreeModel>();
            var departments = _departmentBll.GetDepartments(null).Where(x => x.DepartmentId != departmentId).ToList();
            if (departments.Any())
            {
                var itemRoots = departments.Where(x => x.ParentId == null || x.ParentId == 0).ToList();
                kendoDropDownTreeModels.AddRange(itemRoots.Select(x => new KendoDropDownTreeModel
                {
                    value = x.DepartmentId.ToString(),
                    text = x.DepartmentName,
                    expanded = true,
                    ChildModels = GetChilds(x.DepartmentId, departments)
                }));
            }
            return kendoDropDownTreeModels;
        }

        public List<KendoDropDownTreeModel> GetChilds(long departmentId, List<Department> departments)
        {
            var kendoDropDownTreeModels = new List<KendoDropDownTreeModel>();
            var childGroups = departments.Where(x => x.ParentId == departmentId).ToList();
            if (childGroups.Any())
            {
                kendoDropDownTreeModels.AddRange(childGroups.Select(x => new KendoDropDownTreeModel
                {
                    value = x.DepartmentId.ToString(),
                    text = x.DepartmentName,
                    expanded = true,
                    ChildModels = GetChilds(x.DepartmentId, departments)
                }));
            }
            return kendoDropDownTreeModels;
        }
    }
}