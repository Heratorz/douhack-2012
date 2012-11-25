using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding4Fun.CleanTamagotchi
{
    public class MessageComparer : IEqualityComparer<Message>
    {
        public bool Equals(Message x, Message y)
        {
            return x.Category == y.Category
                && x.CheckId == y.CheckId
                && x.FixCategory == y.FixCategory
                && x.Status == y.Status
                && x.TypeName == y.TypeName
                && x.Issue.Certainty == y.Issue.Certainty
                && x.Issue.File == y.Issue.File
                && x.Issue.Name == y.Issue.Name
                && x.Issue.Path == y.Issue.Path;
        }

        public int GetHashCode(Message obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return obj.GetHashCode();
        }
    }
}
