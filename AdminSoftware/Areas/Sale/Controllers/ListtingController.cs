using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Sale;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Sale.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Helper;
using AdminSoftware.Models;
using Entity.Sale;
using Core.Helper.Extensions;

namespace AdminSoftware.Areas.Sale.Controllers
{
    public class ListtingController : BaseController
    {
        private readonly AutoNumberBll _autoNumberBll;
        private readonly ListtingBll _listtingBll;
        private readonly UserBll _userBll;
        private readonly GmailBll _gmailBll;

        public ListtingController()
        {
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _listtingBll = SingletonIpl.GetInstance<ListtingBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _gmailBll = SingletonIpl.GetInstance<GmailBll>();
        }

        [SaleFilter]
        public ActionResult Index()
        {
            var gmails = _gmailBll.GetGmails();
            ViewBag.Gmails =
                gmails.Select(x => new KendoForeignKeyModel { value = x.Id.ToString(), text = x.FullName });
            ViewBag.Users =
                _userBll.GetUsers(null)
                    .Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString()));
            return View();
        }

        public JsonResult Listtings()
        {
            return Json(_listtingBll.GetListtings(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Listting(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Listting
                {
                    ListtingId = 0,
                    IsActive = true
                });
            }
            var obj = _listtingBll.GetListting(long.Parse(id));
            return PartialView(obj);
        }

        public JsonResult Save(Listting model)
        {
            try
            {
                model.IsActive = true;
                if (model == null)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
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

                if (model.ThreeNumberPayOnner != model.PayOnner.GetLast(3))
                {
                    return Json(new { Status = 0, Message = "Số PayOnner và 3 số cuối không trùng khớp!" }, JsonRequestBehavior.AllowGet);
                }
                var check = _listtingBll.GetListtingByGmailId(model.GmailId);
                if (model.ListtingId <= 0)
                {
                    if (check != null)
                    {
                        return Json(new { Status = 0, Message = "Tài khoản gmail này đã tồn tại bản ghi vui lòng cập nhật bản ghi cũ!" }, JsonRequestBehavior.AllowGet);

                    }
                    model.CreateDate = DateTime.Now;
                    model.CreateBy = UserLogin.UserId;
                    if (_listtingBll.Insert(model) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                model.UpdateDate = DateTime.Now;
                model.UpdateBy = UserLogin.UserId;

                if (_listtingBll.Update(model))
                {
                    if (check != null && check.ListtingId != model.ListtingId)
                    {
                        return Json(new { Status = 0, Message = "Tài khoản gmail cập nhật này đã tồn tại bản ghi vui lòng cập nhật bản ghi cũ!" }, JsonRequestBehavior.AllowGet);

                    }
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = 0,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
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
                if (_listtingBll.Delete(long.Parse(id)))
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

        #region Import Excel
        public ActionResult ViewExcel()
        {
            return PartialView();
        }

        public JsonResult ImportExcel()
        {
            if (Request.Files["files"] == null)
                return Json(new { Status = 0, Message = "Không có dữ liệu để import!" }, JsonRequestBehavior.AllowGet);
            try
            {
                var file = Request.Files["files"];
                var dataLst = ExcelHelper.ReadExcelDictionary(file.InputStream);
                var listObj = new List<Listting>();
                if (dataLst.Count == 0)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);

                }
                foreach (var data in dataLst)
                {
                    if (
                        data.ContainsKey("Mail") &&
                        data.ContainsKey("Ba số Payonner") &&
                        data.ContainsKey("Payonner") &&
                        data.ContainsKey("Listting") &&
                        data.ContainsKey("Balance") &&
                        data.ContainsKey("Ghi chú"))
                    {
                        string gmailName = data["Mail"];
                        string threeNumberPayOnner = data["Ba số Payonner"];
                        string payOnner = data["Payonner"];
                        string balance = data["Balance"];
                        string listProduct = data["Listting"];

                        if (string.IsNullOrEmpty(gmailName))
                        {
                            return Json(new { Status = 0, Message = "Tài khoản mail không được để trống" },
                                JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(threeNumberPayOnner))
                        {
                            return Json(new { Status = 0, Message = "Ba số Payonner không được để trống" },
                                JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(payOnner))
                        {
                            return Json(new { Status = 0, Message = "Số Payonner không được để trống" },
                                JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(balance))
                        {
                            return Json(new { Status = 0, Message = "Balance không được để trống" },
                                JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(listProduct))
                        {
                            return Json(new { Status = 0, Message = "Listting không được để trống" },
                                JsonRequestBehavior.AllowGet);
                        }
                        gmailName = gmailName.Trim();
                        var checkName = _gmailBll.GetGmailByName(gmailName);

                        if (checkName == null)
                        {
                            return Json(new { Status = 0, Message = gmailName +" Tài khoản mail này chưa được tạo trong hệ thống!" },
                                JsonRequestBehavior.AllowGet);
                        }

                        var obj = new Listting
                        {
                            GmailId = checkName.Id,
                            ThreeNumberPayOnner = threeNumberPayOnner.Trim(),
                            PayOnner = payOnner.Trim(),
                            ListProduct = int.Parse(listProduct.Trim()),
                            IsActive = true,
                            Description = data["Ghi chú"],
                            CreateBy = UserLogin.UserId,
                            CreateDate = DateTime.Now,
                            Balance = int.Parse(balance.Trim())
                        };

                        if (!TryValidateModel(obj))
                            return Json(new { Status = 0, Message = "Dữ liệu không hợp lệ!" },
                                JsonRequestBehavior.AllowGet);
                        if (obj.ThreeNumberPayOnner != obj.PayOnner.GetLast(3))
                        {
                            return Json(new { Status = 0, Message = gmailName +" Số PayOnner và 3 số cuối không trùng khớp!" }, JsonRequestBehavior.AllowGet);
                        }

                        listObj.Add(obj);
                    }
                    else
                    {
                        return Json(new { Status = 0, Message = "File import không đúng định dạng!" },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                if (_listtingBll.Saves(listObj))
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

        #endregion


        public JsonResult ClearSession()
        {
            return null;
        }
    }
}