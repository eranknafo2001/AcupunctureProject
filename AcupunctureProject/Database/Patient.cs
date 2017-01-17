using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.database
{
    public class Patient
    {
        public static readonly string NAME = "NAME";
        public static readonly string TELEPHONE = "TELEPHONE";
        public static readonly string CELLPHONE = "CELLPHONE";
        public static readonly string BIRTHDAY = "BIRTHDAY";
        public static readonly string GENDER = "GENDER";
        public static readonly string ADDRESS = "ADDRESS";
        public static readonly string EMAIL = "EMAIL";
        public static readonly string MEDICAL_DESCRIPTION = "MEDICAL_DESCRIPTION";

        public class Gender
        {
            public static readonly Gender MALE = new Gender(0), FEMALE = new Gender(1), OTHER = new Gender(2);

            public int Value { get; private set; }

            private Gender(int value)
            {
                this.Value = value;
            }

            public static Gender FromValue(int value)
            {
                if (value < 0 || value > 2)
                    throw new Exception("ERROR::Gender is not exist");
                return new Gender(value);
            }
        };

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Cellphone { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gend { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string MedicalDescription { get; set; }

        public Patient(int id, string name, string telephone, string cellphone, DateTime birthday, Gender gender,
                string address, string email, string medicalDescription)
        {
            this.Id = id;
            this.Name = name;
            this.Telephone = telephone;
            this.Cellphone = cellphone;
            this.Birthday = birthday;
            this.Gend = gender;
            this.Address = address;
            this.Email = email;
            this.MedicalDescription = medicalDescription;
        }

        public Patient(int id, Patient other)
        {
            this.Id = id;
            this.Name = other.Name;
            this.Telephone = other.Telephone;
            this.Cellphone = other.Cellphone;
            this.Birthday = other.Birthday;
            this.Gend = other.Gend;
            this.Address = other.Address;
            this.Email = other.Email;
            this.MedicalDescription = other.MedicalDescription;
        }

        public Patient(string name, string telephone, string cellphone, DateTime birthday, Gender gender, string address,
                string email, string medicalDescription)
        {
            this.Id = -1;
            this.Name = name;
            this.Telephone = telephone;
            this.Cellphone = cellphone;
            this.Birthday = birthday;
            this.Gend = gender;
            this.Address = address;
            this.Email = email;
            this.MedicalDescription = medicalDescription;
        }
    }
}
