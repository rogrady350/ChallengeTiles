using ChallengeTiles.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeTiles.Server.Controllers
{
    public class AuthController : ControllerBase
    {
        //controller handles login and registration logic
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
