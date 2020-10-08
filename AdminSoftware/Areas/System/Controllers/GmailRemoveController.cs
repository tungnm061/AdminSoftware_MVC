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
    public class GmailRemoveController : BaseController
    {

        private readonly GmailRemoveBll _gmailRemoveBll;
        private readonly UserBll _userBll;
        private readonly GmailBll _gmailBll;

        public GmailRemoveController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _gmailRemoveBll = SingletonIpl.GetInstance<GmailRemoveBll>();
            _gmailBll = SingletonIpl.GetInstance<GmailBll>();
        }

        // GET: Printify/GmailRemove
        [SystemFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            var gmails = _gmailBll.GetGmails(null);

            ViewBag.Users = users.Select(x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName }); ViewBag.Users = users.Select(x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName });
            ViewBag.Gmails = gmails.Select(x => new KendoForeignKeyModel { value = x.Id.ToString(), text = x.FullName });

            return View();
        }

        public ActionResult GmailRemoves()
        {
            var GmailRemoves = _gmailRemoveBll.GetGmailRemoves(true);
            return Json(GmailRemoves, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GmailRemove(int id)
        {
            if (id == 0)
            {
                return PartialView(new GmailRemove
                {
                    GmailRemoveId = 0,
                    IsActive = true
                });
            }
            return PartialView(_gmailRemoveBll.GetGmailRemove(id));
        }

        [HttpPost]
        public JsonResult Save(GmailRemove model)
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

                if (model.GmailRemoveId == 0)
                {
                    model.IsActive = true;
                    model.CreateBy = UserLogin.UserId;
                    model.CreateDate = DateTime.Now;
                    var insert = _gmailRemoveBll.Insert(model);
                    if (insert > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    else if (insert == -1)
                    {
                        return Json(new { Status = 0, Message = "Tài khoản Gmail gỡ Amazon này đã tồn tại vui lòng cập nhật bản ghi cũ!" },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    model.IsActive = true;
                    model.UpdateBy = UserLogin.UserId;
                    model.UpdateDate = DateTime.Now;
                    int update = _gmailRemoveBll.Update(model);
                    if (update > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    else if (update == -1)
                    {
                        return Json(new { Status = 0, Message = "Tài khoản Gmail gỡ Amazon thay đổi này đã tồn tại không thể cập nhật!" },
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
                if (_gmailRemoveBll.Delete(int.Parse(id)))
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