﻿using System;
using System.ComponentModel.DataAnnotations;

namespace IndyBooks.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string SKU { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public Writer Author { get; set; }

    }
}
