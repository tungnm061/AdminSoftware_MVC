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
    public class CompanyBankController : BaseController
    {

        private readonly CompanyBankBll _companyBankBll;
        private readonly UserBll _userBll;

        public CompanyBankController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _companyBankBll = SingletonIpl.GetInstance<CompanyBankBll>();
        }

        // GET: Printify/CompanyBank
        [SystemFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            ViewBag.Users = users.Select(x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName });

            return View();
        }

        public ActionResult CompanyBanks()
        {
            var CompanyBanks = _companyBankBll.GetCompanyBanks();
            return Json(CompanyBanks, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompanyBank(int id)
        {
            if (id == 0)
            {
                return PartialView(new CompanyBank
                {
                    CompanyBankId = 0,
                    IsActive = true
                });
            }
            return PartialView(_companyBankBll.GetCompanyBank(id));
        }

        [HttpPost]
        public JsonResult Save(CompanyBank model)
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

                if (model.CompanyBankId == 0)
                {
                    model.CreateBy = UserLogin.UserId;
                    model.CreateDate = DateTime.Now;
                    model.IsActive = true;
                    if (_companyBankBll.Insert(model) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    model.UpdateBy = UserLogin.UserId;
                    model.UpdateDate = DateTime.Now;
                    model.IsActive = true;
                    if (_companyBankBll.Update(model))
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
                if (_companyBankBll.Delete(int.Parse(id)))
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