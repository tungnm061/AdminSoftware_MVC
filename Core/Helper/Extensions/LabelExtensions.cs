using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Core.Helper.Extensions
{
    public static class LabelExtensions
    {
        public static MvcHtmlString Label(this HtmlHelper html, string expression, string id = "", bool generatedId = false)
        {
            return LabelHelper(html, ModelMetadata.FromStringExpression(expression, html.ViewData), expression, id, generatedId);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string id = "", bool generatedId = false)
        {
            return LabelHelper(html, ModelMetadata.FromLambdaExpression(expression, html.ViewData), ExpressionHelper.GetExpressionText(expression), id, generatedId);
        }

        internal static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string id, bool generatedId)
        {
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            var sb = new StringBuilder();
            sb.Append(labelText);
            if (metadata.IsRequired)
                sb.Append("*");

            var tag = new TagBuilder("label");
            if (!string.IsNullOrWhiteSpace(id))
            {
                tag.Attributes.Add("id", id);
            }
            else if (generatedId)
            {
                tag.Attributes.Add("id", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName) + "_Label");
            }

            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(sb.ToString());

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString LabelForRequire<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, bool required = true, bool usingFor = true)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
            {
                if (property.Name == "textlang")
                {
                    var value = property.GetValue(htmlAttributes);
                    if (value != null)
                        labelText = value.ToString();
                }
            }

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            var sb = new StringBuilder();
            sb.Append(labelText);
            if (required && metadata.IsRequired)
                sb.Append("*");
            sb.Append(":");
            var tag = new TagBuilder("label");
            tag.MergeAttributes(attributes);
            if (usingFor)
                tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(sb.ToString());
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString LabelLang<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return LabelForRequire(html, expression, htmlAttributes, false);
        }

        public static MvcHtmlString LabelLang<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, bool isRequired, bool usingFor)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
            {
                if (property.Name == "textlang")
                {
                    var value = property.GetValue(htmlAttributes);
                    if (value != null)
                        labelText = value.ToString();
                }
            }

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            var sb = new StringBuilder();
            sb.Append(labelText);
            if (isRequired || metadata.IsRequired)
                sb.Append("*");
            sb.Append(":");
            var tag = new TagBuilder("label");
            tag.MergeAttributes(attributes);
            if (usingFor)
                tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(sb.ToString());
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}
