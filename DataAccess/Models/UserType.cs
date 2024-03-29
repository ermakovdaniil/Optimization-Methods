﻿using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new ObservableCollection<User>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ObservableCollection<User> Users { get; set; }
    }
}