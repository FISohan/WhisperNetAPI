using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WhisperNetAPI.Data;
using WhisperNetAPI.Models;

namespace WhisperNetAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(DataContext context,IConfiguration configuration,IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }



        public async Task<ResponseMessage<string?>> Login(UserDto user)
        {
            ResponseMessage<string?> responseMessage = new ResponseMessage<string>();
            User ?_user = await _context.Users.FirstOrDefaultAsync(item => item.Username == user.Username);
            if( _user == null ) {
                responseMessage.Response = "User not found";
                responseMessage.Success = false;
                return responseMessage;
            }
            if(!VerifyPassword(user.Password,_user.PasswordHash,_user.PasswordSalt))
            {
                responseMessage.Response = "Incorrect Password";
                responseMessage.Success = false;
                return responseMessage;
            }
            responseMessage.Response = "Successfully login";
            responseMessage.Success = true;
            responseMessage.Data = CreateToken(_user);
            return responseMessage;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Appsetting:token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: cred
                    );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public async Task<ResponseMessage<string>> Register(UserDto user)
        {
            ResponseMessage<string> response = new ResponseMessage<string>();
            User? _user = await _context.Users.FirstOrDefaultAsync(item => item.Username == user.Username);
            if (_user != null) { 
                response.Success = false;
                response.Response = "User Name Already Exist";
                return response;
            }
            CreatePasswordHash(user.Password, out byte[] passwordHash,out byte[] passwordSalt);
            User newUser = new User();
            newUser.Username = user.Username;
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            response.Success = true;
            response.Response = "Successfully create user";

            _context.Add(newUser);
            await _context.SaveChangesAsync();
            return response;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash,out byte[] passwordSalt) { 
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<ResponseMessage<UserView>> GetUser(string username)
        {
           ResponseMessage<UserView> response = new ResponseMessage<UserView>();
           User? user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(user == null) { response.Success = false;response.Response = "User not found";return response;}
           UserView views = _mapper.Map<User?, UserView>(user);
            response.Success = true;
            response.Response = "yes";
            response.Data = views;
            return response;
        }
    }
}
