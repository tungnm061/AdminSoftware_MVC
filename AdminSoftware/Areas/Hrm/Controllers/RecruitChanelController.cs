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
    public class RecruitChanelController : BaseController
    {
        private readonly RecruitChanelBll _recruitChanelBll;
        private readonly UserBll _userBll;

        public RecruitChanelController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _recruitChanelBll = SingletonIpl.GetInstance<RecruitChanelBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString())).ToList();
            return View();
        }

        public JsonResult RecruitChanels()
        {
            return Json(_recruitChanelBll.GetRecruitChanels(null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecruitChanel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new RecruitChanel {IsActive = true});
            }
            return PartialView(_recruitChanelBll.GetRecruitChanel(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult Save(RecruitChanelModel model)
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
                if (model.RecruitChanelId == 0)
                {
                    model.CreateDate = DateTime.Now;
                    model.CreateBy = UserLogin.UserId;
                    if (_recruitChanelBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_recruitChanelBll.Update(model.ToObject()))
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
                if (_recruitChanelBll.Delete(int.Parse(id)))
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