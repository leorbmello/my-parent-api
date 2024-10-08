﻿using MyParentApi.Application.DTOs.Requests;
using MyParentApi.Application.DTOs.Requests.Auth;
using MyParentApi.Application.DTOs.Requests.Profile;
using MyParentApi.Application.DTOs.Requests.Users;
using MyParentApi.Application.DTOs.Responses;

namespace MyParentApi.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthUserAsync(AuthRequest request);
        Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
        Task<GenericResponse> RecoveryPasswordAsync(PasswordRecoveryRequest request);
        void Logout(AuthRefreshRequest request);
    }
}
