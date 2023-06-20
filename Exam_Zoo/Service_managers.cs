using System;

namespace Exam_Zoo
{
    public partial class Service_managers
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        DateTime _bDay;
        public DateTime BDay
        {
            get { return _bDay; }
            set { _bDay = value; }
        }

    }
}

