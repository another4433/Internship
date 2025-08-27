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
        var builder = WebApplication.CreateBuilder(args);
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
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=~/Controller/RestaurantLoginController.cs}/{action=~/View/Login/RestaurantLogin.cshtml}"
        );

        app.MapControllerRoute(
            name: "Register",
            pattern: "{controller=~/Controller/RestaurantRegisterController.cs}/{action=RestaurantRegister}"
        );

        app.MapControllerRoute(
            name: "Home",
            pattern: "{controller=~/Controller/RestaurantHomeController.cs}/{action=RestaurantHome}"
        );

        app.MapControllerRoute(
            name: "Profile",
            pattern: "{controller=~/Controller/RestaurantProfileController.cs}/{action=RestaurantProfile}"
        );

        app.MapControllerRoute(
            name: "First",
            pattern: "{controller=~/Controller/RestaurantFirstController.cs}/{action=RestaurantFirstOrder}"
        );

        app.MapControllerRoute(
            name: "Last",
            pattern: "{controller=~/Controller/RestaurantLastController.cs}/{action=RestaurantLastOrder}"
        );

        app.MapControllerRoute(
            name: "Order",
            pattern: "{action=~/Controller/RestaurantViewOrder.cs}/{id?}"
        );
        
        app.MapControllerRoute(
            name: "Search",
            pattern: "{controller=~/Controller/RestaurantSearchController.cs}/{action=RestaurantSearchOrder}"
        );
        
        app.MapControllerRoute(
            name: "Delete",
            pattern: "{controller=~/Controller/RestaurantDeleteController.cs}/{action=RestaurantDeleteOrder}/{id?}"
        );
        
        app.MapControllerRoute(
            name: "Process",
            pattern: "{controller=~/Controller/RestaurantProcessController.cs}/{action=RestaurantProcessOrder}"
        );
        
        app.MapControllerRoute(
            name: "Add",
            pattern: "{controller=~/Controller/RestaurantAddItemController.cs}/{action=RestaurantAddItem}"
        );
        
        app.Run();
    }
}