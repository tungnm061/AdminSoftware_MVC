using System;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class ReligionController : BaseController
    {
        private readonly EmployeeBll _employeeBll;
        private readonly ReligionBll _religionBll;

        public ReligionController()
        {
            _religionBll = SingletonIpl.GetInstance<ReligionBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Religions()
        {
            var religions = _religionBll.GetReligions();
            return Json(religions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Religion(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return
                    PartialView(new Religion());
            }
            return PartialView(_religionBll.GetReligion(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult Save(ReligionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new {Status = 0, Message = MessageAction.ModelStateNotValid},
                        JsonRequestBehavior.AllowGet);
                if (model.ReligionId == 0)
                {
                    if (_religionBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }

                if (!_religionBll.Update(model.ToObject()))
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
                if (_religionBll.Delete(int.Parse(id)))
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