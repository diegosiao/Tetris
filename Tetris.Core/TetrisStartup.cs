﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Tetris
{
    public class TetrisStartup
    {
        public TetrisStartup(IConfiguration configuration)
        {
            Configuration = configuration;
            TetrisSettings.LoadConfiguration(configuration);
        }

        public static IConfiguration Configuration { get; private set; }

        public readonly string CorsAllowedOrigins = "_tetris_cors_config";

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(x => 
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => 
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TetrisSettings.TetrisEncryptionSecret)),
                    };
                });

            services.AddControllers()
                .AddJsonOptions((options) =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsAllowedOrigins,
                builder =>
                {
                    builder.WithOrigins(TetrisSettings.CorsAllowedOrigins)
                           .AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsAllowedOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static public IHostBuilder CreateHostBuilder<T>(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup(typeof(T));
                });
        }

        private static void LoadSecuredRoutesSql()
        {
            var scriptFilePath = $"{ AppDomain.CurrentDomain.BaseDirectory}tetris_secured_routes_script.sql";
            var scriptStr = new StringBuilder();
            scriptStr.AppendLine("/**** TETRIS GENERATED SCRIPT ****/");
            try
            {
                var routes = new List<TetrisServiceCodeAttribute>();

                foreach (var type in Assembly.GetEntryAssembly().GetTypes())
                {

                    try
                    {
                        var obj = Activator.CreateInstance(type);

                        if (obj is TetrisApiController)
                        {
                            try
                            {
                                var routeController = (RouteAttribute)obj.GetType().GetCustomAttribute(typeof(RouteAttribute));

                                foreach (var method in type.GetMethods())
                                {
                                    var routeCode = method.GetCustomAttribute(typeof(TetrisServiceCodeAttribute)) as TetrisServiceCodeAttribute;

                                    if (routeCode == null)
                                        continue;

                                    var routeVerb = method.GetCustomAttribute(typeof(HttpMethodAttribute));
                                    var routeMethod = method.GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute;

                                    scriptStr.AppendLine(
                                        $"INSERT INTO `sys_securedroutes` (" +
                                        "    `idapiservico`,  " +
                                        "    `caminhoamigavel`,  " +
                                        "    `api`,  " +
                                        "    `method`,  " +
                                        "    `controller`,  " +
                                        "    `action`,  " +
                                        "    `codigo`,  " +
                                        "    `descricao` " +
                                        ") " +
                                        "VALUES ( " +
                                        $"    '{Guid.NewGuid().ToString()}', " +
                                        $"    '{routeController?.Name ?? " -"} / {routeCode?.Description ?? "- "}', " +
                                        $"    '{type.Assembly.GetCustomAttribute<AssemblyDefaultAliasAttribute>()?.DefaultAlias ?? type.Assembly.GetName().FullName.Split(',')[0]}', " +
                                        $"    '{routeVerb?.GetType()?.Name.Substring(4).Replace("Attribute", "")}', " +
                                        $"    '{routeController?.Name ?? " - "}', " +
                                        $"    '{routeCode?.Description ?? " - "}', " +
                                        $"    '{routeCode?.Code ?? " - " }', " +
                                        $"    '{routeMethod?.Name ?? " - "}' " +
                                        ");");
                                }
                            }
                            catch (Exception ex)
                            {
                                scriptStr.AppendLine($"-- Script generation failed for controller: '{obj.GetType().FullName}");
                                scriptStr.AppendLine($"/* {ex.Message} */");
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(scriptFilePath, $"-- Erro carregando script....\r\n{ex.Message}\r\n{ex.StackTrace}");
            }

            File.WriteAllText(scriptFilePath, scriptStr.ToString());
        }
    }
}
