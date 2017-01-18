﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.database
{
    public class Symptom
    {
        public static readonly string NAME = "NAME";
	    public static readonly string COMMENT = "COMMENT";

	    public int Id { get; private set; }
        public string Name { get; set; }
        public string Comment { get; set; }

        public Symptom(int Id, string Name, string Comment)
        {
            this.Id = Id;
            this.Name = Name;
            this.Comment = Comment;
        }

        public Symptom(string Name, string Comment)
        {
            this.Id = -1;
            this.Name = Name;
            this.Comment = Comment;
        }

        public Symptom(int Id, Symptom other)
        {
            this.Id = Id;
            this.Name = other.Name;
            this.Comment = other.Comment;
        }
    }
}