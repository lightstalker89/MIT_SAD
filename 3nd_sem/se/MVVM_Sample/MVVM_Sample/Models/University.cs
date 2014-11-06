using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Sample.Models
{
    public class University
    {
        public string Name { get; set; }

        public List<Course> Courses { get; set; }
    }
}
