using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuilding.Core.Const.ErrorMessages
{
    public class OwnerErrorMessages
    {
        public const string BirthdayLessThan21Years = "Birthday cannot be less than 21 years ago today";
        public const string BirthdayGreaterThan100Years = "Birthday cannot be greater than 100 years ago today";
        public const string BirthdayCannotNull = "Birthday cannot be null";
        public const string NameCannotNull = "Name cannot be null";
        public const string AddressCannotNull = "Address cannot be null";
        public const string OwnerDoesNotExist = "Owner does not exist";
        public const string OwnerUpdateNotAllowed = "Owner update not allowed";

    }
}
