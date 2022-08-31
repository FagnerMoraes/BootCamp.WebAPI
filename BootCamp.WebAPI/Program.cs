using BootCamp.WebAPI.Dal;
using BootCamp.WebAPI.Dal.Repositories;
using BootCamp.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Transformar palavra chave em bytes:
var key = Encoding.ASCII.GetBytes(Settings.Secret);
// Para instanciar os serviços
builder.Services.AddAuthentication(x =>
{
    // Adicionamos a autenticação e dentro, as configurações necessárias (deve autenticar com o esquema
    // padrão).
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Chave para compilação:
.AddJwtBearer(x =>
{
    // Nenhum metadado poderá passar:
    x.RequireHttpsMetadata = false;
    // Salvar o token:
    x.SaveToken = true;
    // Parâmetros padrões requisitados:
    x.TokenValidationParameters = new TokenValidationParameters
    {
        // Assinatura:
        ValidateIssuerSigningKey = true,
        // Vai utilizar a key normal sempre que validar o código:
        IssuerSigningKey = new SymmetricSecurityKey(key),
        // Valida se a assinatura está correta:
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

builder.Services.AddSwaggerGen();
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


