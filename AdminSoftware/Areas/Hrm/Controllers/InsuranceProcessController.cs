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
    public class InsuranceProcessController : BaseController
    {
        private readonly InsuranceBll _insuranceBll;
        private readonly InsuranceProcessBll _insuranceProcessBll;
        private readonly UserBll _userBll;

        public InsuranceProcessController()
        {
            _insuranceBll = SingletonIpl.GetInstance<InsuranceBll>();
            _insuranceProcessBll = SingletonIpl.GetInstance<InsuranceProcessBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString()));
            ViewBag.Insurances = _insuranceBll.GetInsurances(null);
            return View();
        }

        public JsonResult InsuranceProcesses()
        {
            ViewBag.Insurances = _insuranceBll.GetInsurances(null);
            return Json(_insuranceProcessBll.GetInsuranceProcesses(0), JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsuranceProcess(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new InsuranceProcess
                {
                    InsuranceProcessId = Guid.Empty.ToString(),
                    Amount = 0,
                    FromDate = DateTime.Now
                });
            return PartialView(_insuranceProcessBll.GetInsuranceProcess(id));
        }

        [HttpPost]
        public JsonResult Save(InsuranceProcessModel model)
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
                model.CreateBy = UserLogin.UserId;
                model.CreateDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.InsuranceProcessId) || model.InsuranceProcessId == Guid.Empty.ToString())
                {
                    model.InsuranceProcessId = Guid.NewGuid().ToString();
                    if (_insuranceProcessBll.Insert(model.ToObject()))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_insuranceProcessBll.Update(model.ToObject()))
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
                if (_insuranceProcessBll.Delete(id))
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