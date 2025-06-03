using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Features.SellerBlanks.Apply;
using RaritetBooks.Application.Features.SellerBlanks.Approve;
using RaritetBooks.Application.Features.SellerBlanks.Decline;
using RaritetBooks.Application.Features.Sellers.DeletePhoto;
using RaritetBooks.Application.Features.Sellers.DeleteProduct;
using RaritetBooks.Application.Features.Sellers.PublishProduct;
using RaritetBooks.Application.Features.Sellers.UpdateProduct;
using RaritetBooks.Application.Features.Sellers.UploadPhoto;
using RaritetBooks.Application.Features.Users.Activate;
using RaritetBooks.Application.Features.Users.Login;
using RaritetBooks.Application.Features.Users.Logout;
using RaritetBooks.Application.Features.Users.RefreshToken;
using RaritetBooks.Application.Features.Users.Register;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHandlers();
        services.AddValidatorsFromAssembly(typeof(DependencyRegistration).Assembly);

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<
            ICommandHandler<PublishProductRequest, Guid, Error>,
            PublishProductHandler>();
        services.AddScoped<
            ICommandHandler<UpdateProductRequest, Guid, Error>,
            UpdateProductHandler>();
        services.AddScoped<
            ICommandHandler<Guid, Guid, Error>,
            DeleteProductHandler>();
        services.AddScoped<
            ICommandHandler<UploadSellerPhotoRequest, string, Error>,
            UploadSellerPhotoHandler>();
        services.AddScoped<
            ICommandHandler<DeleteSellerPhotoRequest, bool, Error>, 
            DeleteSellerPhotoHandler>();
        services.AddScoped<
            ICommandHandler<ApplySellerBlankRequest, Guid, Error>,
            ApplySellerRequestHandler>();
        services.AddScoped<
            ICommandHandler<ApproveSellerBlankRequest, bool, Error>,
            ApproveSellerBlanktHandler>();
        services.AddScoped<
            ICommandHandler<DeclineSellerBlankRequest, bool, Error>,
            DeclineSellerBlankHandler>();
        services.AddScoped<
            ICommandHandler<RegisterRequest, bool, Error>,
            RegisterHandler>();
        services.AddScoped<
            ICommandHandler<LoginRequest, LoginResponse, Error>,
            LoginHandler>();
        services.AddScoped<
            ICommandHandler<RefreshTokenResponse, Error>,
            RefreshTokenHandler>();
        services.AddScoped<
            ICommandHandler<Guid, bool, Error>,
            ActivateHandler>();
        services.AddScoped<
            ICommandHandler<bool, Error>,
            LogoutHandler>();

        return services;
    }
}