using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using GestorTask.Application.Implementation;
using GestorTask.Application.Interfaces;
using GestorTask.Application.Services;
using GestorTask.Helpers;
using GestorTask.Infraestructure;
using GestorTask.Utilitys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;

namespace GestorTask.setup;

public static class ServicesSetuppublic
{
    public static IServiceCollection RegisterServices(this IServiceCollection Services,
            TokenValidationParameters tokenValidationParameters,
            string MyAllowSpecificOrigins,
            string connectionString)
    {

        Services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins, builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .WithMethods("Get,Post,Put,Delete");
            });
        });

        Services.AddAntiforgery(options =>
        {
            //Set options to avoid click jacking attack
            options.SuppressXFrameOptionsHeader = true;
        });

        Services.Configure<GzipCompressionProviderOptions>(opt =>
        {
            opt.Level = CompressionLevel.Fastest;

        });

        Services.AddOptions();

        #region versioning
        Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                            new HeaderApiVersionReader("x-api-version"),
                            new MediaTypeApiVersionReader("x-api-version"));
        }).AddApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        #endregion
        Services.AddEndpointsApiExplorer();
        Services.AddSwaggerGen();

        Services.ConfigureOptions<ConfigureSwaggerOptions>();

        Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
            options.SaveToken = true;
        });

        #region Add Culture
        //Representation of the default user locale of the system. This controls default number and date formatting and the like.
        var cultureInfo = new CultureInfo("es-DO");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        //CurrentUICulture  = UI localization/translation part of your app.
        var cultureInfoUi = new CultureInfo("es-DO");
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfoUi;
        #endregion

        Services.AddDbContext<ModelContext>(x =>
            x.UseOracle(
                connectionString,
                options => options.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion23)
            ));
            
        #region services
        Services.AddScoped(typeof(IDataRepository<>), typeof(Repository<>));
        Services.AddSingleton<ICryptProvider, CryptProvider>();

        Services.AddScoped(typeof(IAuthenticationServices), typeof(AuthenticationServices));
        Services.AddScoped(typeof(IUserServices), typeof(UserServices));
        Services.AddScoped(typeof(ITaskServices), typeof(TaskServices));

        #endregion

        #region fluent validation
        Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        Services.AddControllers(options =>
        options.Filters.Add(typeof(FilterValidationException)));

        Services.AddFluentValidationAutoValidation();
        Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        #endregion

        return Services;
    }
}