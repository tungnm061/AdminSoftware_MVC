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
    public class GmailRegController : BaseController
    {

        private readonly GmailRegBll _gmailRegBll;
        private readonly UserBll _userBll;
        private readonly GmailBll _gmailBll;

        public GmailRegController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _gmailRegBll = SingletonIpl.GetInstance<GmailRegBll>();
            _gmailBll = SingletonIpl.GetInstance<GmailBll>();
        }

        // GET: Printify/GmailReg
        [SystemFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            var gmails = _gmailBll.GetGmails(null);

            ViewBag.Users = users.Select(x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName });            ViewBag.Users = users.Select(x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName });
            ViewBag.Gmails = gmails.Select(x => new KendoForeignKeyModel { value = x.Id.ToString(), text = x.FullName });

            return View();
        }

        public ActionResult GmailRegs()
        {
            var GmailRegs = _gmailRegBll.GetGmailRegs(true);
            return Json(GmailRegs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GmailReg(int id)
        {
            if (id == 0)
            {
                return PartialView(new GmailReg
                {
                    GmailRegId = 0,
                    IsActive = true
                });
            }

            var obj = _gmailRegBll.GetGmailReg(id);
            return PartialView(obj);
        }

        [HttpPost]
        public JsonResult Save(GmailReg model)
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

                if (model.GmailRegId == 0)
                {
                    model.IsActive = true;
                    model.CreateBy = UserLogin.UserId;
                    model.CreateDate = DateTime.Now;
                    var insert = _gmailRegBll.Insert(model);
                    if (insert > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    else if (insert == -1)
                    {
                        return Json(new { Status = 0, Message = "Tài khoản Gmail Reg này đã tồn tại vui lòng cập nhật bản ghi cũ!" },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    model.IsActive = true;
                    model.UpdateBy = UserLogin.UserId;
                    model.UpdateDate = DateTime.Now;
                    int update = _gmailRegBll.Update(model);
                    if (update > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    else if (update == -1)
                    {
                        return Json(new { Status = 0, Message =  "Tài khoản Gmail Reg thay đổi này đã tồn tại không thể cập nhật!" },
                            JsonRequestBehavior.AllowGet);
                    }

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
                if (_gmailRegBll.Delete(int.Parse(id)))
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