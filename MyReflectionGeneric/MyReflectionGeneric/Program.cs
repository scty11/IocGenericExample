using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyReflectionGeneric
{
    class Program
    {
        static void Main(string[] args)
        {
            var employeeList = CreatCollection(typeof(List<>), typeof(Employee));
            Console.WriteLine(employeeList.GetType());
            var genArgs = employeeList.GetType().GenericTypeArguments;

            foreach (var item in genArgs)
            {
                Console.WriteLine(item);
            }

            var employee = new Employee { Name = "Scott" };
            //getting the Employee type info
            var employeeType = typeof(Employee);
            //getting teh method info
            var methodInfo = employeeType.GetMethod("Speak");
            //creating the method with a generic argument
            methodInfo = methodInfo.MakeGenericMethod(typeof(DateTime));
            //invoking the the method on the created employee object.
            methodInfo.Invoke(employee, null);
            Console.Read();
        }

        //takes a list<> with no arguments as the collection type
        //and we use the itemType to craete a closed list collection.
        private static object CreatCollection(Type collectionType, Type itemType)
        {
            var closedType = collectionType.MakeGenericType(itemType);
            return Activator.CreateInstance(closedType);
        }

        
    }
    public class Employee
    {
        public String Name { get; set; }
        public void Speak<T>()
        {
            Console.WriteLine(typeof(T).Name);
        }
    }
}
