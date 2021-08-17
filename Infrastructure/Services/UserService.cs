using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;

namespace Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserLoginResponseModel> Login(UserLoginRequestModel requestModel)
        {
            var user = await _userRepository.GetUserByEmail(requestModel.Email);
            if (user==null)
            {
                return null;
            }

            var hashedPassword = GetHashedPassword(requestModel.Password, user.Salt);
            if (hashedPassword != user.HashedPassword) return null;
            var response = new UserLoginResponseModel
            {
                Id = user.Id, Email = user.Email, FirstName = user.Email, LastName = user.LastName, DateOfBirth = user.DateOfBirth
            };
            return response;
        }

        public async Task<UserRegisterResponseModel> Register(UserRegisterRequestModel requestModel)
        {
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);
            if (dbUser !=null)
            {
                throw new ConflictException("Email already exists");
            }

            var salt = CreateSalt();
            var hashedPassword = GetHashedPassword(requestModel.Password, salt);

            var user = new User
            {
                Email = requestModel.Email, FirstName = requestModel.FirstName, LastName = requestModel.LastName, Salt = salt, HashedPassword = hashedPassword
            };
            var createdUser = await _userRepository.AddAsync(user);

            var response = new UserRegisterResponseModel
            {
                Id = createdUser.Id, Email = createdUser.Email, FirstName = createdUser.FirstName, LastName = createdUser.LastName
            };
            return response;
        }

        public async Task<IEnumerable<MovieCardResponseModel>> GetPurchasedMovies(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovieCardResponseModel>> GetFavorites(int userId)
        {
            throw new NotImplementedException();
        }

        public string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
           
            return Convert.ToBase64String(randomBytes);
        }
        
        public string GetHashedPassword(string password, string salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }

    
}