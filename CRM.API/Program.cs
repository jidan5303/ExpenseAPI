using CRM.API.Auth;
using CRM.Common.QueryHelper;
using CRM.DataAccess;
using CRM.Services;
using CRM.Services.Interface;
using CRM.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/*--------Cors policy-------------------------------*/
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                //.AllowCredentials()
                //.WithOrigins("http://localhost:4200/");
                .AllowAnyOrigin();
        });
});

/*--------Swagger authentication checking-----------*/
//builder.Services.AddSwaggerGen(c =>
//{
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
//                      Enter 'Bearer' [space] and then your token in the text input below.
//                                  \r\n\r\nExample: 'Bearer 12345abcdef'",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                },
//                Scheme = "oauth2",
//                Name = "Bearer",
//                In = ParameterLocation.Header,

//            },
//            new List<string>()
//        }
//    });
//    c.CustomSchemaIds(i => i.FullName);
//});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "Jwt",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Authentication with JWT token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {

        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        },
        new List<string>()
        }
    });
});

/*-------------Database connection--------*/
builder.Services.AddDbContext<CRMDbContext>
    (
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("CRMCON"))
    );
builder.Services.AddDbContext<DatavancedDbContext>
    (
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Datavanced"))
    );
//builder.Services.AddIdentity<Employee, IdentityRole>(options =>
//    {
//        options.User.RequireUniqueEmail = false;
//    })
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<DatavancedDbContext>()
//    .AddDefaultTokenProviders();


/*-------------HttpContext access permission----*/
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

/*-------------Service register area-------START-----*/
builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
builder.Services.AddScoped<IActionsService, ActionsService>();
builder.Services.AddScoped<ICaregiverHistoryService, CaregiverHistoryService>();
builder.Services.AddScoped<ICaregiverService, CaregiverService>();
builder.Services.AddScoped<IDefaultLayoutDetailService, DefaultLayoutDetailService>();
builder.Services.AddScoped<IDefaultLayoutService, DefaultLayoutService>();
builder.Services.AddScoped<IDefaultTableColumnService, DefaultTableColumnService>();
builder.Services.AddScoped<IDefaultTableService, DefaultTableService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentStatusService, DepartmentStatusService>();
builder.Services.AddScoped<IDepartmentTaskHistoryService, DepartmentTaskHistoryService>();
builder.Services.AddScoped<IDepartmentTaskService, DepartmentTaskService>();
builder.Services.AddScoped<IKnowledgeBaseHistoryService, KnowledgeBaseHistoryService>();
builder.Services.AddScoped<IKnowledgeBaseService, KnowledgeBaseService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISystemUserService, SystemUserService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<IDepartmentTypeService, DepartmentTypeService>();
builder.Services.AddScoped<IPatientNotesService, PatientNoteService>();

builder.Services.AddScoped<ICaregiverNoteService, CaregiverNoteService>();
builder.Services.AddScoped<ICaregiverAttachmentService, CaregiverAttachmentService>();

builder.Services.AddScoped<IPatientAttachmentService, PatientAttachmentService>();
builder.Services.AddScoped<IScheduleTaskService, ScheduleTaskService>();

builder.Services.AddScoped<IOrganizationService, OrganizationService>();

builder.Services.AddScoped<ICustomFilterService, CustomFilterService>();
builder.Services.AddScoped<BuildDynamicFilter>();

// expense 
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IExpenseTypeService, ExpenseTypeService>();
builder.Services.AddScoped<IExpenseAttachmentService, ExpenseAttachmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExpensePermissionService, ExpensePermissionService>();

/*-------------Service register area-------END-----*/

//Configure the HTTP request pipeline.


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Versioned API v1.0");
});
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//else if (app.Environment.IsProduction())
//{
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Versioned API v1.0");
//    });
//}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");


// app.UseAuthorization();


app.UseMiddleware<JwtAuthenticationMiddleware>();

app.UseMiddleware<JwtAuthorizationMiddleware>();


app.MapControllers();
//app.UseCRMAuthentication();
//app.UseCRMAuthorization();

app.Run();