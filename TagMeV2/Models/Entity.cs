using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagMeV2.Models
{
    public class Entity : IEquatable<Entity>
    {

        public virtual Guid _id { get; set; }

        public virtual bool Equals(Entity other)
        {
            if (other == null)
            {
                return false;
            }

            if (other._id == default(Guid) || this._id == default(Guid))
            {
                return false;
            }

            return this._id.Equals(other._id);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Entity);
        }
    }
}