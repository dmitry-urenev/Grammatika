using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingvoLive
{
    public class LingvoApiService : WebHttpService
    {
        public LingvoApiService()
            : base("http://lingvolive.com/api/Translation/")
        {
        }
    }
}
