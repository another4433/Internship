using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime;
using Data_Structure;

namespace RestaurantIntoWeb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Restaurant Management System!");
            Console.WriteLine("This is a simple console application to manage restaurant operations.");
            WhatIsATree<string> whatIsATree = new WhatIsATree<string>("Welcome to the restaurant");
            whatIsATree.add("Menu");
            whatIsATree.add("Orders");
            whatIsATree.add("Reservations");
            whatIsATree.add("Staff");
            whatIsATree.add("Feedback");
            whatIsATree.add("Suppliers");
            whatIsATree.add("Inventory");
            whatIsATree.printTree();
            MWArrayList<string> menu = new MWArrayList<string>();
            menu.add("Pizza");
            menu.add("Burger");
            menu.add("Pasta");
            menu.add("Salad");
            Console.WriteLine("Menu Items:");
            menu.printList();
        }
    }
}
