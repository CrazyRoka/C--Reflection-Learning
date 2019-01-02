using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WhatsNewAttributes;

namespace LookUpWhatsNew
{
    class Program
    {
        private static readonly StringBuilder outputText = new StringBuilder(1000);
        private static DateTime backDateTo = new DateTime(2017, 2, 1);
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.Load(new AssemblyName("VectorClass"));
            Attribute supportsAttribute = assembly.GetCustomAttribute(typeof(SupportsWhatsNewAttribute));

            AddToOutput($"Assembly {assembly.FullName}");
            if(supportsAttribute == null)
            {
                AddToOutput("This assembly does not support WhatsNew attributes");
                return;
            }
            else
            {
                AddToOutput("Defined types:");
            }

            IEnumerable<Type> types = assembly.ExportedTypes;
            foreach(Type definedType in types)
            {
                DisplayTypeInfo(definedType);
            }

            Console.WriteLine($"What's New since {backDateTo:D}");
            Console.WriteLine(outputText.ToString());
            Console.ReadLine();
        }

        private static void DisplayTypeInfo(Type type)
        {
            if (!type.GetTypeInfo().IsClass)
            {
                return;
            }

            AddToOutput($"Class {type.Name}");
            IEnumerable<LastModifiedAttribute> lastModifiedAttributes =
                type.GetTypeInfo().GetCustomAttributes()
                .OfType<LastModifiedAttribute>()
                .Where(a => a.DateModified >= backDateTo);

            if(lastModifiedAttributes.Count() == 0)
            {
                AddToOutput($"\tNo changes to the class {type.Name}");
            }
            else
            {
                foreach(LastModifiedAttribute attribute in lastModifiedAttributes)
                {
                    WriteAttributeInfo(attribute);
                }
            }

            AddToOutput("Changes to methods of this class:");

            foreach(MethodInfo method in type.GetTypeInfo().DeclaredMembers.OfType<MethodInfo>())
            {
                IEnumerable<LastModifiedAttribute> attributesToMethods =
                    method.GetCustomAttributes().OfType<LastModifiedAttribute>()
                    .Where(a => a.DateModified >= backDateTo);

                if(attributesToMethods.Count() > 0)
                {
                    AddToOutput($"{method.ReturnType} {method.Name}()");
                    foreach(Attribute attribute in attributesToMethods)
                    {
                        WriteAttributeInfo(attribute);
                    }
                }
            }
        }

        private static void WriteAttributeInfo(Attribute attribute)
        {
            if(attribute is LastModifiedAttribute lastModifiedAttribute)
            {
                AddToOutput($"\tmodified: {lastModifiedAttribute.DateModified:D}: {lastModifiedAttribute.Changes}");
                if (lastModifiedAttribute.Issues != null)
                {
                    AddToOutput($"\tOutstanding issues: {lastModifiedAttribute.Issues}");
                }
            }
        }

        static void AddToOutput(string text) => outputText.Append($"{Environment.NewLine}{text}");
    }
}
