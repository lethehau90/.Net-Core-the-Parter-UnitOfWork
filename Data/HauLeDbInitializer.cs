using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public static class HauLeDbInitializer
    {


        public static void Initialize(HauLeDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any())
            {
                return; //Db hass been seeded
            }


            //Insert for student
            if (context.Students.Count() == 0)
            {
                var students = new Student[]
                {
                    new Student{Name="Carson",EnrollmentDate=DateTime.Parse("2005-09-01")},
                    new Student{Name="Meredith",EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{Name="Arturo",EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{Name="Gytis",EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{Name="Yan",EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{Name="Peggy",EnrollmentDate=DateTime.Parse("2001-09-01")},
                    new Student{Name="Laura",EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{Name="Nino",EnrollmentDate=DateTime.Parse("2005-09-01")}
            };
                foreach (Student s in students)
                {
                    context.Students.Add(s);
                }
            }

            //insert for Course

            var courses = new Course[]
            {
                new Course{Name="Chemistry",Credit=3},
                new Course{Name="Microeconomics",Credit=3},
                new Course{Name="Macroeconomics",Credit=3},
                new Course{Name="Calculus",Credit=4},
                new Course{Name="Trigonometry",Credit=4},
                new Course{Name="Composition",Credit=3},
                new Course{Name="Literature",Credit=4}
             };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            //insert for enrollments
            var enrollments = new Enrollment[]
            {
                new Enrollment{StudentId=1,CourseId=1,Grade=Grade.A},
                new Enrollment{StudentId=1,CourseId=1,Grade=Grade.C},
                new Enrollment{StudentId=1,CourseId=2,Grade=Grade.B},
                new Enrollment{StudentId=2,CourseId=2,Grade=Grade.B},
                new Enrollment{StudentId=2,CourseId=2,Grade=Grade.F},
                new Enrollment{StudentId=2,CourseId=3,Grade=Grade.F},
                new Enrollment{StudentId=3,CourseId=3},
                new Enrollment{StudentId=4,CourseId=3},
                new Enrollment{StudentId=4,CourseId=4,Grade=Grade.F},
                new Enrollment{StudentId=5,CourseId=4,Grade=Grade.C},
                new Enrollment{StudentId=6,CourseId=4},
                new Enrollment{StudentId=7,CourseId=4,Grade=Grade.A},
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
                context.SaveChanges();

        }

        
    }
}
