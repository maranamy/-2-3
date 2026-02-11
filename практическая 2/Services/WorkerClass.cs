using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using практическая_2.Services.ValidationAttributes;

namespace практическая_2.Services
{
    public class WorkerClass
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "ФИО - обязательное поле для заполнения!")]
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string FullName { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Должность - обязательное поле для заполнения!")]
        [Required]
        public string Position { get; set; } = string.Empty;

        [BirthDateRange(18, 65)]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Address(5,200)]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Номер телефона - обязательное поле для заполнения!")]
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }



        public string Email { get; set; }


        public DateTime AddedAt { get; set; }


        [Required]
        [MinLength(5)]
        public string Login { get; set; }
        

        [Required]
        [MinLength(8)]
        public string Password { get; set; }



        public WorkerClass(int id,string name,  string position, DateTime? birth, string address, string phone,string email, DateTime added, string login, string password)
        {
            Id = id;
            FullName = name;
            Position = position;
            BirthDate = birth;
            Address = address;
            PhoneNumber = phone;
            Email = email;
            AddedAt = added;
            Login = login;
            Password = password;
        }
    }
}
