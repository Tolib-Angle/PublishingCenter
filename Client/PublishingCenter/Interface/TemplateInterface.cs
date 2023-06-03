using System.Collections.Generic;
using PublishingCenter.ServerComm;
using System.Linq;

namespace PublishingCenter.PublishingCenter.Interface
{
    public class TemplateInterface<T> where T : Entity
    {
        private int FirtsId = 0;
        private int LasdId;
        private int PackageSize;
        public string Condition { get; set; }

        public TemplateInterface(int packageSize)
        {
            PackageSize = packageSize;
            LasdId = 0;
            FirtsId = 0;
            Condition = "";
        }

        public IList<T> NextPackage()
        {
            Query query = new Query { Table = typeof(T).Name, StartId = FirtsId, Count = PackageSize };
            if (Condition != null)
            {
                query.Condition = Condition;
            }

            var result = Repository.Instance.FindByCondition<T>(query);
            if (result.Count != 0)
            {
                FirtsId = result[result.Count - 1].id;
                LasdId = result[0].id;
            }
            return result;
        }

        public IList<T> PrevPackage()
        {
            Query query = new Query { Table = typeof(T).Name, EndId = LasdId, Count = PackageSize };
            if (Condition != null)
            {
                query.Condition = Condition;
            }

            var result = Repository.Instance.FindByCondition<T>(query);
            if (result.Count != 0)
            {
                LasdId = result[result.Count - 1].id;
                FirtsId = result[0].id;
            }
            IList<T> result2 = Enumerable.Reverse(result).ToList();
            return result2;
        }
        public T GetDataByNumber(int number)
        {
            var result = Repository.Instance.GetByNumber<T>(number);
            return result;
        }
        public void Reset()
        {
            FirtsId = 0;
            LasdId = 0;
        }

        public void SetNewId(int first_id, int last_id)
        {
            FirtsId = first_id;
            LasdId = last_id;
        }
    }
}
