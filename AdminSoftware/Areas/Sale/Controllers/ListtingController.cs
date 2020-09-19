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
            return PartialView(_listtingBll.GetListting(long.Parse(id)));
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

                if (model.ListtingId <= 0)
                {
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
            var stringCategoryKpi = Request.Form["CategoryKpiId"];

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

                        var checkName = _gmailBll.GetGmailByName(gmailName);

                        if (checkName == null)
                        {
                            return Json(new { Status = 0, Message = "Tài khoản mail này chưa được tạo trong hệ thống!" },
                                JsonRequestBehavior.AllowGet);
                        }

                        var obj = new Listting
                        {
                            GmailId = checkName.Id,
                            ThreeNumberPayOnner = int.Parse(data["Ba số Payonner"]),
                            PayOnner =  int.Parse(data["Payonner"]),
                            ListProduct = int.Parse(data["Listting"]),
                            IsActive = true,
                            Description = data["Ghi chú"],
                            CreateBy = UserLogin.UserId,
                            CreateDate = DateTime.Now,
                            Balance = int.Parse(data["Balance"])
                        };

                        if (!TryValidateModel(obj))
                            return Json(new { Status = 0, Message = "Dữ liệu không hợp lệ!" },
                                JsonRequestBehavior.AllowGet);


                        listObj.Add(obj);
                        //SaveLog((byte)ActionTypeEnum.Create, "Import hệ thống tà khoản", "ChartOfAccount");
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