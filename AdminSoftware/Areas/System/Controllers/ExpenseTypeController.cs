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
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using DuyAmazone.Areas.Printify.Models;
using Entity.System;

namespace AdminSoftware.Areas.System.Controllers
{
    public class ExpenseTypeController : BaseController
    {

        private readonly ExpenseTypeBll _expenseTypeBll;
        private readonly UserBll _userBll;

        public ExpenseTypeController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _expenseTypeBll = SingletonIpl.GetInstance<ExpenseTypeBll>();
        }

        // GET: Printify/ExpenseType
        [SystemFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            ViewBag.Users = users.Select(x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName });

            return View();
        }

        public ActionResult ExpenseTypes()
        {
            var ExpenseTypes = _expenseTypeBll.GetExpenseTypes(true);
            return Json(ExpenseTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExpenseType(int id)
        {
            if (id == 0)
            {
                return PartialView(new ExpenseType
                {
                    ExpenseId = 0,
                    IsActive = true
                });
            }
            return PartialView(_expenseTypeBll.GetExpenseType(id));
        }

        [HttpPost]
        public JsonResult Save(ExpenseType model)
        {
            try
            {
                if (model == null)
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
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

                if (model.ExpenseId == 0)
                {
                    model.IsActive = true;
                    if (_expenseTypeBll.Insert(model) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    model.IsActive = true;
                    if (_expenseTypeBll.Update(model))
                        return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                            JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = -1, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_expenseTypeBll.Delete(int.Parse(id)))
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


        public JsonResult ClearSession()
        {
            return null;
        }


    }
}