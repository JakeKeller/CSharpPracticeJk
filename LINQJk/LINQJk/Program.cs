using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQJk
{
    class Program
    {
        /// <summary>
        /// Demonstrations of LINQ to object queries. Mostly adapted from C# Had First, O'Reilly and from MSDN.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            // LINQ allows me to change the result of my query within the LINQ statement.
            Console.WriteLine("\n----1----\n");

            string[] sandwiches = { "Tuna", "Chicken", "Beef" };
            var extraCheeseSandwiches = from sandwich in sandwiches
                                   select sandwich + " with extra cheese!";

            foreach (var sandwich in extraCheeseSandwiches)
                Console.WriteLine(sandwich);

            // Extension Methods. LINQ offers various extension methods (that are actually part of LINQ not of the IENumerable class according
            // to C# from head to toe):

            Console.WriteLine("\n----2----\n");

            Random random = new Random();
            List<int> listWithNumbers = new List<int>();
            int length = random.Next(50, 150);
            for (int i = 0; i < length; i++)
                listWithNumbers.Add(random.Next(100));

            Console.WriteLine("There are {0} numbers", listWithNumbers.Count());
            Console.WriteLine("The smallest is {0}", listWithNumbers.Min());
            Console.WriteLine("The biggest is {0}", listWithNumbers.Max());
            Console.WriteLine("Their sum is {0}", listWithNumbers.Sum());
            Console.WriteLine("Thir average is {0:F2}", listWithNumbers.Average());

            // I can save the result of a LINQ query into a list. This causes the query to be executed right away (normally LINQ queries
            // are not actually run until their result is used somewhere). Apparently I could also use ToArray() or ToDictionary() but why 
            // would I?

            Console.WriteLine("\n----3----\n");

            var under50sorted = from number in listWithNumbers
                                where number < 50
                                orderby number descending
                                select number;
            List<int> newLIst = under50sorted.ToList();

            // You can use .Take() to get a chosen number of results from the query (starting at the beginning of the list).

            var firstFive = under50sorted.Take(5);

            List<int> shortList = firstFive.ToList();
            foreach (int n in shortList)
                Console.WriteLine(n);

            // You can use "select" to get back a different name and format than was originally used.
            // Let's say you have an int array of values of items. You can use LINQ to get a (formatted) price back.
            // *Currency formatting currently needs the code below, I assume that's due to conflicting windows language
            // and localization settings.

            Console.WriteLine("\n----4----\n");

            // Just some operations with LINQ queries. Who comes up with these things?!
            int newLength = random.Next(50, 100);
            int[] itemValues = new int[newLength];

            for (int i = 0; i < newLength; i++)
                itemValues[i] = random.Next(100);
            
            var itemPrices = from value in itemValues
                             orderby value
                             select String.Format(new System.Globalization.CultureInfo("en-US"), "{0:c}", value);
            foreach (var price in itemPrices.Take(5))
                Console.WriteLine(price);

            Console.WriteLine("\n----5----\n");

            int[] badgers = { 36, 5, 91, 3, 41, 69, 8 };

            var mountainlion = from dove in badgers
                               where (dove != 36 && dove < 50)
                               orderby dove descending
                               select dove + 5;

            var bears = mountainlion.Take(3);

            var weasel = from owl in bears
                         select owl - 1;

            Console.WriteLine("Have fun on route {0}", weasel.Sum());

            // LINQ can sort results into groups. "group" returns a sequence of sequences.
            // This example is from: https://msdn.microsoft.com/en-us/library/bb397900.aspx?f=255&MSPPError=-2147217396
            // The class student is at the bottom of the Program.cs
            // Create a data source by using a collection initializer.

            Console.WriteLine("\n----5----\n");

            List<Student> students = new List<Student>
                {
                   new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores= new List<int> {97, 92, 81, 60}},
                   new Student {First="Claire", Last="O'Donnell", ID=112, Scores= new List<int> {75, 84, 91, 39}},
                   new Student {First="Sven", Last="Mortensen", ID=113, Scores= new List<int> {88, 94, 65, 91}},
                   new Student {First="Cesar", Last="Garcia", ID=114, Scores= new List<int> {97, 89, 85, 82}},
                   new Student {First="Debra", Last="Garcia", ID=115, Scores= new List<int> {35, 72, 91, 70}},
                   new Student {First="Fadi", Last="Fakhouri", ID=116, Scores= new List<int> {99, 86, 90, 94}},
                   new Student {First="Hanying", Last="Feng", ID=117, Scores= new List<int> {93, 92, 80, 87}},
                   new Student {First="Hugo", Last="Garcia", ID=118, Scores= new List<int> {92, 90, 83, 78}},
                   new Student {First="Lance", Last="Tucker", ID=119, Scores= new List<int> {68, 79, 88, 92}},
                   new Student {First="Terry", Last="Adams", ID=120, Scores= new List<int> {99, 82, 81, 79}},
                   new Student {First="Eugene", Last="Zabokritski", ID=121, Scores= new List<int> {96, 85, 91, 60}},
                   new Student {First="Michael", Last="Tucker", ID=122, Scores= new List<int> {94, 92, 91, 91} }
                };

            students.Add(new Student { First = "Jake", Last = "Keller", ID = 123, Scores = new List<int> { 20, 80, 22, 54 } });

            //Simple query
            //var studentQuery = from student in students
            //                    where student.Scores[0] > 80
            //                    select student;
            //foreach (var student in studentQuery)
            //    Console.WriteLine("Student {0} scored a {1}!", student.Last, student.Scores[0]);

            /*
            "Grouping is a powerful capability in query expressions. 
            A query with a group clause produces a sequence of groups, 
            and each group itself contains a Key and a sequence that consists 
            of all the members of that group. The following new query groups 
            the students by using the first letter of their last name as the key.
            */

            // studentQuery2 is an IEnumerable<IGrouping<char, Student>>
            var studentQuery2 = from student in students
                                group student by student.Last[0];

            //// To order it I can do this:
            //var studentQuery2 = from student in students
            //                    orderby student.Last[0]
            //                    group student by student.Last[0];
            
            // Or this, using "into" and studentGroup.Key
            //var studentQuery2 =
            //    from student in students
            //        // orderby student.Last[0]
            //    group student by student.Last[0] into studentGroup
            //    orderby studentGroup.Key
            //    select studentGroup;

            /*
            "Note that the type of the query has now changed. 
            It now produces a sequence of groups that have a 
            char type as a key, and a sequence of Student objects. 
            Because the type of the query has changed, the 
            following code changes the foreach execution loop also:"
            */

            foreach (var studentGroup in studentQuery2)
            {
                Console.WriteLine(studentGroup.Key);
                foreach (Student student in studentGroup)
                {
                    Console.WriteLine("    {0}, {1}",
                              student.Last, student.First);
                }
            }

            /* 
            I can introduce an indentifier by using "let".
             
            "You can use the let keyword to introduce an identifier 
            for any expression result in the query expression.
            This identifier can be a convenience, as in the following 
            example, or it can enhance performance by storing the results 
            of an expression so that it does not have to be calculated multiple times."
            
            studentQuery5 is an IEnumerable<string>
            This query returns those students whose
            first test score was higher than their
            average score:
            */

            Console.WriteLine("\n----6----\n");

            var studentQuery5 =
                from student in students
                let totalScore = student.Scores[0] + student.Scores[1] +
                    student.Scores[2] + student.Scores[3] // Here they could have used "student.Scores.Sum()"
                where totalScore / 4 < student.Scores[0]
                select student.Last + " " + student.First;

            foreach (string s in studentQuery5)
                Console.WriteLine(s);

            /*
            To use method syntax in a query expression:

            "As described in Query Syntax and Method Syntax in LINQ (C#), 
            some query operations can only be expressed by using method 
            syntax. The following code calculates the total score for each 
            Student in the source sequence, and calls the Average() 
            method on the results of that query to calculate the average 
            score of the class. Note the placement of parentheses around 
            the query expression.
            */
            
            Console.WriteLine("\n----7----\n");
                        
            double averageScore = (from student in students
                                   let totalScore = student.Scores[0] + student.Scores[1] +
                                   student.Scores[2] + student.Scores[3]
                                   select totalScore).Average();
            Console.WriteLine("Class average score = {0}", averageScore);




            Console.ReadLine();

        }
    }

    public class Student
    {
        public string First { get; set; }
        public string Last { get; set; }
        public int ID { get; set; }
        public List<int> Scores;
    }
}
