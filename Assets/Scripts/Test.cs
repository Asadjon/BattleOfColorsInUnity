using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class Test : MonoBehaviour
    {
        public void Click(int i)
        {
            ActivityManager.GetActivityManager.UnLoadActivity(i);
        }
    }
}
