﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database.Treatment
{
    public class Note
    {
        public int Id { get; private set; }
        public string Text { get; set; }

        public Note(int id, string text)
        {
            Id = id;
            Text = text;
        }
        public Note(int id,Note other) : this(id, other.Text) { }
    }
}
