using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Kpi;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.System.Models.Kpi;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.System;

namespace AdminSoftware.Areas.System.Controllers.Kpi
{
    public class CategoryKpiController : BaseController
    {
        private readonly CategoryKpiBll _categoryKpiBll;
        private readonly TaskBll _taskBll;
        private readonly UserBll _userBll;
        public CategoryKpiController()
        {
            _categoryKpiBll = SingletonIpl.GetInstance<CategoryKpiBll>();
            _taskBll = SingletonIpl.GetInstance<TaskBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.FullName, x.UserId.ToString()));
            return View();
        }

        public JsonResult CategoryKpis()
        {
            var tasks = _taskBll.GetTasks(null, null, null, null);
            var categoryKpis = _categoryKpiBll.GetCategoryKpis();
            //foreach (var item in tasks)
            //{
            //    foreach (var item2 in categoryKpis)
            //    {
            //        if (item.DepartmentName.Trim() == item2.KpiName.Trim())
            //        {
            //            item.CategoryKpiId = item2.CategoryKpiId;
            //            _taskBll.Update(item);
            //        }
            //    }
            //}
            return Json(_categoryKpiBll.GetCategoryKpis(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CategoryKpi(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new CategoryKpi
                {
                    CategoryKpiId = 0
                }
               );
            }
            return PartialView(_categoryKpiBll.GetCategoryKpi(Int32.Parse(id)));
        }

        public JsonResult Save(CategoryKpiModel model)
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
                model.CreateDate = DateTime.Now;
                model.CreateBy = UserLogin.UserId;
                if (model.CategoryKpiId==0)
                {
                    
                    return Json(_categoryKpiBll.Insert(model.ToObject())
                        ? new { Status = 1, Message = MessageAction.MessageCreateSuccess }
                        : new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(_categoryKpiBll.Update(model.ToObject())
                    ? new { Status = 1, Message = MessageAction.MessageUpdateSuccess }
                    : new { Status = 0, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message,ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_categoryKpiBll.Delete(Int32.Parse(id)))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageDeleteSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}