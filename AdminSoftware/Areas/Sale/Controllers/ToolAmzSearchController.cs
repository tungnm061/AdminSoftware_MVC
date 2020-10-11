using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminSoftware.Areas.Sale.Controllers
{
    public class ToolAmzSearchController : Controller
    {
        // GET: Sale/ToolAmzSearch
        public ActionResult Index()
        {
            return View();
        }

        public class ElementCustom
        {
            //public IWebElement WebElement { get; set; }
            public string UrlImage { get; set; }
            public string Title { get; set; }

        }


        public JsonResult GetDataSearch(string keySearch, int? page)
        {
            var url = "https://www.amazon.com/s?k={0}&ref=nb_sb_noss";
            url = string.Format(url, keySearch);
            IWebDriver driver;
            driver = new ChromeDriver();
            var options = new ChromeOptions();
            options.AddExtension(@"D:\Test\jkompbllimaoekaogchhkmkdogpkhojg-3.0.2-Crx4Chrome.com.crx");
            var driver2 = new ChromeDriver(options);
            // test
            driver2.Navigate().GoToUrl(url);
            var a = driver2.FindElements(By.ClassName("s-include-content-margin"));
            var listElm = new List<ElementCustom>();
            foreach (var item in a)
            {
                ElementCustom elmCustom = new ElementCustom();
                var elementRoot = item.FindElements(By.ClassName("xtaqv-root"));
                if (elementRoot.Count > 0)
                {
                    //elmCustom.WebElement = elementRoot[0];
                    //var x = item.FindElement(By.ClassName("s-image"));
                    elmCustom.Title = item.FindElement(By.XPath("//h2")).Text;
                    elmCustom.UrlImage = item.FindElement(By.ClassName("s-image")).GetAttribute("src");
                    listElm.Add(elmCustom);
                }
            }
            return Json(listElm, JsonRequestBehavior.AllowGet);
        }
    }
}