using System.Security.Cryptography.X509Certificates;
using RoutingProject.CustomConstraints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;


namespace RoutingProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            

            Dictionary<int, string> Countries = new Dictionary<int, string>();
            Countries.Add(1, "United States");
            Countries.Add(2, "Canada");
            Countries.Add(3, "United Kingdom");
            Countries.Add(4, "India");
            Countries.Add(5, "Japan");

            IdConstraint.SetConstraint(Countries.Keys);
            builder.Services.AddRouting(options =>
            {
                options.ConstraintMap.Add("ValidKey", typeof(IdConstraint));

            });

            var app = builder.Build();


            app.UseRouting();

            app.UseEndpoints(
                Endpoints =>
            {
                Endpoints.MapGet("/Countries/{id:ValidKey?}", async context =>
                {
                    int Urlid = Convert.ToInt32(context.Request.RouteValues["id"]);
                    if (Countries.ContainsKey(Urlid))
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync($@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                                <style>
                                    body {{
                                        font-family: 'Segoe UI', sans-serif;
                                        background-color: #f9f9f9;
                                        text-align: center;
                                        padding: 40px;
                                    }}
                                    h2 {{
                                        color: #2980b9;
                                    }}
                                </style>
                            </head>
                            <body>
                                <p>Route: {context.Request.Path}</p>
                                <h2>{Countries[Urlid]}</h2>
                            </body>
                            </html>");

                    }
                });

                Endpoints.MapGet("/Countries", async context =>
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync($@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <style>
                                body {{
                                    font-family: 'Segoe UI', sans-serif;
                                    padding: 40px;
                                    background-color: #f0f8ff;
                                }}
                                h2 {{
                                    color: #34495e;
                                }}
                                ul {{
                                    list-style-type: none;
                                    padding: 0;
                                }}
                                li {{
                                    background: #e0f7fa;
                                    margin: 10px 0;
                                    padding: 10px;
                                    border-radius: 5px;
                                    font-size: 18px;
                                }}
                            </style>
                        </head>
                        <body>
                            <p>Route: {context.Request.Path} </p>
                            <h2>Available Countries:</h2>
                            <ul>");
                    foreach (var country in Countries)
                    {
                        await context.Response.WriteAsync($"<li>{country.Key}: {country.Value}</li>");
                    }
                    await context.Response.WriteAsync("</ul></body></html>");

                });

            });

            app.MapGet("/", async (HttpContext context) => {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync($@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: 'Segoe UI', sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 40px;
                                text-align: center;
                            }}
                            h2 {{
                                color: #2c3e50;
                                background-color: #ecf0f1;
                                padding: 20px;
                                border-radius: 10px;
                                box-shadow: 0 2px 6px rgba(0,0,0,0.1);
                            }}
                        </style>
                    </head>
                    <body>
                        <p>Route: {context.Request.Path}</p>
                        <h2>Routing in ASP.Net Core, isn't it Amazing</h2>
                    </body>
                    </html>");
                });

            app.Run();
        }
    }
}
