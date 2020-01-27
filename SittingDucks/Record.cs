﻿using System;

namespace SittingDucks
{
    public class Record
    {
        public Record(string website, string accountName, string password)
        {
            this.Website = website;
            this.AccountName = accountName;
            this.Password = password;
            this.ID = Guid.NewGuid();
        }

        public Record() { }

        public string Website { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public Guid ID { get; set; }
    }
}
