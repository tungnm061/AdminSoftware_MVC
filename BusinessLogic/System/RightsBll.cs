using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Singleton;
using DataAccess.System;
using Entity.System;

namespace BusinessLogic.System
{
    public class RightsBll
    {
        private readonly RightDal _rightDal;

        public RightsBll()
        {
            _rightDal = SingletonIpl.GetInstance<RightDal>();
        }

        public Rights GeRightsFunction(int userId, string area, string controller)
        {
            return _rightDal.GeRightsFunction(userId, area, controller);
        }
    }
}
