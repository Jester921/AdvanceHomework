using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AdvanceHomework
{
    public class ProductDB : DbContext
    {
        public ProductDB()
            : base("name=ProductDB")
        {
        }

        public virtual DbSet<ProductInfo> ProductInfo { get; set; }
        public virtual DbSet<ClientInfo> Clients { get; set; }

        public virtual DbSet<ManegersInfo> Manegers { get; set; }
    }

    public class ProductInfo
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public float Cost { get; set; }
        public ProductInfo(int id, string productName, float cost)
        {
            Id = id;
            ProductName = productName;
            Cost = cost;
        }
        public ProductInfo()
        {

        }
    }

    public class ClientInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public int TelephoneNumber { get;  set; }
        public string Login { get; set; }

        public string Password { get; set; }


        public ClientInfo(int id, string firstName, string lastName, int telephoneNumber, string login, string password)
        {
            this.Id = id;
            this.Name = firstName;
            this.LastName = lastName;
            this.TelephoneNumber = telephoneNumber;
            this.Login = login;
            this.Password = password;
        }
        public ClientInfo()
        {

        }
    }
    public class ManegersInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
        /*LoginPassword loginPassword { get; set; }*/

        public ManegersInfo(int id, string firstName, string lastName, string login, string password)
        {
            this.Id = id;
            this.Name = firstName;
            this.LastName = lastName;
            this.Login= login;
            this.Password = password;
        }

        public ManegersInfo()
        {

        }
    }

    /*public class Orders
    {
        public int Id { get; set; }
        ICollection<ProductInfo> products;
        ClientInfo client;
        ManegersInfo maneger;
        public string OrderStatus { get; set; }

        public Orders()
        {
            products = new List<ProductInfo>();
        }
    }*/


    public class LoginPassword
    {
        public string Login { get; set;}
        public string Password { get; set;}
    }
}