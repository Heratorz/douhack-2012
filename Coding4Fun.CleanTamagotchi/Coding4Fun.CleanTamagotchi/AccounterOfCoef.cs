using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding4Fun.CleanTamagotchi
{
    public class AccounterOfCoef
    {
        internal static int GetNegativeCoef(Message[] addedMessages)
        {
            var value = 0;
            foreach (var item in addedMessages)
            {
                value -= GetCoef(item);
            }
            return value;
        }

        internal static int GetPositiveCoef(Message[] fixedMessages)
        {
            var value = 0;
            foreach (var item in fixedMessages)
            {
                value += GetCoef(item);
            }
            return value;
        }

        private static int GetCoef(Message messsage)
        {
            int coef = 0;
            if (messsage.Issue.Certainty >= 1 && messsage.Issue.Certainty <= 50)
            {
                coef += 1;
            }
            else if (messsage.Issue.Certainty >= 51 && messsage.Issue.Certainty <= 99)
            {
                coef += 2;
            }

            switch (messsage.Issue.Level)
            {
                case "Error":
                    coef += 2;
                    break;
                case "Warning":
                    coef += 1;
                    break;
            }

            //switch (messsage.FixCategory)
            //{
            //    case "NonBreaking":
            //        coef += 2;
            //        break;
            //    case "Breaking":
            //        coef += 1;
            //        break;
            //}

            return coef;
        }

        internal static int GetTestCoef(bool prevTest, bool curTest)
        {
            if (prevTest && !curTest)
                return -200;
            if (!prevTest && curTest)
                return 200;
            return 0;
        }
    }
}
