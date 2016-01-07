using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RentaMaq.Models;

namespace RentaMaq.DAL
{
    public class Inicializador : System.Data.Entity.DropCreateDatabaseIfModelChanges<Context>
    {

        protected override void Seed(Context context)
        {
            //base.Seed(context);
        }

    }
}