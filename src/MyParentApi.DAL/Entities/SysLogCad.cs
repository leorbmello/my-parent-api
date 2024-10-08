﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("SysLogOper")]
    public class SysLogOper
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Operation { get; set; }
        public string JsonText { get; set; }
        public string ErrorText { get; set; }
        public DateTime Date { get; set; }
    }
}
