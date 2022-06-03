using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wicked
{
    public class Domain : MonoBehaviour
    {
        [HideInInspector]
        public Character character;

        public List<Location> locations = new List<Location>(4);

        public void Init(Character _character)
        {
            character = _character;

            locations = GetComponentsInChildren<Location>().ToList();

            foreach(Location location in locations)
            {
                location.Init(this);
            }

        }

        public void ActivateLocations()
        {
            foreach (Location location in locations)
            {
                location.TryActivate();
            }
        }

        public void DeactivateLocations()
        {
            foreach (Location location in locations)
            {
                location.Deactivate();
            }
        }
    }
}
