using AlexNitter.Todo.Lib.Entities;
using System;

namespace AlexNitter.Todo.Lib.DataModels
{
    public class LoginResponse : BaseResponse
    {
        public Session SessionCreated { get; set; }
    }
}
