using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Const.ErrorMessages
{
    public class PropertyErrorMessages
    {
        public const string PropertyCannotNull = "Property cannot be null";
        public const string PropertyCannotZero = "Property cannot be zero";
        public const string YearCannotNull = "Year cannot be null";
        public const string YearLessThan1900 = "Year cannot be less than 1900";
        public const string NameCannotNull = "Name cannot be null";
        public const string NameLength = "The length of the name must be greater than 3 and less than 100";
        public const string PropertyDoesNotExist = "Property does not exist";
        public const string PropertyUpdateNotAllowed = "Property update not allowed";
    }
}