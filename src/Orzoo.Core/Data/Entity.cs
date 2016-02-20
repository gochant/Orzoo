using System;
using System.ComponentModel.DataAnnotations;

namespace Orzoo.Core.Data
{

    public class Entity : IEntity
    {
        protected string id;

        [UIHint("Hidden")]
        public virtual string Id
        {
            get
            {
                return id;
            }

            set
            {
                if (value != null)
                {
                    id = value;
                }
            }
        }

        public Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

}
