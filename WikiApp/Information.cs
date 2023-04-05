using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiApp
{
    [Serializable]
    public class Information : IComparable<Information>
    {
        // 6.1 Create a separate class file to hold the four data items of the Data Structure (use the Data Structure Matrix as a guide).
        // Use private properties for the fields which must be of type “string”. The class file must have separate setters and getters, add an appropriate IComparable for the Name attribute.
        // Save the class as “Information.cs”.
        private string name;
        private string category;
        private string structure;
        private string definition;

        public Information() { }

        public string GetName() { return name; }
        public void SetName(string setName) { name = setName; }

        public string GetCategory() { return category; }
        public void SetCategory(string setCategory) { category = setCategory; }

        public string GetStructure() { return structure; }
        public void SetStructure(string setStructure) { structure = setStructure; }

        public string GetDefinition() { return definition; }
        public void SetDefinition(string setDefinition) { definition = setDefinition; }


        public int CompareTo(Information other)
        {
            return name.CompareTo(other.GetName());
        }
    }
}
