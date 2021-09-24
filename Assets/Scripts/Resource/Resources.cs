using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Resource
{
    [CreateAssetMenu(fileName = "New Resources", menuName = "GameProperties/Resources")]
    public class Resources : ScriptableObject
    {
        public List<ViewResources> m_Resources = null;
    }
}
