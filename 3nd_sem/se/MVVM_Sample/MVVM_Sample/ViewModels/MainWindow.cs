using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM_Sample.Models;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using Mutzl.MvvmLight;
using System.Collections.ObjectModel;

namespace MVVM_Sample.ViewModels
{
    public class MainWindow : ViewModelBase
    {
        public MainWindow()
        {
            this.Title = "Student App";

            this.University = new University();

            this.Students = new ObservableCollection<Student>()
            {
               new Student(){ FirstName="Test", LastName="TestLast"},
               new Student(){ FirstName="Master", LastName="Desaster"}
            };

            this.AddStudentCommand = new DependentRelayCommand(AddStudent, CanAddStudent, this, () => this.FirstName, () => this.LastName);
        }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public University University { get; set; }

        public List<Course> Courses { get; set; }

        public ObservableCollection<Student> Students { get; set; }

        public bool CanAddStudent()
        {
            return string.IsNullOrEmpty(this.FirstName) || string.IsNullOrEmpty(this.LastName) ? false : true;
        }

        public void AddStudent()
        {
            this.Students.Add(new Student() { FirstName = this.FirstName, LastName = this.LastName });
            // this.University.Students.Add(new Student() { FirstName = this.FirstName, LastName = this.LastName });
        }

        public ICommand AddStudentCommand
        {
            get;
            private set;
        }

    }
}
