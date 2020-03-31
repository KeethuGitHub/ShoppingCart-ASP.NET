using System;
using System.Collections.Generic;
using System.Data.Entity;

using ShoppingWebApp.Models;

namespace ShoppingWebApp.DB
{
    public class ShoppingDbInitializer<T> : DropCreateDatabaseAlways<ShoppingDbContext>
    {
        protected override void Seed(ShoppingDbContext context)
        {
            // Initialize non-related properties

            Customer customer = new Customer() {
                FirstName = "Tom",
                LastName = "Tom",
                Gender = "M",
                Username = "username",
                Password = Functions.ComputeSHA256("password"),
                Email="e0457790@u.nus.edu",
                Dateofbirth=new DateTime(1985, 1, 1),
                UserAgreement=true, 
                JoinDate=new DateTime(2019, 10, 2, 8, 0, 0),
                Order=new List<Order>()
            };

            List<Category> categories = new List<Category>()
            {
                new Category() { Name="Data Science", Products=new List<Product>() },
                new Category() { Name="Development", Products=new List<Product>() },
                new Category() { Name="Mathematics", Products=new List<Product>() }
            };

            List<Product> products = new List<Product>()
            {
                new Product() { Name=".NET Analytics", Description="Performs data mining and analytics easily in .NET.", Price=299.00, Orders=new List<Order>() },
                new Product() { Name=".NET Charts", Description="Brings powerful charting applications to your .NET applications.", Price=99.00, Orders=new List<Order>() },
                new Product() { Name=".NET Logger", Description="Logs and aggregates events easily in your .NET apps.", Price=49.00, Orders=new List<Order>() },
                new Product() { Name=".NET ML", Description="Supercharged .NET machine learning libraries.", Price=299.00, Orders=new List<Order>() },
                new Product() { Name=".NET Numerics", Description="Powerful numerical methods for your .NET simulations.", Price=199.00, Orders=new List<Order>() },
                new Product() { Name=".NET PayPal", Description="Integrate your.NET apps with PayPal the easy way!.", Price=69.00, Orders=new List<Order>() }
            };

            List<Order> orders = new List<Order>()
            {
                new Order() { Date=new DateTime(2019,10,3,10,0,0), Price=products[0].Price+products[1].Price, Products=new List<Product>() },
                new Order() { Date=new DateTime(2019,10,3,11,0,0), Price=products[2].Price+products[3].Price, Products=new List<Product>() },
                new Order() { Date=new DateTime(2019,10,3,12,0,0), Price=products[4].Price+products[5].Price, Products=new List<Product>() }
            };

            List<OrderDetail> orderDetails = new List<OrderDetail>()
            {
                new OrderDetail() { Quantity=1 },
                new OrderDetail() { Quantity=1 },
                new OrderDetail() { Quantity=1 },
                new OrderDetail() { Quantity=1 },
                new OrderDetail() { Quantity=1 },
                new OrderDetail() { Quantity=1 }
            };

            List<OrderDetailExtended> orderDetailsExtended = new List<OrderDetailExtended>()
            {
                new OrderDetailExtended { ActivationCode = Guid.NewGuid() },
                new OrderDetailExtended { ActivationCode = Guid.NewGuid() },
                new OrderDetailExtended { ActivationCode = Guid.NewGuid() },
                new OrderDetailExtended { ActivationCode = Guid.NewGuid() },
                new OrderDetailExtended { ActivationCode = Guid.NewGuid() },
                new OrderDetailExtended { ActivationCode = Guid.NewGuid() }
            };

            // Add customer to context
            context.Customer.Add(customer);

            // Add categories to context
            categories[0].Products.Add(products[0]);
            categories[0].Products.Add(products[3]);
            categories[1].Products.Add(products[2]);
            categories[1].Products.Add(products[5]);
            categories[2].Products.Add(products[1]);
            categories[2].Products.Add(products[4]);

            foreach (Category c in categories)
                context.Category.Add(c);

            // Add products to context
            products[0].Category = categories[0];
            products[1].Category = categories[2];
            products[2].Category = categories[1];
            products[3].Category = categories[0];
            products[4].Category = categories[2];
            products[5].Category = categories[1];


            products[0].Orders.Add(orders[0]);
            products[1].Orders.Add(orders[0]);
            products[2].Orders.Add(orders[1]);
            products[3].Orders.Add(orders[1]);
            products[4].Orders.Add(orders[2]);
            products[5].Orders.Add(orders[2]);

            foreach (Product p in products)
                context.Product.Add(p);

            // Add orders to context
            orders[0].Customer = customer;
            orders[1].Customer = customer;

            orders[0].Products.Add(products[0]);
            orders[0].Products.Add(products[1]);
            orders[1].Products.Add(products[2]);
            orders[1].Products.Add(products[3]);
            orders[2].Products.Add(products[4]);
            orders[2].Products.Add(products[5]);

            foreach (Order o in orders)
                context.Order.Add(o);

            // Add order details to context
            orderDetails[0].Order = orders[0];
            orderDetails[0].Product = products[0];
            orderDetails[1].Order = orders[0];
            orderDetails[1].Product = products[1];
            orderDetails[2].Order = orders[1];
            orderDetails[2].Product = products[2];
            orderDetails[3].Order = orders[1];
            orderDetails[3].Product = products[3];
            orderDetails[4].Order = orders[2];
            orderDetails[4].Product = products[4];
            orderDetails[5].Order = orders[2];
            orderDetails[5].Product = products[5];


            foreach (OrderDetail od in orderDetails)
                context.OrderDetail.Add(od);

            // Add order details (extended) to context

            orderDetailsExtended[0].OrderDetail = orderDetails[0];
            orderDetailsExtended[1].OrderDetail = orderDetails[1];
            orderDetailsExtended[2].OrderDetail = orderDetails[2];
            orderDetailsExtended[3].OrderDetail = orderDetails[3];
            orderDetailsExtended[4].OrderDetail = orderDetails[4];
            orderDetailsExtended[5].OrderDetail = orderDetails[5];

            foreach (OrderDetailExtended ode in orderDetailsExtended)
                context.OrderDetailExtended.Add(ode);

            base.Seed(context);
        }
    }
}