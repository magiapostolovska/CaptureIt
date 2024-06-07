using CaptureIt.Authentication;
using CaptureIt.AutoMapper;
using CaptureIt.Data;
using CaptureIt.Repos;
using CaptureIt.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using CaptureIt.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CaptureIt.Middleware;
using Microsoft.OpenApi.Models;
using System.Net.Mail;
using System.Net;
using CaptureIt;
using Microsoft.Azure.CognitiveServices.Vision.Face;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

builder.Services.AddLogging();
builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = null;
    });

builder.Services.AddDbContext<CaptureItContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IPasswordRecoveryRepository, PasswordRecoveryRepository>();
builder.Services.AddScoped<IPictureRepository, PictureRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IPasswordRecoveryService, PasswordRecoveryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFaceService, FaceService>();


builder.Services.AddAutoMapper(typeof(AlbumMapper));
builder.Services.AddAutoMapper(typeof(PictureMapper));
builder.Services.AddAutoMapper(typeof(BadgeMapper));
builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.AddAutoMapper(typeof(CommentMapper));
builder.Services.AddAutoMapper(typeof(LikeMapper));
builder.Services.AddAutoMapper(typeof(PasswordRecoveryMapper));
builder.Services.AddAutoMapper(typeof(EventMapper));

builder.Services.AddSingleton<SmtpClient>(provider =>
{
    var smtpClient = new SmtpClient("smtp.gmail.com")
    {
        Port = 587,
        Credentials = new NetworkCredential("capture.it.cit@gmail.com", "zobo bgxf vdth tihv"),
        EnableSsl = true
    };
    return smtpClient;
});

builder.Services.AddSingleton<IFaceClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var subscriptionKey = configuration["AzureFaceApi:SubscriptionKey"];
    var endpoint = configuration["AzureFaceApi:Endpoint"];
    return new FaceClient(new ApiKeyServiceClientCredentials(subscriptionKey)) { Endpoint = endpoint };
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseMiddleware<TokenValidation>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

