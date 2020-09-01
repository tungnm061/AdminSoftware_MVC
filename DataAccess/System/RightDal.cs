using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.System;

namespace DataAccess.System
{
    public class RightDal : BaseDal<ADOProvider>
    {
        public Rights GeRightsFunction(int userId , string area,string controller)
        {
            try
            {
                return UnitOfWork.Procedure<Rights>("[dbo].[Get_Rights_FunctionName]",new
                {
                    UserId = userId,
                    Area = area,
                    Controller = controller
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new Rights();
            }
        }
    }
}
