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
                Value = value;
            }

            public static Gender FromValue(int value)
            {
                switch (value)
                {
                    case 0:
                        return MALE;
                    case 1:
                        return FEMALE;
                    case 2:
                        return OTHER;
                    default:
                        throw new Exception("ERROR::Gender is not exist");
                }
            }

            public override string ToString()
            {
                switch (Value)
                {
                    case 0:
                        return "זכר";
                    case 1:
                        return "נקבה";
                    case 2:
                        return "אחר";
                    default:
                        return null;

                }
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

        public Patient(int id, Patient other) : this(id, other.Name, other.Telephone, other.Cellphone, other.Birthday, other.Gend, other.Address, other.Email, other.MedicalDescription)
        {
        }

        public Patient(string name, string telephone, string cellphone, DateTime birthday, Gender gender, string address,
                string email, string medicalDescription) :
            this(-1, name, telephone, cellphone, birthday, gender, address, email, medicalDescription)
        {
        }
    }
}
