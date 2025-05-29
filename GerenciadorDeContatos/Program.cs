var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços MVC e sessão
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // <- Habilita o uso de sessão

var app = builder.Build();

// Configura o pipeline
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // <- Ativa o middleware de sessão

app.UseAuthorization();

// Mapeia rotas padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contatos}/{action=Index}/{id?}");

app.Run();
