﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStoreAppCore.Services
{
    public class UserServices : IUserServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public bool isAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
