﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
     [Table(Name = "AddressTypes")]
    public class AddressType : Sars.Systems.Data.DataAccessObject
    {
        [Column(Name = "Code"), DBQueryParameter("@Code")]
        public string Code {get; set;}
        [Column(Name = "Description"), DBQueryParameter("@Description")]
        public string Description{get; set;}
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp{get; set;}
        [Column(Name = "IsActive"), DBQueryParameter("@IsActive")]
        public bool IsActive{get; set;}
        
    }
}
