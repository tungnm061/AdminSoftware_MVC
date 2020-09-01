using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class PositionBll
    {
        private readonly PositionDal _positionDal;

        public PositionBll()
        {
            _positionDal = SingletonIpl.GetInstance<PositionDal>();
        }

        public List<Position> GetPositions()
        {
            return _positionDal.GetPositions();
        }

        public Position GetPosition(int positionId)
        {
            return _positionDal.GetPosition(positionId);
        }

        public Position GetPosition(string positionCode)
        {
            return _positionDal.GetPosition(positionCode);
        }

        public int Insert(Position position)
        {
            return _positionDal.Insert(position);
        }

        public bool Update(Position position)
        {
            return _positionDal.Update(position);
        }

        public bool Delete(int positionId)
        {
            return _positionDal.Delete(positionId);
        }
    }
}