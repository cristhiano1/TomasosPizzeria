﻿namespace TomasosPizzeria.Data.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int DishId { get; set; }
        public Dish Dish { get; set; }

        public int Quantity { get; set; }
    }
}