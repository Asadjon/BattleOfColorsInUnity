using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Activitys
{
    class ActivitesID
    {
        public readonly static int MENU_ACTIVITY = 1;
        public readonly static int MULTIGAME_ACTIVITY = 3;

        public static int GetId(Type type)
        {
            if (type == typeof(MenuActivity)) return MENU_ACTIVITY;
            else if (type == typeof(MultiGameActivity)) return MULTIGAME_ACTIVITY;

            return 0;
        }
    }
}
