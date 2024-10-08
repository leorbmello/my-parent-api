﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("UserLogOper")]
    public class UserLogOper
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Operation { get; set; }
        public string JsonText { get; set; }
        public DateTime Date { get; set; }
    }
}
