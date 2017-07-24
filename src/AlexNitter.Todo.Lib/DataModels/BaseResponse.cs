using System;
using System.Collections.Generic;
using System.Text;

namespace AlexNitter.Todo.Lib.DataModels
{
    public class BaseResponse
    {
        public Boolean Success { get; set; }
        public String Message { get; set; }
    }
}
