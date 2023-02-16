using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class ResultViewModel<T> where T : class
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public List<T> list { get; set; }
        public T response { get; set; }
        public bool IsSucess { get; set; } = false;
        public bool IsError { get; set; } = false;


    }
}
