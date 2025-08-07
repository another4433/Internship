using System;
using System.Collections.Generic;
using BusinessRestaurant;

public class CustomerViewModel {
    public List<Customer> CustomerList {get; set;} = new List<Customer>();
    public Customer NewCustomer {get; set;} = new Customer();
}