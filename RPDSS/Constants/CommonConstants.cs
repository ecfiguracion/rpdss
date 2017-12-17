using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPDSS.Constants
{
    public class ParameterTypes: List<IdValue>
    {
        private static List<IdValue> items = new List<IdValue>();

        static ParameterTypes()
        {
            items.Add(new IdValue { Id = 1, Name = "Rainfall" });
            items.Add(new IdValue { Id = 2, Name = "Temperature" });
        }

        public static List<IdValue> GetList()
        {
            return items;
        }

        public static IdValue GetById(int id)
        {
            return items.SingleOrDefault(x => x.Id == id);
        }
    }
}
