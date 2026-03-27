
using BackEnd.Repositorios.SDR.DAL;
using BackEnd.Servicos.SDR.Interfaces;
using BackEnd.Servicos.SDR.Services;
using BackEnd.Servicos.SDR.Validacoes;
using Microsoft.AspNetCore.HttpOverrides;
using Supabase;

namespace BackEnd.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /* VOu deixar todo o meu código original comentado caso de alguma bosta, INICIO
             * 
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton(provider =>
            {
                var url = builder.Configuration["Supabase:Url"];
                var key = builder.Configuration["Supabase:ApiKey"];

                var options = new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = false
                };
                var client = new Client(url, key, options);

                return client;
            });
            */

            /*
            builder.Services.AddHttpClient<MotoMatsuoSupabaseClient>(client =>
            {
                var baseUrl = builder.Configuration["MotoMatsuoSupabase:BaseUrl"];
                if (!string.IsNullOrEmpty(baseUrl))
                    client.BaseAddress = new Uri(baseUrl);

                var apiKey = builder.Configuration["MotoMatsuoSupabase:ApiKey"];
                if (!string.IsNullOrEmpty(apiKey))
                    client.DefaultRequestHeaders.Add("apikey", apiKey);

                var authorization = builder.Configuration["MotoMatsuoSupabase:Authorization"];
                if (!string.IsNullOrEmpty(authorization))
                    client.DefaultRequestHeaders.Add("Authorization", authorization);
            });
            */

            // MotoMatsuoSupabase client configurado via appsettings / env
            /*
            builder.Services.AddHttpClient<MotoMatsuoSupabaseClient>(client =>
            {
                var baseUrl = builder.Configuration["MotoMatsuoSupabase:BaseUrl"];
                var apiKey = builder.Configuration["MotoMatsuoSupabase:ApiKey"];
                var authorization = builder.Configuration["MotoMatsuoSupabase:Authorization"];

                if (!string.IsNullOrWhiteSpace(baseUrl))
                    client.BaseAddress = new Uri(baseUrl);

                if (!string.IsNullOrWhiteSpace(apiKey))
                    client.DefaultRequestHeaders.Add("apikey", apiKey);

                if (!string.IsNullOrWhiteSpace(authorization))
                    client.DefaultRequestHeaders.Add("Authorization", authorization);
            });


            builder.Services.AddScoped<LeadDAL>();
            builder.Services.AddScoped<LeadService>();
            builder.Services.AddScoped<ContactDAL>();
            builder.Services.AddScoped<ContactService>();
            builder.Services.AddScoped<AddressDAL>();
            builder.Services.AddScoped<AddressService>();
            builder.Services.AddScoped<NoteDAL>();
            builder.Services.AddScoped<NoteService>();
            builder.Services.AddScoped<ILeadValidator, LeadValidator>();
            builder.Services.AddScoped<IAddressValidator, AddressValidator>();
            builder.Services.AddScoped<LoginPortalService>();
            builder.Services.AddScoped<SdrService>();




            // Preciso disso para liberar acesso a api no front
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000", "http://localhost:5173") // Vite
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }); // Tenho que mover para o appsettings

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowFrontend"); // ✅ TEM que vir antes do MapControllers

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            FINAL, se der merda eu volto a versão original*/


            // VERSÂO SUGERIDA PELO CHAT
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            // Supabase client (já lendo da configuração)
            builder.Services.AddSingleton(provider =>
            {
                var url = builder.Configuration["Supabase:Url"];
                var key = builder.Configuration["Supabase:ApiKey"];

                var options = new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = false
                };
                var client = new Client(url, key, options);
                return client;
            });

            // MotoMatsuo typed HttpClient configurado via appsettings/user-secrets/env
            builder.Services.AddHttpClient<MotoMatsuoSupabaseClient>(client =>
            {
                var baseUrl = builder.Configuration["MotoMatsuoSupabase:BaseUrl"];
                var apiKey = builder.Configuration["MotoMatsuoSupabase:ApiKey"];
                var authorization = builder.Configuration["MotoMatsuoSupabase:Authorization"];

                if (!string.IsNullOrWhiteSpace(baseUrl))
                    client.BaseAddress = new Uri(baseUrl);

                if (!string.IsNullOrWhiteSpace(apiKey))
                    client.DefaultRequestHeaders.Add("apikey", apiKey);

                if (!string.IsNullOrWhiteSpace(authorization))
                    client.DefaultRequestHeaders.Add("Authorization", authorization);
            });

            builder.Services.AddHttpClient<ReceitaAWS>(client =>
            {
                client.BaseAddress = new Uri("https://www.receitaws.com.br/");
            });

            // Registros de serviços/repositórios (mantive os seus)
            builder.Services.AddScoped<LeadDAL>();
            builder.Services.AddScoped<LeadService>();
            builder.Services.AddScoped<ContactDAL>();
            builder.Services.AddScoped<ContactService>();
            builder.Services.AddScoped<AddressDAL>();
            builder.Services.AddScoped<AddressService>();
            builder.Services.AddScoped<NoteDAL>();
            builder.Services.AddScoped<NoteService>();
            builder.Services.AddScoped<ILeadValidator, LeadValidator>();
            builder.Services.AddScoped<IAddressValidator, AddressValidator>();
            builder.Services.AddScoped<LoginPortalService>();
            builder.Services.AddScoped<SdrService>();

            // CORS configurado via appsettings
            var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new string[0];
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.Configure<ForwardedHeadersOptions>(options => // Esse código transforma sua API de “modo debug” para “modo produção seguro”.
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var app = builder.Build();

            // em Program.cs, após app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error"); // endpoint genérico
                app.UseHsts();
            }

            app.UseForwardedHeaders();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // mostra stack traces amigáveis em dev
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("FrontendPolicy");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
