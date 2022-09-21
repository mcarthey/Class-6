using Class_6.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_6
{
    public class ExampleClass
    {
        public void ExampleMethod()
        {
            var fileService = new FileService();

            var fileService1 = new FileService(1241234);
            var fileService2 = new FileService("hello");
        }
    }
}
