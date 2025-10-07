using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practices
{
    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public int Personal_ID_Number { get; set; }
        public Home Home { get; set; }
        public Person(int age, string name, int personalIdNumber)
        {
            Age = age;
            Name = name;
            Personal_ID_Number = personalIdNumber;
        }
    }
}
