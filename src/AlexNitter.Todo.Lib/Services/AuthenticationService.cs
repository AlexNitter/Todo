using AlexNitter.Todo.Lib.DataLayer.Repositories;
using AlexNitter.Todo.Lib.DataModels;
using AlexNitter.Todo.Lib.Entities;
using System;

namespace AlexNitter.Todo.Lib.Services
{
    public class AuthenticationService
    {
        public BaseResponse Register(LoginRequest request)
        {
            var response = new BaseResponse() { Success = false };

            if (!passwordsMatch(request.Passwort, request.PasswortWiederholung))
            {
                response.Message = "Die eingegebenen Passwörter stimmen nicht überin";
            }
            else
            {
                var repo = new UserRepository();
                var temp = repo.FindByUsername(request.Username);

                if (temp != null)
                {
                    response.Message = "Der gewünschte Username ist bereits vergeben";
                }
                else
                {
                    var user = new User() { Username = request.Username };

                    repo.Insert(user);

                    response.Message = "Der User wurde erfolgreich angelegt";
                    response.Success = true;
                }
            }

            return response;
        }

        public LoginResponse Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        private Boolean passwordsMatch(String passwort, String passwortWiederholung)
        {
            return passwort == passwortWiederholung;
        }
    }
}
