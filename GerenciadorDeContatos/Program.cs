var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os MVC e sess�o
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // <- Habilita o uso de sess�o

var app = builder.Build();

// Configura o pipeline
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // <- Ativa o middleware de sess�o

app.UseAuthorization();

// Mapeia rotas padr�o
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contatos}/{action=Index}/{id?}");

app.Run();
