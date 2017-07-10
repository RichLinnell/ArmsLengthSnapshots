using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using SnapshotTestingGround.ModelClasses;
using SnapshotTestingGround.Snapshot;

namespace SnapshotTestingGround
{
    class Program
    {
        static void Main(string[] args)
        {
            var myModel = new Model();
            var MS = new ExternalCompany()
            {
                Name = "Microsoft",
            };
            MS.Users.Add(50, new User()
            {
                Company = MS,
                Id = 50,
                Name = "Kevin Carter",
            });
            MS.Users.Add(40, new User()
            {
                Company = MS,
                Id = 40,
                Name = "Marge Simpson",
            });
            var nokia= new ExternalCompany()
            {
                Name = "Nokia",
                ParentCompany = MS,
            };
            nokia.Users.Add(222, new User()
            {
                Name = "Amanda Figg",
                Company=nokia,
                Id = 222,
            });
            myModel.ExternalCompanies.Add(MS);
            myModel.ExternalCompanies.Add(nokia);
            var writer = new Writer(123);
            myModel.Snapshot(writer);
            var json = writer.GetJson();
            Console.WriteLine(json);

            var reader = new Reader();

            reader.Read(json);
            Console.WriteLine("---------------");
            var secondModel = reader.GetCreatedObjects<Model>().SingleOrDefault();

            writer = new Writer(124);
            secondModel.Snapshot(writer);
            var json2 = writer.GetJson();
            Console.WriteLine(json2);

            Console.WriteLine();
            Console.WriteLine("Press any key....");
            Console.ReadLine();
        }

        public static void Reflector()
        {
            var assembly = typeof(Model).Assembly;
            var types = assembly.GetTypes().Where(t => t.IsClass && t.CustomAttributes.Any(a => a.AttributeType == typeof(SnapshotAttribute))).ToArray();

            foreach (var t in types)
            {
                foreach (var field in t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    var fieldName = field.Name;
                    if (fieldName.Contains("BackingField"))
                    {
                        fieldName = fieldName.Substring(fieldName.IndexOf("<") + 1,
                            fieldName.IndexOf(">") - fieldName.IndexOf("<") - 1);
                    }
                    if (field.FieldType.IsValueType || field.FieldType == typeof(string))
                    {
                        Console.WriteLine($"writer.WriteProperty(\"{fieldName}\", {fieldName});");
                    }
                    if (field.FieldType.IsClass && field.FieldType.CustomAttributes.Any(a => a.AttributeType == typeof(SnapshotAttribute)))
                    {
                        Console.WriteLine($"writer.WriteProperty(\"{fieldName}Id\", {fieldName}.Id);");
                    }
                    if (field.FieldType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
                    {
                        Console.WriteLine($"writer.WriteProperty(\"{fieldName}\", ");
                    }
                }
            }

            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }

    }


}
