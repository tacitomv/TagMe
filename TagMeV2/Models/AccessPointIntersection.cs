using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagMeV2.Models;

namespace TagMeV2.Models
{
    public class AccessPointIntersection
    {
        public AccessPointSignal AP1 { get; set; }
        public AccessPointSignal AP2 { get; set; }
        public Location Center { get; set; }
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }

        public AccessPointIntersection(AccessPointSignal ap1, AccessPointSignal ap2)
        {
            this.AP1 = ap1;
            this.AP2 = ap2;
        }

        public bool hasIntersection()
        {
            //Diminuir o erro de cálculo (d1 <= d2)
            AccessPointSignal biggest = null, smallest = null;
            if (AP1.Distance <= AP2.Distance)
            {
                smallest = AP1;
                biggest = AP2;
            }
            else
            {
                smallest = AP2;
                biggest = AP1;
            }

            DeltaX = biggest.AP.Location._x - smallest.AP.Location._x;
            DeltaY = biggest.AP.Location._y - smallest.AP.Location._y;
            double euclidean = Math.Sqrt(Math.Abs(Math.Pow(DeltaX, 2) + Math.Pow(DeltaY, 2)));
            if (euclidean > biggest.Distance + smallest.Distance)
            {
                //circulos não intersectam (um longe do outro)
                return false;
            }
            else if (euclidean < biggest.Distance - smallest.Distance)
            {
                //circulos não intersectam (um dentro do outro)
                return false;
            }
            else
            {
                //então temos uma intersecção
                Center._x = smallest.AP.Location._x + ((Math.Pow(DeltaX, 2) + smallest.Distance - biggest.Distance) / (2 * DeltaX)) / DeltaX;
                Center._y = smallest.AP.Location._y + ((Math.Pow(DeltaY, 2) + smallest.Distance - biggest.Distance) / (2 * DeltaY)) / DeltaY;
            }
            return true;
        }
    }
}
