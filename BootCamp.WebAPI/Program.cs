using BootCamp.WebAPI.Dal;
using BootCamp.WebAPI.Dal.Repositories;
using BootCamp.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Transformar palavra chave em bytes:
var key = Encoding.ASCII.GetBytes(Settings.Secret);
// Para instanciar os servi�os
builder.Services.AddAuthentication(x =>
{
    // Adicionamos a autentica��o e dentro, as configura��es necess�rias (deve autenticar com o esquema
    // padr�o).
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Chave para compila��o:
.AddJwtBearer(x =>
{
    // Nenhum metadado poder� passar:
    x.RequireHttpsMetadata = false;
    // Salvar o token:
    x.SaveToken = true;
    // Par�metros padr�es requisitados:
    x.TokenValidationParameters = new TokenValidationParameters
    {
        // Assinatura:
        ValidateIssuerSigningKey = true,
        // Vai utilizar a key normal sempre que validar o c�digo:
        IssuerSigningKey = new SymmetricSecurityKey(key),
        // Valida se a assinatura est� correta:
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder => {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
        });    
});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", 
        new OpenApiInfo{
            Title = "ACME",
            Version = "v1",
            Description = "API REST"
        });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer "+
                      " scheme. \r\n\r\n Enter 'Bearer'[space] and then your token" +
                      "in the text input below. \r\n\r\nExample: \"Bearer 1234abcdef\"",
    });
    c.AddSecurityRequirement( new OpenApiSecurityRequirement
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
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddTransient<IFornecedor, FornecedorDAL>();
builder.Services.AddTransient<IContrato, ContratoDAL>();
builder.Services.AddTransient<IProduto, ProdutoDAL>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("EnableCORS");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();


