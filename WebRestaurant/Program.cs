using Data_Structure;
using BusinessRestaurant;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebRestaurant;

internal abstract class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Console.WriteLine(args);
        Customer customer = new Customer();
        LWLinkedList<Customer> customers = new LWLinkedList<Customer>();
        customers.add(customer);
        customers.printList();
        User user = new User();
        List<User> users = [user];
        foreach (User item in users)
        {
            Console.WriteLine(item);
        }
        
        //Apply this to all of your web pages
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=RestaurantLogin}/{id?}"
        );

        app.MapControllerRoute(
            name: "register",
            pattern: "{controller=Register}/{action=RestaurantRegister}/{id?}"
        );

        app.MapControllerRoute(
            name: "home",
            pattern: "{controller=Home}/{action=RestaurantHome}/{id?}"
        );

        app.MapControllerRoute(
            name: "profile",
            pattern: "{controller=Profile}/{action=RestaurantProfile}/{id?}"
        );

        app.MapControllerRoute(
            name: "first",
            pattern: "{controller=First}/{action=RestaurantFirstOrder}/{id?}"
        );

        app.MapControllerRoute(
            name: "last",
            pattern: "{controller=Last}/{action=RestaurantLastOrder}/{id?}"
        );

        app.MapControllerRoute(
            name: "view",
            pattern: "{controller=View}/{action=RestaurantViewOrder}"
        );

        app.MapControllerRoute(
            name: "search",
            pattern: "{controller=Search}/{action=RestaurantSearchOrder}"
        );

        app.MapControllerRoute(
            name: "delete",
            pattern: "{controller=Delete}/{action=RestaurantDeleteOrder}"
        );

        app.MapControllerRoute(
            name: "process",
            pattern: "{controller=Delete}/{action=RestaurantProcessOrder}"
        );
        
        app.Run();
    }
}