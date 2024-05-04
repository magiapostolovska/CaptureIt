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


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options => //novo
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddDbContext<CaptureItContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
//builder.Services.AddScoped<IEventParticipantRepository, EventParticipantRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IPasswordRecoveryRepository, PasswordRecoveryRepository>();
//builder.Services.AddScoped<IUserBadgeRepository, UserBadgeRepository>();
builder.Services.AddScoped<IPictureRepository, PictureRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IEventParticipantService, EventParticipantService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IPasswordRecoveryService, PasswordRecoveryService>();
//builder.Services.AddScoped<IUserBadgeService, UserBadgeService>();
builder.Services.AddScoped<ICommentService, CommentService>();


builder.Services.AddAutoMapper(typeof(AlbumMapper));
builder.Services.AddAutoMapper(typeof(PictureMapper));
builder.Services.AddAutoMapper(typeof(BadgeMapper));
builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.AddAutoMapper(typeof(CommentMapper));
builder.Services.AddAutoMapper(typeof(LikeMapper));
builder.Services.AddAutoMapper(typeof(EventParticipantMapper));
builder.Services.AddAutoMapper(typeof(PasswordRecoveryMapper));
builder.Services.AddAutoMapper(typeof(UserBadgeMapper));
builder.Services.AddAutoMapper(typeof(EventMapper));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

