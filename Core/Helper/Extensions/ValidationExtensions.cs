using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Core.Helper.Extensions
{
    public static class ValidationExtensions
    {
        public static MvcHtmlString ValidationMessageLabelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string errorClass = "error")
        {
            string elementName = ExpressionHelper.GetExpressionText(expression);
            MvcHtmlString normal = html.ValidationMessageFor(expression);
            if (normal != null)
            {
                string newValidator = Regex.Replace(normal.ToHtmlString(), @"<span([^>]*)>([^<]*)</span>", string.Format("<label for=\"{0}\" $1>$2</label>", elementName), RegexOptions.IgnoreCase);
                if (!string.IsNullOrWhiteSpace(errorClass))
                    newValidator = newValidator.Replace("field-validation-error", errorClass);
                return MvcHtmlString.Create(newValidator);
            }
            return null;
        }
    }
}
