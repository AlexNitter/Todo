using AlexNitter.Todo.Lib.DataLayer.Repositories;
using AlexNitter.Todo.Lib.DataModels;
using AlexNitter.Todo.Lib.Entities;
using System;

namespace AlexNitter.Todo.Lib.Services
{
    public class AuthenticationService
    {
        private KryptoService _kryptoService = new KryptoService();
        private SessionRepository _sessionRepo = new SessionRepository();
        private UserRepository _userRepo = new UserRepository();

        public BaseResponse Register(RegisterRequest request)
        {
            var response = new BaseResponse() { Success = false };

            if (!passwordsMatch(request.Passwort, request.PasswortWiederholung))
            {
                response.Message = "Die eingegebenen Passwörter stimmen nicht überin";
            }
            else
            {
                var temp = _userRepo.FindByUsername(request.Username);

                if (temp != null)
                {
                    response.Message = "Der gewünschte Username ist bereits vergeben";
                }
                else
                {
                    var salt = _kryptoService.GenerateSalt(Config.SALT_BIT_SIZE);
                    var passwortHash = _kryptoService.Hash(request.Passwort, salt);

                    var user = new User()
                    {
                        Username = request.Username,
                        PasswortSalt = salt,
                        PasswortHash = passwortHash
                    };

                    _userRepo.Insert(user);

                    response.Message = "Der User wurde erfolgreich angelegt";
                    response.Success = true;
                }
            }

            return response;
        }

        public LoginResponse Login(LoginRequest request)
        {
            var response = new LoginResponse()
            {
                Success = false,
                Message = "Unbekannter Benutzername oder falsches Passwort"
            };

            var user = _userRepo.FindByUsername(request.Username);

            if (user != null)
            {
                var tempHash = _kryptoService.Hash(request.Passwort, user.PasswortSalt);

                if (passwordsMatch(tempHash, user.PasswortHash))
                {
                    var session = new Session()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Aktiv = true,
                        UserId = user.Id,
                        Erstellungsdatum = DateTime.Now
                    };


                    _sessionRepo.Insert(session);

                    response.Success = true;
                    response.SessionCreated = session;
                    response.Message = "";
                }
            }

            return response;
        }

        public Session ValidateSession(String sessionId)
        {
            var session = _sessionRepo.FindById(sessionId);

            if (session != null && session.Aktiv)
            {
                return session;
            }
            else
            {
                return null;
            }
        }

        public void Logout(String sessionId)
        {
            var session = _sessionRepo.FindById(sessionId);

            if (session != null)
            {
                session.Aktiv = false;
                _sessionRepo.Update(session);
            }
        }

        private Boolean passwordsMatch(String passwort1, String passwort2)
        {
            return passwort1 == passwort2;
        }
    }
}
