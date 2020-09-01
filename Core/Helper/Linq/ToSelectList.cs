using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.Helper.Linq
{
    //http://stackoverflow.com/questions/16172932/entity-framework-generate-listselectlistitem-of-unique-values-from-model
    //public static class ToSelectList
    //{
    //    public static IEnumerable<SelectListItem> GetList<TEntity>(this IEnumerable<TEntity> collection, Expression<Func<TEntity, object>> keyExpression,
    //Expression<Func<TEntity, object>> valueExpression, object selectedValue = null)
    //    {
    //        var keyField = keyExpression.PropertyName();
    //        var valueField = valueExpression.PropertyName();
    //        return new SelectList(collection, keyField, valueField, selectedValue).ToList();
    //    }
    //}
}
