﻿using System;

namespace ZaklepTo.Core.Domain
{
    public class Reservation
    {
        public Guid Id { get; private set; }
        public Restaurant Restaurant { get; private set; }
        public DateTime DateStart { get; private set; }
        public DateTime DateEnd { get; private set; }
        public Table Table { get; private set; }
        public Customer Customer { get; private set; }
        public bool IsConfirmed { get; private set; }
        private bool _isActive;

        public bool IsActive
        {
            get => _isActive && DateTime.Now < DateEnd;
            private set => _isActive = value;
        }

        protected Reservation()
        {
        }

        public Reservation(Restaurant restaurant, DateTime dateStart, DateTime dateEnd, 
            Table table, Customer customer, bool isConfirmed = false, bool isActive = true)
        {
            Id = Guid.NewGuid(); ;
            Restaurant = restaurant;
            DateStart = dateStart;
            DateEnd = dateEnd;
            Table = table;
            Customer = customer;
            IsConfirmed = isConfirmed;
            IsActive = isActive;
        }
    }
}
