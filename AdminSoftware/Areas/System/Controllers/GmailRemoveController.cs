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
using AdminSoftware.Helper;

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


        public ActionResult ViewExcel()
        {
            return PartialView();
        }

        public JsonResult ImportExcel()
        {
            if (Request.Files["files"] == null)
                return Json(new { Status = 0, Message = "Không có dữ liệu để import!" }, JsonRequestBehavior.AllowGet);
            var stringCategoryKpi = Request.Form["CategoryKpiId"];

            try
            {
                var file = Request.Files["files"];
                var dataLst = ExcelHelper.ReadExcelDictionary(file.InputStream);
                var listObj = new List<GmailRemove>();
                if (dataLst.Count == 0)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);

                }
                var gmails = _gmailBll.GetGmails(true);
                var tempDict = gmails.ToDictionary(x => x.FullName.Trim(), x => x.Id);

                foreach (var data in dataLst)
                {
                    if (
                        data.ContainsKey("GMAIL GỠ TK AMZ") &&
                        data.ContainsKey("MẬT KHẨU") &&
                        data.ContainsKey("MAIL KHÔI PHỤC") &&
                        data.ContainsKey("THAY GMAIL") &&
                        data.ContainsKey("MẬT KHẨU MAIL THAY") &&
                        data.ContainsKey("MAIL KHÔI PHỤC THAY"))
                    {
                        string gmailRemove = data["GMAIL GỠ TK AMZ"];
                        string gmailRestore = data["MAIL KHÔI PHỤC"];
                        string passWord = data["MẬT KHẨU"];
                        string gmailChange = data["THAY GMAIL"];
                        string gmailChangeRestore = data["MAIL KHÔI PHỤC THAY"];
                        string passWordChange = data["MẬT KHẨU MAIL THAY"];
                        if (string.IsNullOrEmpty(gmailRemove) || string.IsNullOrEmpty(gmailRestore))
                        {
                            return Json(new { Status = 0, Message = "Tài khoản gmail không được để trống !" },
                                JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(passWordChange))
                        {
                            return Json(new { Status = 0, Message = "Mật khẩu không được để trống !" },
                                JsonRequestBehavior.AllowGet);
                        }
                        if (!tempDict.ContainsKey(gmailRemove.Trim()))
                        {
                            return Json(new { Status = 0, Message = "Tài khoản " + gmailRemove + " chưa được khởi tạo trong hệ thống!" },
                                JsonRequestBehavior.AllowGet);
                        }

                        if (!tempDict.ContainsKey(gmailRestore.Trim()))
                        {
                            return Json(new { Status = 0, Message = "Tài khoản " + gmailRestore + " chưa được khởi tạo trong hệ thống!" },
                                JsonRequestBehavior.AllowGet);
                        }

                        if (!tempDict.ContainsKey(gmailChange.Trim()))
                        {
                            return Json(new { Status = 0, Message = "Tài khoản " + gmailChange + " chưa được khởi tạo trong hệ thống!" },
                                JsonRequestBehavior.AllowGet);
                        }

                        if (!tempDict.ContainsKey(gmailChangeRestore.Trim()))
                        {
                            return Json(new { Status = 0, Message = "Tài khoản " + gmailChangeRestore + " chưa được khởi tạo trong hệ thống!" },
                                JsonRequestBehavior.AllowGet);
                        }

                        listObj.Add(new GmailRemove
                        {
                            GmailId = tempDict[gmailRemove.Trim()],
                            GmailRestoreId = tempDict[gmailRestore.Trim()],
                            Password = passWord.Trim(),
                            GmailChangeId = tempDict[gmailChange.Trim()],
                            GmailRestoreChangeId = tempDict[gmailChangeRestore.Trim()],
                            PasswordGmailChange = passWordChange.Trim(),
                            CreateDate = DateTime.Now,
                            CreateBy = UserLogin.UserId,
                            UpdateDate = DateTime.Now,
                            UpdateBy = UserLogin.UserId,
                            IsActive = true
                        });

                    }
                    else
                    {
                        return Json(new { Status = 0, Message = "File import không đúng định dạng!" },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                if (_gmailRemoveBll.Saves(listObj))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageImportSuccess },
                        JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ClearSession()
        {
            return null;
        }


    }
}