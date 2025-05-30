global using Forja.Application;
global using Forja.Infrastructure;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.OpenApi.Models;
global using Forja.Application.DTOs.UserProfile;
global using Forja.Application.Interfaces.Authentication;
global using Forja.Infrastructure.Keycloak;
global using Forja.Application.Interfaces.UserProfile;
global using Forja.Application.Interfaces.Games;
global using Forja.Domain.Enums;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authorization;
global using Forja.Application.Requests.Authentication;
global using Forja.Application.Requests.UserProfile;
global using Forja.Application.Validators;
global using Forja.Application.Requests.Games;
global using Forja.Application.Interfaces.Storage;
global using Forja.Application.Requests.Storage;
global using System.Security.Claims;
global using Forja.Application.Interfaces.Store;
global using Forja.Application.Interfaces.Support;
global using Forja.Application.Requests.Store;
global using Forja.Application.Requests.Support;
global using Forja.Application.Interfaces.Common;
global using Forja.Application.Requests.Common;
global using Forja.Application.Interfaces.Analytics;
global using System.Globalization;
global using Forja.Application.Logging;
global using Forja.Application.DTOs.Store;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Caching.Distributed;
global using System.Text.Json;
global using Forja.Application.DTOs.Games;
global using Forja.Application.DTOs.Common;